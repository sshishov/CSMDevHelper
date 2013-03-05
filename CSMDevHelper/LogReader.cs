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
            (?<DATE>\d{1,2}(/|\.)\d{1,2}(/|\.)\d{1,4})\s+
            (?<TIME>\d{1,2}:\d{1,2}:\d{1,2}\s(PM|AM|))\s*
            (?<UNK1>\w+)\s+
            (?<UNK2>\w+)\s+:\s*
            (?<MESSAGE>.*)";

        protected static string logModelingPattern = @"^=+";
        protected bool m_isModeling;
        protected CSMEvent csmevent;

        protected LogReader(string filename, bool fromBeginning)
        {
            this.m_fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite|FileShare.Delete|FileShare.Inheritable);
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
                            if (String.IsNullOrEmpty(csmevent.eventInfo.Modeling))
                            {
                                csmevent.eventInfo.Modeling += result;
                            }
                            else
                            {
                                csmevent.eventInfo.Modeling += Environment.NewLine + result;
                            }
                            code = LogCode.LOG_MODELING;
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
                    Dictionary<string, object> jsonDict;
                    object outObject;
                    Match eventMatch;
                    try
                    {
                        eventMatch = Regex.Match(jsonString, LogMCDReader.msgPattern, RegexOptions.IgnorePatternWhitespace);
                        if (eventMatch.Success)
                        {
                            csmevent = new CSMEvent();
                            csmevent.eventInfo.TimeStamp = logResult.timestamp;
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
                            csmevent.node = GenerateTree(jsonDict).ToList();
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
                            logResult.result = csmevent;
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
                        csmevent.node = new List<TreeNode> { new TreeNode(String.Format("Exception: {0}", ex.Message)), new TreeNode(String.Format("JSON: {0}", jsonString)) };
                    }
                    break;
                default:
                    break;
            }
            return logResult;
        }

        private List<TreeNode> GenerateTree(Dictionary<string, object> jsonDict)
        {
            List<TreeNode> currentNodes = new List<TreeNode>();
            foreach (string dictKey in jsonDict.Keys)
            {
                TreeNode currentNode = new TreeNode();
                string strNode = "";

                object dictValue = jsonDict[dictKey];
                // Processing if value is dictionary
                if ((object)dictValue is Dictionary<string, object>)
                {
                    currentNode.Nodes.AddRange(GenerateTree((Dictionary<string, object>)dictValue).ToArray());
                    strNode = dictKey;
                }
                // Processing if value is list
                else if ((Object)dictValue is ArrayList)
                {
                    foreach (Dictionary<string, object> node in (ArrayList)dictValue)
                    {
                        currentNode.Nodes.AddRange(GenerateTree((Dictionary<string, object>)node).ToArray());
                    }
                    strNode = dictKey;
                }
                // Processing if value is atomic
                else
                {
                    strNode = String.Format("{0}: {1}", dictKey, jsonDict[dictKey]);
                }
                currentNode.Text = strNode;
                currentNodes.Add(currentNode);
            }
            return currentNodes;
        }
    }


    class LogCPReader : LogReader
    {
        private static string msgPattern = @"
            Rec:\s*
            (?<MONITOR>[^\s]*?)\s*\=\>
            (?<ID>[^,]*?),
            (?<TYPE>[^,]*?),
            (?<RESYNC_CODE>[^,]*?),
            (?<MON_REF>[^,]*?),
            (?<INFO>.*)";

        public LogCPReader(string filename, bool fromBeginning)
            : base(filename, fromBeginning) {}

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
                        csmevent.eventInfo.TimeStamp = logResult.timestamp;
                        csmevent.eventInfo.MonitorHandlerExtension = eventMatch.Groups["MONITOR"].Value;
                        csmevent.node = new List<TreeNode>();

                        List<string> groups = eventMatch.Groups["INFO"].Value.Split(',').ToList();
                        switch (eventMatch.Groups["TYPE"].Value)
                        {
                            case "CC":
                                csmevent.eventInfo.Type = "Call Cleared";
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "CGCID", groups[0])));
                                csmevent.eventInfo.CGCID = groups[0];
                                csmevent.eventInfo.Cause = groups[1];
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Transferred CID", groups[2])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Transfer destination", groups[3])));
                                break;
                            case "CO":
                                csmevent.eventInfo.Type = "Conferenced";
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Subject EXT", groups[0])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "PGCID", groups[1])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "SGCID", groups[2])));
                                csmevent.eventInfo.PGCID = groups[1];
                                csmevent.eventInfo.SGCID = groups[2];
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Conference controller EXT", groups[3])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Number of parties", groups[4])));
                                for(int i = 5, j = 1; i < groups.Count -1; i += 2, j++)
                                {
                                    csmevent.node.Add(new TreeNode(String.Format("Party #{0}: {1}", j, groups[i])));
                                    csmevent.node.Add(new TreeNode(String.Format("Party #{0}: {1}", j, groups[i+1])));
                                }
                                csmevent.eventInfo.Cause = groups[groups.Count-1];
                                break;
                            case "XC":
                                csmevent.eventInfo.Type = "Connection Cleared";
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "CGCID", groups[0])));
                                csmevent.eventInfo.CGCID = groups[0];
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Cleared EXT", groups[1])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Terminating EXT", groups[2])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Local CNX state", groups[3])));
                                csmevent.eventInfo.Cause = groups[4];
                                break;
                            case "DE":
                                csmevent.eventInfo.Type = "Delivered";
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "CGCID", groups[0])));
                                csmevent.eventInfo.CGCID = groups[0];
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Alerting internal EXT", groups[1])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Alerting outside number", groups[2])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Alerting device type", groups[3])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Internal calling EXT", groups[4])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Outside caller name", groups[5])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Outside caller number", groups[6])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Trunk name", groups[7])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Trunk outside number", groups[8])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Calling device type", groups[9])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Originally called device", groups[10])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Last redirection EXT", groups[11])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Account code", groups[12])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Local CNX state", groups[13])));
                                csmevent.eventInfo.Cause = groups[14];
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "ACD/UCD group", groups[15])));
                                break;
                            case "DI":
                                csmevent.eventInfo.Type = "Diverted";
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "CGCID", groups[0])));
                                csmevent.eventInfo.CGCID = groups[0];
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Diverted from EXT", groups[1])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "New destination EXT", groups[2])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Diverted to outside EXT", groups[3])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Local CNX state", groups[4])));
                                csmevent.eventInfo.Cause = groups[5];
                                break;
                            case "ER":
                                csmevent.eventInfo.Type = "Established Routing";
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "CGCID", groups[0])));
                                csmevent.eventInfo.CGCID = groups[0];
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Answering internal EXT", groups[1])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Answering outside number", groups[2])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Answering device type", groups[3])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Internal calling EXT", groups[4])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Outside caller number", groups[5])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Trunk outside number", groups[6])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Calling device type", groups[7])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Originally called device", groups[8])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Last redirection EXT", groups[9])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Local CNX state", groups[10])));
                                csmevent.eventInfo.Cause = groups[11];

                                break;
                            case "ES":
                                csmevent.eventInfo.Type = "Established";
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "CGCID", groups[0])));
                                csmevent.eventInfo.CGCID = groups[0];
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Answering internal EXT", groups[1])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Answering outside number", groups[2])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Answering device type", groups[3])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Internal calling EXT", groups[4])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Outside caller number", groups[5])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Trunk outside number", groups[6])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Calling device type", groups[7])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Originally called device", groups[8])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Last redirection EXT", groups[9])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Local CNX state", groups[10])));
                                csmevent.eventInfo.Cause = groups[11];
                                break;
                            case "FA":
                                csmevent.eventInfo.Type = "Failed";
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "CGCID", groups[0])));
                                csmevent.eventInfo.CGCID = groups[0];
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Failed EXT", groups[1])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Called number", groups[2])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Local CNX state", groups[3])));
                                csmevent.eventInfo.Cause = groups[4];
                                break;
                            case "HE":
                                csmevent.eventInfo.Type = "Held";
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "CGCID", groups[0])));
                                csmevent.eventInfo.CGCID = groups[0];
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Activate hold EXT", groups[1])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Local CNX state", groups[2])));
                                csmevent.eventInfo.Cause = groups[3];
                                break;
                            case "NT":
                                csmevent.eventInfo.Type = "Network Reached";
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "CGCID", groups[0])));
                                csmevent.eventInfo.CGCID = groups[0];
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Trunk used", groups[1])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Dialed number", groups[2])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Local CNX state", groups[3])));
                                csmevent.eventInfo.Cause = groups[4];
                                break;
                            case "OR":
                                csmevent.eventInfo.Type = "Originated";
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "CGCID", groups[0])));
                                csmevent.eventInfo.CGCID = groups[0];
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Internal calling EXT", groups[1])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Outside caller number", groups[2])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Calling device type", groups[3])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Dialed number", groups[4])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Account code", groups[5])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Local CNX state", groups[6])));
                                csmevent.eventInfo.Cause = groups[7];
                                break;
                            case "QU":
                                csmevent.eventInfo.Type = "Queued";
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "CGCID", groups[0])));
                                csmevent.eventInfo.CGCID = groups[0];
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Queued EXT", groups[1])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Internal calling EXT", groups[2])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Outside caller name", groups[3])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Outside caller number", groups[4])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Trunk name", groups[5])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Trunk outside number", groups[6])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Calling device type", groups[7])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Originally called device", groups[8])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Last redirection EXT", groups[9])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Number queued", groups[10])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Acoount code", groups[11])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Local CNX state", groups[12])));
                                csmevent.eventInfo.Cause = groups[13];
                                break;
                            case "RE":
                                csmevent.eventInfo.Type = "Retrieved";
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "CGCID", groups[0])));
                                csmevent.eventInfo.CGCID = groups[0];
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Retrieving EXT", groups[1])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Local CNX state", groups[2])));
                                csmevent.eventInfo.Cause = groups[3];
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Internal retrieved EXT", groups[4])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Retrieving device outside number", groups[5])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Retrieving device trunk number", groups[6])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Retrieving device type", groups[7])));
                                break;
                            case "SI":
                                csmevent.eventInfo.Type = "Service Initiated";
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "CGCID", groups[0])));
                                csmevent.eventInfo.CGCID = groups[0];
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Initiating EXT", groups[1])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Local CNX state", groups[2])));
                                csmevent.eventInfo.Cause = groups[3];
                                if (groups.Count > 4)
                                {
                                    csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Callback target EXT", groups[4])));
                                }
                                break;
                            case "TR":
                                csmevent.eventInfo.Type = "Transferred";
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Transferred CID", groups[0])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Transferring EXT", groups[1])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Transferred EXT", groups[3])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Announcement CID", groups[4])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Destination EXT", groups[5])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Outside caller name", groups[6])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Outside caller EXT", groups[7])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Trunk name", groups[8])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Trunk outside EXT", groups[9])));
                                csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Local CNX state", groups[10])));
                                csmevent.eventInfo.Cause = groups[11];
                                break;
                            default:
                                csmevent.eventInfo.Type = String.Format("{0} ({1})", csmevent.eventInfo.Type, eventMatch.Groups["TYPE"].Value);
                                int index = 1;
                                foreach (string item in groups)
                                {
                                    csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", index++, item)));
                                }
                                break;
                        }
                        csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "ID", eventMatch.Groups["ID"].Value)));
                        csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Type", eventMatch.Groups["TYPE"].Value)));
                        csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Cause", csmevent.eventInfo.Cause)));
                        csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Monitor", csmevent.Monitor)));
                        csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Resync Code", eventMatch.Groups["RESYNC_CODE"].Value)));
                        csmevent.node.Add(new TreeNode(String.Format("{0}: {1}", "Mon cross Ref ID", eventMatch.Groups["MON_REF"].Value)));
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
