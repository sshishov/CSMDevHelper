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
        }

        private void btnLogStart_Click(object sender, EventArgs e)
        {
            btnLogStart.Enabled = false;
            btnLogRestart.Enabled = true;
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

        private void btnLogRestart_Click(object sender, EventArgs e)
        {
            btnLogStop_Click(sender, e);
            btnLogStart_Click(sender, e);
        }

        private void btnLogStop_Click(object sender, EventArgs e)
        {
            this.isLogUpdate = false;
            this.logThread.Join();
            btnLogStart.Enabled = true;
            btnLogRestart.Enabled = false;
            btnLogStop.Enabled = false;
            btnLogPause.Enabled = false;
            btnLogPause.Text = "Pause";
            treeLog.Nodes.Clear();
            cblstEvents.Items.Clear();
        }

        private void btnLogPause_Click(object sender, EventArgs e)
        {
            if (btnLogPause.Text == "Pause")
            {
                btnLogPause.Text = "Resume";
                // Deprecated and should be changed to more convenience manner
                logThread.Suspend();
            }
            else
            {
                btnLogPause.Text = "Pause";
                // Deprecated and should be changed to more convenience manner
                logThread.Resume();
            }
        }

        void ThreadLogUpdate()
        {
            LogResult update;
            //LogReader logReader = new LogReader(@"C:\GA_logs.txt", false);
            //LogReader logReader = new LogReader(@"C:\ProgramData\Mitel\Customer Service Manager\Server\Logs\TelDrv.log", false);
            LogReader logReader = new LogReader(@"C:\ClearedTelDrv.txt", true);
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
                            // Updating Monitor checklist
                            this.rootNode.Text = treeLog.Nodes.Count + "> " + this.rootNode.Text;
                            treeLog.Nodes.Add(this.rootNode);
                        }
                        this.rootNode = new EventNode(logResult.result);
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
                            this.rootNode.Text = "*** " + this.rootNode.Text;
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
            if (cblstEvents.SelectedItem != null)
            {
                if (!cblstEvents.GetItemChecked(cblstEvents.SelectedIndex))
                {
                    cblstEvents.Items.Remove(cblstEvents.SelectedItem);
                    foreach (EventNode node in treeLog.Nodes.Find(cblstEvents.SelectedItem.ToString(), false))
                    {
                        treeLog.Nodes.Remove(node);
                    //    node.Hide();
                    }
                }
            }
        }
    }
}
