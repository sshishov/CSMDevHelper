using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.Collections;

namespace CSMDevHelper
{
    public enum LogCode: int
    {
        LOG_NOTHING = 0,
        LOG_EVENT = 1,
        LOG_LEG = 2,
        LOG_MODELING = 3,
    };

    public class LogResult
    {
        public LogCode code;
        public object result;
        public string timestamp;

        public LogResult(LogCode _code, object _result, string _timestamp)
        {
            code = _code;
            result = _result;
            timestamp = _timestamp;
        }
    }


    abstract class LogReader
    {
        protected FileStream m_fileStream;
        protected StreamReader m_streamReader;
        protected static string logHeaderPattern = @"
            (?<DATE>\d{1,2}/\d{1,2}/\d{1,4})\s+
            (?<TIME>\d{1,2}:\d{1,2}:\d{1,2}\s(PM|AM|))\s*
            (?<UNK1>\w+)\s+
            (?<UNK2>\w+)\s+:\s*
            (?<MESSAGE>.*)";

        protected static string logModelingPattern = @"^=+";
        protected bool m_isModeling;
        protected CSMEvent csmevent;

        protected LogReader(string filename, bool fromBeginning)
        {
            this.m_fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            this.m_streamReader = new StreamReader(m_fileStream);
            this.m_isModeling = false;
            if (fromBeginning)
            {
                this.m_fileStream.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                this.m_fileStream.Seek(0, SeekOrigin.End);
            }
        }

        ~LogReader() { }


        public virtual LogResult Process()
        {
            string result = String.Empty;
            string timestamp = String.Empty;
            LogCode code = LogCode.LOG_NOTHING;
            Match logMatch;

            result = this.m_streamReader.ReadLine();
            if (result != null && result != String.Empty)
            {
                logMatch = Regex.Match(result, LogReader.logHeaderPattern, RegexOptions.IgnorePatternWhitespace);
                if (logMatch.Success)
                {
                    result = logMatch.Groups["MESSAGE"].Value;
                    timestamp = logMatch.Groups["DATE"].Value + " " + logMatch.Groups["TIME"].Value; ;
                    code = LogCode.LOG_EVENT;
                }
                else
                {
                    if (Regex.Match(result, LogReader.logModelingPattern, RegexOptions.IgnorePatternWhitespace).Success)
                    {
                        this.m_isModeling = !this.m_isModeling;
                    }
                    if (this.m_isModeling)
                    {
                        if (csmevent != null)
                        {
                            if (csmevent.eventInfo.Modeling == String.Empty)
                            {
                                csmevent.eventInfo.Modeling += result;
                            }
                            else
                            {
                                csmevent.eventInfo.Modeling += Environment.NewLine + result;
                            }
                        }
                        else
                        {
                            code = LogCode.LOG_NOTHING;
                        }
                    }
                }
            }
            return new LogResult(code, result, timestamp);
        }
    }

    class LogMCDReader : LogReader
    {
        private static string msgPattern = @"Lay7Dec\|\s*(?<MESSAGE>{\s*""MitaiEvent"".*)";

        public LogMCDReader(string filename, bool fromBeginning): base(filename, fromBeginning) {}

        private string jsonWorkAround(string jsonString)
        {
            // Workaround for incorrectly boolean variables
            jsonString = jsonString.Replace("FALSE", "false");
            jsonString = jsonString.Replace("TRUE", "true");

            // Workaround for old versions (DropEvent w/o comma)
            jsonString = jsonString.Replace(@"false""", @"false,""");
            // Workaround for hex integers
            jsonString = Regex.Replace(jsonString, @"(?<=\s)(\w+)(?=,)", "\"$1\"");
            return jsonString;
        }

