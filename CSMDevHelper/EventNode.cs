using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Web.Script.Serialization;
using System.Collections;
using System.Text.RegularExpressions;

namespace CSMDevHelper
{

    enum FilterComparator: int
    {
        EVENT_TYPE = 0,
        EVENT_MONITOR = 1,
        EVENT_GCID = 2,
    };

    class EventInfo
    {
        public string Type;
        public string TypeNumber;
        public string Cause;
        public string CauseNumber;
        public string MonitorHandler;
        public string MonitorHandlerNode;
        public string MonitorHandlerExtension;
        public string TimeStamp;
        public string Modeling;
        public string CGCID;
        public string PGCID;
        public string SGCID;
        public bool isParked;

        public EventInfo()
        {
            Type = "UnknownType";
            TypeNumber = "N/A";
            Cause = "UnknownCause";
            CauseNumber = "N/A";
            MonitorHandler = default(string);
            MonitorHandlerNode = default(string);
            MonitorHandlerExtension = "UnknownExtension";
            TimeStamp = default(string);
            Modeling = default(string);
            CGCID = default(string);
            PGCID = default(string);
            SGCID = default(string);
            isParked = default(bool);
        }
    }

    class EventNode : TreeNode
    {
        public EventInfo eventInfo;
        public int filterCount;

        public bool hasModeling
        {
            get{ return this.eventInfo.Modeling != default(String); }
        }

        public string Monitor
        {
            get
            {
                if (this.eventInfo.MonitorHandlerNode == default(string))
                    return this.eventInfo.MonitorHandlerExtension;
                else
                    return String.Format("{0} (node {1})", this.eventInfo.MonitorHandlerExtension, this.eventInfo.MonitorHandlerNode);
            }
        }
        
        public bool Parked
        {
            get { return this.eventInfo.isParked; }
        }

        public bool Compare(FilterComparator attribute, string value)
        {
            bool result = default(bool);
            switch (attribute)
            {
                case FilterComparator.EVENT_TYPE:
                    result = (this.eventInfo.Type == value);
                    break;
                case FilterComparator.EVENT_MONITOR:
                    result = (this.Monitor == value);
                    break;
                case FilterComparator.EVENT_GCID:
                    result = this.GCID.Contains(value);
                    break;
            }
            return result;
        }

        public HashSet<String> GCID
        {
            get
            {
                HashSet<String> result = new HashSet<String>();
                if (this.eventInfo.CGCID != default(string))
                    result.Add(this.eventInfo.CGCID);
                if (this.eventInfo.PGCID != default(string))
                    result.Add(this.eventInfo.PGCID);
                if (this.eventInfo.SGCID != default(string))
                    result.Add(this.eventInfo.SGCID);
                if (result.Count == 0)
                    result.Add("N/A");
                return result;
            }
        }

        private Dictionary<string, object> jsonDict;

        static Dictionary<string, Color> colorDict = new Dictionary<string, Color>()
        {
            {"CallOriginatedEvent" , Color.Blue},
            {"CallDeliveredEvent" , Color.Red},
            {"CallReceivedEvent" , Color.Green},
            {"CallClearedEvent" , Color.Orange}
        };

