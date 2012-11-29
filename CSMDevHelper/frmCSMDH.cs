using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace CSMDevHelper
{
    delegate void LogUpdateDelegate(LogResult logResult);

    public partial class frmCSMDH : Form
    {
        public frmCSMDH()
        {
            InitializeComponent();
            isLogUpdate = false;
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
            LogReader logReader = new LogReader(@"C:\GA_logs.txt", true);
            //LogReader logReader = new LogReader(@"C:\ProgramData\Mitel\Customer Service Manager\Server\Logs\TelDrv.log", true);
            while(this.isLogUpdate)
            {
                update = logReader.Process();
                Invoke(this.myDelegate, update);
            }
            MessageBox.Show("EAH!");
        }

        void LogUpdate(LogResult logResult)
        {
            if (this.isLogUpdate)
            {
                switch (logResult.code)
                {
                    case LogCode.LOG_MITAI:
                        EventNode rootNode = new EventNode(logResult.result);
                        if (!cblstEvents.Items.Contains(rootNode.eventInfo.eventType))
                        {
                            cblstEvents.Items.Add(rootNode.eventInfo.eventType, true);
                        }
                        treeLog.Nodes.Add(rootNode);
                        break;
                    case LogCode.LOG_MODELING:
                        EventNode lastNode = (EventNode)treeLog.Nodes[treeLog.GetNodeCount(false) - 1];
                        if (lastNode.ToolTipText == String.Empty)
                        {
                            lastNode.ToolTipText += logResult.result;
                        }
                        else
                        {
                            lastNode.ToolTipText += "\n" + logResult.result;
                        }
                        lastNode.NodeFont = new Font(treeLog.Font, FontStyle.Bold);
                        lastNode.Text += String.Empty;
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
            string selectedEvent = cblstEvents.SelectedItem.ToString();
            if (!cblstEvents.GetItemChecked(cblstEvents.SelectedIndex))
            {
                foreach (EventNode node in treeLog.Nodes.Find(selectedEvent, false))
                {
                    node.Hide();
                }
            }
            else
            {
                foreach (EventNode node in treeLog.Nodes.Find(selectedEvent, false))
                {
                    node.Show();
                }
            }
        }
    }
}
