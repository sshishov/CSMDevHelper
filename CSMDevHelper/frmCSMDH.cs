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
            this.listGCID = new BindingList<string>();
            this.listMonitor = new BindingList<string>();
            this.listEvent = new BindingList<string>();
            this.listNode = new BindingList<EventNode>();
            this.lbGCID.DataSource = this.listGCID;
            this.lbMonitor.DataSource = this.listMonitor;
            this.lbEvent.DataSource = this.listEvent;
        }

        private void btnLogStart_Click(object sender, EventArgs e)
        {
            treeLog.Nodes.Clear();
            //cblstEvents.Items.Clear();
            //cblstMonitors.Items.Clear();

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
                tsslEvents.Text = "Event count = " + treeLog.Nodes.Count;
                switch (logResult.code)
                {
                    case LogCode.LOG_MITAI:
                        this.rootNode = new EventNode(logResult.result, logResult.timestamp);
                        // Updating Event checklist
                        if (!lbEvent.Items.Contains(this.rootNode.eventInfo.eventType))
                        {
                            this.listEvent.Add(this.rootNode.eventInfo.eventType);
                        }
                        // Updating Monitor checklist
                        if (!listMonitor.Contains(this.rootNode.Monitor))
                        {
                            listMonitor.Add(this.rootNode.Monitor);
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
                            if (!listGCID.Contains(gcid))
                            {
                                listGCID.Add(gcid);
                            }
                        }
                        this.rootNode.Text = String.Format("{0,4}> {1}", treeLog.Nodes.Count + 1, this.rootNode.Text);
                        treeLog.Nodes.Add(this.rootNode);
                        break;
                    case LogCode.LOG_MODELING:
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
                        this.rootNode.hasModeling = true;
                        //this.rootNode.BackColor = Color.LightGoldenrodYellow;
                        break;
                    case LogCode.LOG_LEG:
                        break;
                    case LogCode.LOG_NOTHING:
                    default:
                        break;
                }
            }
        }

        private void tbMonitor_TextChanged(object sender, EventArgs e)
        {
            IEnumerable<string> filtered = this.listMonitor.Where((i) => i.IndexOf(tbMonitor.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            if (filtered.Count<string>() != 0)
            {
                lbMonitor.DataSource = new BindingSource(filtered, "");
            }
            else
            {
                lbMonitor.DataSource = null;
            }
        }

        private void tbGCID_TextChanged(object sender, EventArgs e)
        {
            IEnumerable<string> filtered = this.listGCID.Where((i) => i.IndexOf(tbGCID.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            if (filtered.Count<string>() != 0)
            {
                lbGCID.DataSource = new BindingSource(filtered, "");
            }
            else
            {
                lbGCID.DataSource = null;
            }
        }

        private void tbEvent_TextChanged(object sender, EventArgs e)
        {
            IEnumerable<string> filtered = this.listEvent.Where((i) => i.IndexOf(tbEvent.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            if (filtered.Count<string>() != 0)
            {
                lbEvent.DataSource = new BindingSource(filtered, "");
            }
            else
            {
                lbEvent.DataSource = null;
            }
        }

        private void btnAddEvent_Click(object sender, EventArgs e)
        {
            foreach (string item in lbEvent.SelectedItems) { if (!lbFilterEvent.Items.Contains(item)) { lbFilterEvent.Items.Add(item); } }
        }

        private void btnAddAllEvent_Click(object sender, EventArgs e)
        {
            foreach (string item in lbEvent.Items) { if (!lbFilterEvent.Items.Contains(item)) { lbFilterEvent.Items.Add(item); } }
        }

        private void btnDelEvent_Click(object sender, EventArgs e)
        {
            while (lbFilterEvent.SelectedItems.Count > 0) { lbFilterEvent.Items.Remove(lbFilterEvent.SelectedItem); }
        }

        private void btnDelAllEvent_Click(object sender, EventArgs e)
        {
            lbFilterEvent.Items.Clear();
        }

        private void btnAddMonitor_Click(object sender, EventArgs e)
        {
            foreach (string item in lbMonitor.SelectedItems) { if (!lbFilterMonitor.Items.Contains(item)) { lbFilterMonitor.Items.Add(item); } }
        }

        private void btnAddAllMonitor_Click(object sender, EventArgs e)
        {
            foreach (string item in lbMonitor.Items) { if (!lbFilterMonitor.Items.Contains(item)) { lbFilterMonitor.Items.Add(item); } }
        }

        private void btnDelMonitor_Click(object sender, EventArgs e)
        {
            while (lbFilterMonitor.SelectedItems.Count > 0) { lbFilterMonitor.Items.Remove(lbFilterMonitor.SelectedItem); }
        }

        private void btnDelAllMonitor_Click(object sender, EventArgs e)
        {
            lbFilterMonitor.Items.Clear();
        }

        private void btnAddGCID_Click(object sender, EventArgs e)
        {
            foreach (string item in lbGCID.SelectedItems) { if (!lbFilterGCID.Items.Contains(item)) { lbFilterGCID.Items.Add(item); } }
        }

        private void btnAddAllGCID_Click(object sender, EventArgs e)
        {
            foreach (string item in lbGCID.Items) { if (!lbFilterGCID.Items.Contains(item)) { lbFilterGCID.Items.Add(item); } }
        }

        private void btnDelGCID_Click(object sender, EventArgs e)
        {
            while (lbFilterGCID.SelectedItems.Count > 0) { lbFilterGCID.Items.Remove(lbFilterGCID.SelectedItem); }
        }

        private void btnDelAllGCID_Click(object sender, EventArgs e)
        {
            lbFilterGCID.Items.Clear();
        }
    }
}
