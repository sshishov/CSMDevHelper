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
    class EventInfo
    {
        public string eventType;
        public string eventTypeNumber;
        public string eventCause;
        public string eventCauseNumber;
        public string eventMonitorHandler;
        public string eventMonitorHandlerNode;
        public string eventMonitorHandlerExtension;
        public string eventTimeStamp;
        public string eventModeling;

        public EventInfo()
        {
            eventType = String.Empty;
            eventTypeNumber = String.Empty;
            eventCause = String.Empty;
            eventCauseNumber = String.Empty;
            eventMonitorHandler = String.Empty;
            eventMonitorHandlerNode = String.Empty;
            eventMonitorHandlerExtension = String.Empty;
            eventTimeStamp = String.Empty;
            eventModeling = String.Empty;
        }
    }

    class EventNode : TreeNode
    {
        public EventInfo eventInfo;
        public bool hasModeling;
        public bool isParked;
        public HashSet<string> eventSetGCID;

        public string Monitor
        {
            get
            {
                if (this.eventInfo.eventMonitorHandlerNode == String.Empty)
                    return this.eventInfo.eventMonitorHandlerExtension;
                else
                    return this.eventInfo.eventMonitorHandlerExtension + " (node" + this.eventInfo.eventMonitorHandlerNode + ")";
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
            this.isParked = false;
            this.hasModeling = false;
            this.eventInfo = new EventInfo();
            this.eventSetGCID = new HashSet<string>();
            Match regMatch;

            this.eventInfo.eventTimeStamp = timestamp;
            try
            {
                // Workaround for parked events
                if (jsonString.Contains("{*** Processing Parked Event ***}"))
                {
                    jsonString = jsonString.Replace("{*** Processing Parked Event ***}", "");
                    this.isParked = true;
                }
                // Workaround for old versions (DropEvent w/o comma)
                jsonString = jsonString.Replace(@"false""", @"false,""");
                //Workaround for hex integers
                //jsonString = new Regex(@"(?=\s\w+,)").Replace(jsonString, "$2");
                //Console.WriteLine("JSON = ***" + jsonString + "***");
                JavaScriptSerializer mySer = new JavaScriptSerializer();
                this.jsonDict = mySer.Deserialize<Dictionary<string, object>>(jsonString);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("EXCEPTION: " + ex.Message);
                return;
            }


            this.jsonDict = (Dictionary<string, object>)jsonDict["MitaiEvent"];
            object outObject;
            if (this.jsonDict.TryGetValue("Type", out outObject))
            {
                regMatch = Regex.Match((string)this.jsonDict["Type"], @"^(?<TYPE>\D+)\((?<TYPENUM>\d+)\)");
                if (regMatch.Success)
                {
                    this.eventInfo.eventType = regMatch.Groups["TYPE"].Value;
                    this.eventInfo.eventTypeNumber = regMatch.Groups["TYPENUM"].Value;
                }
                else
                {
                    this.eventInfo.eventType = "UnknownType";
                    this.eventInfo.eventTypeNumber = "N/A";
                }
            }
            else
            {
                this.eventInfo.eventType = "UnknownType";
                this.eventInfo.eventTypeNumber = "N/A";
            }
            if (this.jsonDict.TryGetValue("Cause", out outObject))
            {
                regMatch = Regex.Match((string)outObject, @"^(?<CAUSE>\D+)\((?<CAUSENUM>\d+)\)");
                if (regMatch.Success)
                {
                    this.eventInfo.eventCause = regMatch.Groups["CAUSE"].Value;
                    this.eventInfo.eventCauseNumber = regMatch.Groups["CAUSENUM"].Value;
                }
                else
                {
                    this.eventInfo.eventCause = "UnknownCause";
                    this.eventInfo.eventCauseNumber = "N/A";
                }
            }
            else
            {
                this.eventInfo.eventCause = "UnknownCause";
                this.eventInfo.eventCauseNumber = "N/A";
            }

            if (this.jsonDict.TryGetValue("MonitorHandle", out outObject))
            {
                regMatch = Regex.Match((string)outObject, @"^(?<MONITOR>.+)\((?<MONITORNUM>.*)\)");
                if (regMatch.Success)
                {
                    this.eventInfo.eventMonitorHandler = regMatch.Groups["MONITOR"].Value;
                    this.eventInfo.eventMonitorHandlerExtension = regMatch.Groups["MONITORNUM"].Value;
                    regMatch = Regex.Match(regMatch.Groups["MONITORNUM"].Value, @"(?<MONITORNODE>.*)\|(?<MONITORNUM>.*)");
                    if (regMatch.Success)
                    {
                        this.eventInfo.eventMonitorHandlerNode = regMatch.Groups["MONITORNODE"].Value;
                        this.eventInfo.eventMonitorHandlerExtension = regMatch.Groups["MONITORNUM"].Value;
                    }
                }
                else
                {
                    this.eventInfo.eventMonitorHandlerExtension = "UnknownExtension";
                }
            }
            else
            {
                this.eventInfo.eventMonitorHandlerExtension = "UnknownExtension";
            }

            if (this.jsonDict.TryGetValue("CGCID", out outObject))
            {
                this.eventSetGCID.Add((string)outObject);
            }
            if (this.jsonDict.TryGetValue("PGCID", out outObject))
            {
                this.eventSetGCID.Add((string)outObject);
            }
            if (this.jsonDict.TryGetValue("SGCID", out outObject))
            {
                this.eventSetGCID.Add((string)outObject);
            }

            this.Name = this.eventInfo.eventType;

            Color defaultColor;
            EventNode.colorDict.TryGetValue(this.eventInfo.eventType, out defaultColor);
            this.ForeColor = defaultColor;
        }

        public void Hide()
        {
            this.Nodes.Clear();
            this.Text = String.Empty;
        }
        public void Show()
        {
            this.Nodes.AddRange(GenerateTree(this.jsonDict));
            this.Text = String.Format("{0,-25}: {1,-25} ({2,15})",
                this.eventInfo.eventType, this.eventInfo.eventCause,
                this.eventInfo.eventTimeStamp);
            if (this.isParked)
            {
                this.BackColor = Color.LightCoral;
                this.Text = String.Format("{0} <== Parked", this.Text);
            }
            if (this.hasModeling)
            {
                this.BackColor = Color.LightGoldenrodYellow;
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
