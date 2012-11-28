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
    delegate void LogUpdateDelegate(string str);

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
            isLogUpdate = false;
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
            string update = "";
            //LogReader logReader = new LogReader(@"C:\DEV_logs1.txt");
            LogReader logReader = new LogReader(@"C:\ProgramData\Mitel\Customer Service Manager\Server\Logs\TelDrv.log");
            while(isLogUpdate)
            {
                update = logReader.Process();
                if (update != null && update != "")
                {
                    Invoke(this.myDelegate, update);
                }
                else if (update == "")
                {

                }
            }
        }

        void LogUpdate(string jsonString)
        {
            if (isLogUpdate)
            {
                EventNode rootNode = new EventNode(jsonString);
                if (rootNode.eventInfo.eventType != null)
                {
                    if (!cblstEvents.Items.Contains(rootNode.eventInfo.eventType))
                    {
                        cblstEvents.Items.Add(rootNode.eventInfo.eventType, true);
                    }
                    treeLog.Nodes.Add(rootNode);
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