        public override LogResult Process()
        {
            LogResult logResult;
            logResult = base.Process();
            switch (logResult.code)
            {
                case LogCode.LOG_EVENT:
                    string jsonString = (string)logResult.result;
                    csmevent = new CSMEvent();
                    csmevent.eventInfo.TimeStamp = logResult.timestamp;
                    Dictionary<string, object> jsonDict;
                    object outObject;
                    Match eventMatch;
                    try
                    {
                        eventMatch = Regex.Match(jsonString, LogMCDReader.msgPattern, RegexOptions.IgnorePatternWhitespace);
                        if (eventMatch.Success)
                        {
                            jsonString = eventMatch.Groups["MESSAGE"].Value;
                            // Workaround for parked events
                            if (jsonString.Contains("{*** Processing Parked Event ***}"))
                            {
                                jsonString = jsonString.Replace("{*** Processing Parked Event ***}", "");
                                csmevent.eventInfo.isParked = true;
                            }
                            jsonString = jsonWorkAround(jsonString);
                            JavaScriptSerializer mySer = new JavaScriptSerializer();
                            jsonDict = mySer.Deserialize<Dictionary<string, object>>(jsonString);
                            jsonDict = (Dictionary<string, object>)jsonDict["MitaiEvent"];
                            csmevent.node = GenerateTree(jsonDict);
                            if (jsonDict.TryGetValue("Type", out outObject))
                            {
                                eventMatch = Regex.Match((string)jsonDict["Type"], @"^(?<TYPE>\D+)\((?<TYPENUM>\d+)\)");
                                if (eventMatch.Success)
                                {
                                    csmevent.eventInfo.Type = eventMatch.Groups["TYPE"].Value;
                                    csmevent.eventInfo.TypeNumber = eventMatch.Groups["TYPENUM"].Value;
                                }
                            }
                            if (jsonDict.TryGetValue("Cause", out outObject))
                            {
                                eventMatch = Regex.Match((string)outObject, @"^(?<CAUSE>\D+)\((?<CAUSENUM>\d+)\)");
                                if (eventMatch.Success)
                                {
                                    csmevent.eventInfo.Cause = eventMatch.Groups["CAUSE"].Value;
                                    csmevent.eventInfo.CauseNumber = eventMatch.Groups["CAUSENUM"].Value;
                                }
                            }
                            if (jsonDict.TryGetValue("MonitorHandle", out outObject))
                            {
                                eventMatch = Regex.Match((string)outObject, @"^(?<MONITOR>.+)\((?<MONITORNUM>.*)\)");
                                if (eventMatch.Success)
                                {
                                    csmevent.eventInfo.MonitorHandler = eventMatch.Groups["MONITOR"].Value;
                                    csmevent.eventInfo.MonitorHandlerExtension = eventMatch.Groups["MONITORNUM"].Value;
                                    eventMatch = Regex.Match(eventMatch.Groups["MONITORNUM"].Value, @"(?<MONITORNODE>.*)\|(?<MONITORNUM>.*)");
                                    if (eventMatch.Success)
                                    {
                                        csmevent.eventInfo.MonitorHandlerNode = eventMatch.Groups["MONITORNODE"].Value;
                                        csmevent.eventInfo.MonitorHandlerExtension = eventMatch.Groups["MONITORNUM"].Value;
                                    }
                                }
                            }
                            if (jsonDict.TryGetValue("CGCID", out outObject))
                            {
                                csmevent.eventInfo.CGCID = (string)outObject;
                            }
                            if (jsonDict.TryGetValue("PGCID", out outObject))
                            {
                                csmevent.eventInfo.PGCID = (string)outObject;
                            }
                            if (jsonDict.TryGetValue("SGCID", out outObject))
                            {
                                csmevent.eventInfo.SGCID = (string)outObject;
                            }
                        }
                        else
                        {
                            logResult.code = LogCode.LOG_NOTHING;
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        csmevent.eventInfo.Type = "Exception";
                        csmevent.eventInfo.Cause = "Invalid JSON";
                        csmevent.node = new TreeNode[] { new TreeNode(String.Format("Exception: {0}", ex.Message)), new TreeNode(String.Format("JSON: {0}", jsonString)) };
                    }
                    logResult.result = csmevent;
                    break;
                default:
                    break;
            }
            return logResult;
        }
        private TreeNode[] GenerateTree(Dictionary<string, object> jsonDict)
        {
            int counter = 0;
            TreeNode[] currentNodes = new TreeNode[jsonDict.Count];
            foreach (string dictKey in jsonDict.Keys)
            {
                TreeNode currentNode = new TreeNode();
                string strNode = "";

                object dictValue = jsonDict[dictKey];
                // Processing if value is dictionary
                if ((object)dictValue is Dictionary<string, object>)
                {
                    currentNode.Nodes.AddRange(GenerateTree((Dictionary<string, object>)dictValue));
                    strNode = dictKey;
                }
                // Processing if value is list
                else if ((Object)dictValue is ArrayList)
                {
                    foreach (Dictionary<string, object> node in (ArrayList)dictValue)
                    {
                        currentNode.Nodes.AddRange(GenerateTree((Dictionary<string, object>)node));
                    }
                    strNode = dictKey;
                }
                // Processing if value is atomic
                else
                {
                    strNode = String.Format("{0}: {1}", dictKey, jsonDict[dictKey]);
                }
                currentNode.Text = strNode;
                currentNodes[counter++] = currentNode;
            }
            return currentNodes;
        }

    }

    class LogCPReader : LogReader
    {
        private static string msgPattern = @"Rec:\s(?<MONITOR>[^\s]+)\s=>(?<G1>[^,]+),(?<G2>[^,]+),(?<G3>[^,]+),(?<G4>[^,]+),(?<G5>[^,]+).*";
        
        public LogCPReader(string filename, bool fromBeginning): base(filename, fromBeginning){}

        public override LogResult Process()
        {
            LogResult logResult;
            logResult = base.Process();
            switch (logResult.code)
            {
                case LogCode.LOG_EVENT:
                    Match eventMatch;
                    string csvstring = (string)logResult.result;
                    eventMatch = Regex.Match(csvstring, LogCPReader.msgPattern, RegexOptions.IgnorePatternWhitespace);
                    if (eventMatch.Success)
                    {
                        csmevent = new CSMEvent();
                        logResult.result = csmevent;
                    }
                    else
                    {
                        logResult.code = LogCode.LOG_NOTHING;
                    }
                    break;
            }
            return logResult;
        }
    }
}