        public EventNode(string jsonString, string timestamp)
        {
            this.eventInfo = new EventInfo();
            this.filterCount = 0;
            object outObject;
            Match regMatch;

            this.eventInfo.TimeStamp = timestamp;
            try
            {
                // Workaround for parked events
                if (jsonString.Contains("{*** Processing Parked Event ***}"))
                {
                    jsonString = jsonString.Replace("{*** Processing Parked Event ***}", "");
                    this.eventInfo.isParked = true;
                }
                // Workaround for old versions (DropEvent w/o comma)
                jsonString = jsonString.Replace(@"false""", @"false,""");
                // Workaround for hex integers
                jsonString = Regex.Replace(jsonString, @"(?<=\s)(\w+)(?=,)", "\"$1\"");
                //Console.WriteLine("JSON = ***" + jsonString + "***");
                JavaScriptSerializer mySer = new JavaScriptSerializer();
                this.jsonDict = mySer.Deserialize<Dictionary<string, object>>(jsonString);
                this.jsonDict = (Dictionary<string, object>)jsonDict["MitaiEvent"];
                this.Nodes.AddRange(GenerateTree(this.jsonDict));
                if (this.jsonDict.TryGetValue("Type", out outObject))
                {
                    regMatch = Regex.Match((string)this.jsonDict["Type"], @"^(?<TYPE>\D+)\((?<TYPENUM>\d+)\)");
                    if (regMatch.Success)
                    {
                        this.eventInfo.Type = regMatch.Groups["TYPE"].Value;
                        this.eventInfo.TypeNumber = regMatch.Groups["TYPENUM"].Value;
                    }
                }
                if (this.jsonDict.TryGetValue("Cause", out outObject))
                {
                    regMatch = Regex.Match((string)outObject, @"^(?<CAUSE>\D+)\((?<CAUSENUM>\d+)\)");
                    if (regMatch.Success)
                    {
                        this.eventInfo.Cause = regMatch.Groups["CAUSE"].Value;
                        this.eventInfo.CauseNumber = regMatch.Groups["CAUSENUM"].Value;
                    }
                }
            }
            catch (ArgumentException ex)
            {
                this.eventInfo.Type = "Exception";
                this.eventInfo.Cause = "Invalid JSON";
                this.Nodes.Add(String.Format("Exception: {0}", ex.Message));
                this.Nodes.Add(String.Format("JSON: {0}", jsonString));
                return;
            }
            finally
            {

                this.Name = this.eventInfo.Type;
                this.Text = String.Format("{0,-25}: {1,-20} ({2,15})",
                    this.eventInfo.Type, this.eventInfo.Cause,
                    this.eventInfo.TimeStamp);
                Color defaultColor;
                EventNode.colorDict.TryGetValue(this.eventInfo.Type, out defaultColor);
                this.ForeColor = defaultColor;
            }
            if (this.jsonDict.TryGetValue("MonitorHandle", out outObject))
            {
                regMatch = Regex.Match((string)outObject, @"^(?<MONITOR>.+)\((?<MONITORNUM>.*)\)");
                if (regMatch.Success)
                {
                    this.eventInfo.MonitorHandler = regMatch.Groups["MONITOR"].Value;
                    this.eventInfo.MonitorHandlerExtension = regMatch.Groups["MONITORNUM"].Value;
                    regMatch = Regex.Match(regMatch.Groups["MONITORNUM"].Value, @"(?<MONITORNODE>.*)\|(?<MONITORNUM>.*)");
                    if (regMatch.Success)
                    {
                        this.eventInfo.MonitorHandlerNode = regMatch.Groups["MONITORNODE"].Value;
                        this.eventInfo.MonitorHandlerExtension = regMatch.Groups["MONITORNUM"].Value;
                    }
                }
            }
            if (this.jsonDict.TryGetValue("CGCID", out outObject))
            {
                this.eventInfo.CGCID = (string)outObject;
            }
            if (this.jsonDict.TryGetValue("PGCID", out outObject))
            {
                this.eventInfo.PGCID = (string)outObject;
            }
            if (this.jsonDict.TryGetValue("SGCID", out outObject))
            {
                this.eventInfo.SGCID = (string)outObject;
            }
            if (this.Parked)
            {
                this.Text = String.Format("{0} <== Parked", this.Text);
            }
        }

        private TreeNode[] GenerateTree(Dictionary<string, object> dict)
        {
            int counter = 0;
            TreeNode[] currentNodes = new TreeNode[dict.Count];
            foreach (string dictKey in dict.Keys)
            {
                TreeNode currentNode = new TreeNode();
                string strNode = "";

                object dictValue = dict[dictKey];
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
                    strNode = String.Format("{0}: {1}", dictKey, dict[dictKey]);
                }
                currentNode.Text = strNode;
                currentNodes[counter++] = currentNode;
            }
            return currentNodes;
        }
    }
}
