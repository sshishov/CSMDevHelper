using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Web.Script.Serialization;

namespace CSMDevHelper
{
    delegate void LogUpdateDelegate(LogResult logResult);

    public partial class frmCSMDH : Form
    {
        public frmCSMDH()
        {
            InitializeComponent();
            this.isLogUpdate = false;
            this.rootNode = null;
            this.dictGCID = new Dictionary<string, HashSet<string>>();
        }

        private void btnLogStart_Click(object sender, EventArgs e)
        {
            treeLog.Nodes.Clear();
            cblstEvents.Items.Clear();
            cblstMonitors.Items.Clear();

            treeLog.Enabled = false;
            tcLogFilters.Enabled = false;

            btnLogStart.Enabled = false;
            btnLogStop.Enabled = true;
            btnLogPause.Enabled = true;
            isLogUpdate = true;

            ThreadStart sthread = new ThreadStart(ThreadLogUpdate);
            logThread = new Thread(sthread);
            myDelegate = new LogUpdateDelegate(this.LogUpdate);
            logThread.Name = "LogReaderThread";
            logThread.Priority = ThreadPriority.Lowest;
            logThread.IsBackground = true;
            logThread.Start();
            while (!logThread.IsAlive) ;
        }

        private void btnLogStop_Click(object sender, EventArgs e)
        {
            treeLog.Enabled = true;
            tcLogFilters.Enabled = true;

            this.isLogUpdate = false;
            //this.logThread.Join();
            btnLogStart.Enabled = true;
            btnLogStop.Enabled = false;
            btnLogPause.Enabled = false;
            btnLogPause.Text = "Pause";
        }

        private void btnLogPause_Click(object sender, EventArgs e)
        {
            if (btnLogPause.Text == "Pause")
            {
                btnLogPause.Text = "Resume";
                // Deprecated and should be changed to more convenience manner
                logThread.Suspend();
                treeLog.Enabled = true;
                tcLogFilters.Enabled = true;
            }
            else
            {
                btnLogPause.Text = "Pause";
                // Deprecated and should be changed to more convenience manner
                logThread.Resume();
                treeLog.Enabled = false;
                tcLogFilters.Enabled = false;
            }
        }

        void ThreadLogUpdate()
        {
            LogResult update;
            //LogReader logReader = new LogReader(@"C:\GA_logs.txt", false);
            LogReader logReader = new LogReader(@"C:\ProgramData\Mitel\Customer Service Manager\Server\Logs\TelDrv.log", false);
            //LogReader logReader = new LogReader(@"C:\ClearedTelDrv.txt", true);
            while(this.isLogUpdate)
            {
                update = logReader.Process();
                Invoke(this.myDelegate, update);
            }
        }

        void LogUpdate(LogResult logResult)
        {
            if (this.isLogUpdate)
            {
                tsslEvents.Text = "Event count = " + treeLog.Nodes.Count;
                switch (logResult.code)
                {
                    case LogCode.LOG_MITAI:
                        if (this.rootNode != null && this.rootNode.Name != "MonitorSetEvent")
                        {
                            // Updating Event checklist
                            if (!cblstEvents.Items.Contains(this.rootNode.eventInfo.eventType))
                            {
                                cblstEvents.Items.Add(this.rootNode.eventInfo.eventType, true);
                            }
                            // Updating Monitor checklist
                            if (!cblstMonitors.Items.Contains(this.rootNode.Monitor))
                            {
                                if (this.rootNode.eventInfo.eventMonitorHandlerExtension == "UnknownExtension")
                                {
                                    cblstMonitors.Items.Add(this.rootNode.Monitor, true);
                                }
                                else
                                {
                                    cblstMonitors.Items.Add(this.rootNode.Monitor, true);
                                }
                            }
                            //Updating GCID checklist
                            foreach (string gcid in this.rootNode.eventSetGCID)
                            {
                                if (this.dictGCID.ContainsKey(gcid))
                                {
                                    this.dictGCID[gcid].Union(this.rootNode.eventSetGCID);
                                }
                                else
                                {
                                    this.dictGCID[gcid] = this.rootNode.eventSetGCID;
                                }
                                if (!cblstGCID.Items.Contains(gcid))
                                {
                                    cblstGCID.Items.Add(gcid);
                                }
                            }
                            this.rootNode.Show();
                            this.rootNode.Text = String.Format("{0,4}> {1}", treeLog.Nodes.Count, this.rootNode.Text);
                            treeLog.Nodes.Add(this.rootNode);
                        }
                        this.rootNode = new EventNode(logResult.result, logResult.timestamp);
                        break;
                    case LogCode.LOG_MODELING:
                        this.rootNode.hasModeling = true;
                        if (this.rootNode.ToolTipText == String.Empty)
                        {
                            this.rootNode.eventInfo.eventModeling += logResult.result;
                            this.rootNode.ToolTipText += logResult.result;
                        }
                        else
                        {
                            this.rootNode.eventInfo.eventModeling += Environment.NewLine + logResult.result;
                            this.rootNode.ToolTipText += Environment.NewLine + logResult.result;
                        }
                        if (!this.rootNode.Text.StartsWith("***"))
                        {
                            this.rootNode.Text = String.Format("{0,3} {1}","***",this.rootNode.Text);
                        }
                        break;
                    case LogCode.LOG_LEG:
                        break;
                    case LogCode.LOG_NOTHING:
                    default:
                        break;
                }
            }
        }

        private void cblstEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cblstEvents.SelectedItem != null && cblstEvents.SelectedItem.ToString() != String.Empty)
            {
                if (!cblstEvents.GetItemChecked(cblstEvents.SelectedIndex))
                {
                    foreach (EventNode node in treeLog.Nodes.Find(cblstEvents.SelectedItem.ToString(), false))
                    {
                        node.Remove();
                    }
                    cblstEvents.Items.Remove(cblstEvents.SelectedItem);
                }
            }
        }

        private void cblstGCID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cblstGCID.SelectedItem != null && cblstGCID.SelectedItem.ToString() != String.Empty)
            {
                int index = 0;
                while (index < treeLog.Nodes.Count)
                {
                    EventNode node = (EventNode)treeLog.Nodes[index];
                    if (!node.eventSetGCID.Overlaps(this.dictGCID[cblstGCID.SelectedItem.ToString()]))
                    {
                        node.Remove();
                    }
                    else
                    {
                        index++;
                    }
                }
            }
        }
    }
}
