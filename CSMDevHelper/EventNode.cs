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

    internal enum FilterComparator: int
    {
        EVENT_TYPE = 0,
        EVENT_MONITOR = 1,
        EVENT_GCID = 2,
    };

    internal class EventInfo
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

    public class CSMEvent
    {
        internal EventInfo eventInfo;
        public int filterCount;
        public List<TreeNode> node;

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

        internal bool Compare(FilterComparator attribute, string value)
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
                    result = this.CID.Contains(value);
                    break;
            }
            return result;
        }

        public HashSet<String> CID
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

        static Dictionary<string, Color> colorDict = new Dictionary<string, Color>()
        {
            {"Service Initiated", Color.Goldenrod},
            {"Network Reached", Color.Goldenrod},

            {"CallOriginatedEvent" , Color.DarkBlue},
            {"Originated" , Color.DarkBlue},

            {"CallDeliveredEvent" , Color.LimeGreen},
            {"Delivered" , Color.LimeGreen},
            {"CallReceivedEvent" , Color.LimeGreen},

            {"CallEstablishedEvent" , Color.Green},
            {"Established" , Color.Green},
            
            {"CallDivertedEvent" , Color.DarkViolet},
            {"Diverted" , Color.DarkViolet},

            {"CallClearedEvent" , Color.Red},
            {"Call Cleared" , Color.Red},
            {"Connection Cleared" , Color.Red},
        };

        public CSMEvent()
        {
            this.eventInfo = new EventInfo();
            this.filterCount = 0;
        }

        public Color GetColor()
        {
            Color defaultColor;
            CSMEvent.colorDict.TryGetValue(this.eventInfo.Type, out defaultColor);
            return defaultColor;
        }
    }
}
