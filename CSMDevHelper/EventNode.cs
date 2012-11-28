﻿using System;
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
        public string EventCauseNumber;
        public string MonitorHandler;
        public string MonitorHandlerExtension;
        public bool ParkedEvent;
    }

    class EventNode : TreeNode
    {
        public EventInfo eventInfo;
        private Dictionary<string, object> jsonDict;

        static Dictionary<string, Color> colorDict = new Dictionary<string, Color>()
        {
            {"CallOriginatedEvent" , Color.Blue},
            {"CallDeliveredEvent" , Color.Red},
            {"CallReceivedEvent" , Color.Green},
            {"CallClearedEvent" , Color.Orange}
        };

        public EventNode(string jsonString)
        {
            eventInfo = new EventInfo();
            Match regMatch;
            try
            {
                // Workaroud for parked events
                if (jsonString.Contains("{*** Processing Parked Event ***}"))
                {
                    jsonString = jsonString.Replace("{*** Processing Parked Event ***}", "");
                    this.eventInfo.ParkedEvent = true;
                }
                JavaScriptSerializer mySer = new JavaScriptSerializer();
                this.jsonDict = mySer.Deserialize<Dictionary<string, object>>(jsonString);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("EXCEPTION: " + ex.Message);
                return;
            }


            object outObject;
            if (this.jsonDict.TryGetValue("MitaiEvent", out outObject))
            {
                this.jsonDict = (Dictionary<string, object>)outObject;
            }
            else
            {
                return;
            }
            regMatch = Regex.Match((string)this.jsonDict["Type"], @"^(?<TYPE>\D+)\((?<TYPENUM>\d+)\)");
            if (regMatch.Success)
            {
                this.eventInfo.eventType = regMatch.Groups["TYPE"].Value;
                this.eventInfo.eventTypeNumber = regMatch.Groups["TYPENUM"].Value;
            }
            regMatch = Regex.Match((string)this.jsonDict["Cause"], @"^(?<CAUSE>\D+)\((?<CAUSENUM>\d+)\)");
            if (regMatch.Success)
            {
                this.eventInfo.eventCause = regMatch.Groups["CAUSE"].Value;
                this.eventInfo.EventCauseNumber = regMatch.Groups["CAUSENUM"].Value;
            }

            this.Name = this.eventInfo.eventType;

            Color defaultColor;
            EventNode.colorDict.TryGetValue(this.eventInfo.eventType, out defaultColor);
            this.ForeColor = defaultColor;

            this.Show();
        }

        public void Hide()
        {
            this.Nodes.Clear();
            this.Text = "";
        }
        public void Show()
        {
            this.Nodes.AddRange(GenerateTree(this.jsonDict));
            this.Text = this.eventInfo.eventType + ": " + this.eventInfo.eventCause;
            if (this.eventInfo.ParkedEvent)
            {
                this.Text = "PARKED: " + this.Text;
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