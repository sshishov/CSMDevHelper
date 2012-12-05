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
            this.treeLog.Sorted = true;
            this.dictGCID = new Dictionary<string, HashSet<string>>();

            this.listGCID = new CustomBindingList<string>();
            this.lbGCID.DataSource = this.listGCID;

            this.listMonitor = new CustomBindingList<string>();
            this.lbMonitor.DataSource = this.listMonitor;

            this.listEvent = new CustomBindingList<string>();
            this.lbEvent.DataSource = this.listEvent;

            this.listFilterGCID = new CustomBindingList<string>();
            this.lbFilterGCID.DataSource = this.listFilterGCID;
            this.listFilterGCID.ListChanged += new ListChangedEventHandler(listFilterGCID_ListChanged);

            this.listFilterMonitor = new CustomBindingList<string>();
            this.lbFilterMonitor.DataSource = this.listFilterMonitor;
            this.listFilterMonitor.ListChanged += new ListChangedEventHandler(listFilterMonitor_ListChanged);

            this.listFilterEvent = new CustomBindingList<string>();
            this.lbFilterEvent.DataSource = this.listFilterEvent;
            this.listFilterEvent.ListChanged += new ListChangedEventHandler(listFilterEvent_ListChanged);

            this.listNode = new CustomBindingList<EventNode>();
            this.listNode.ListChanged += new ListChangedEventHandler(listNode_ListChanged);
            this.listFilterNode = new CustomBindingList<EventNode>();
            this.listFilterNode.ListChanged += new ListChangedEventHandler(listFilterNode_ListChanged);
            
        }

        private void btnLogStart_Click(object sender, EventArgs e)
        {
            listNode.Clear();
            listFilterNode.Clear();
            listMonitor.Clear();
            listFilterMonitor.Clear();
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
                        listNode.Add(rootNode);
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
            int[] arr = new int[lbEvent.SelectedItems.Count];
            lbEvent.SelectedIndices.CopyTo(arr, 0);
            for (int index = lbEvent.SelectedItems.Count - 1; index >= 0; index--)
            {
                string item = (string)lbEvent.Items[arr[index]];
                listFilterEvent.Add(item);
                listEvent.Remove(item);
            }
            this.tbEvent_TextChanged(sender, e);
            lbEvent.ClearSelected();
        }

        private void btnAddAllEvent_Click(object sender, EventArgs e)
        {
            foreach (string item in listEvent) { listFilterEvent.Add(item); }
            listEvent.Clear();
            this.tbEvent_TextChanged(sender, e);
            lbEvent.ClearSelected();
        }

        private void btnDelEvent_Click(object sender, EventArgs e)
        {
            int[] arr = new int[lbFilterEvent.SelectedItems.Count];
            lbFilterEvent.SelectedIndices.CopyTo(arr, 0);
            for (int index = lbFilterEvent.SelectedItems.Count - 1; index >= 0; index--)
            {
                listEvent.Add(listFilterEvent[arr[index]]);
                listFilterEvent.RemoveAt(arr[index]);
            }
            this.tbEvent_TextChanged(sender, e);
            lbFilterEvent.ClearSelected();
        }

        private void btnDelAllEvent_Click(object sender, EventArgs e)
        {
            foreach (string item in listFilterEvent) { listEvent.Add(item); }
            listFilterEvent.Clear();
            this.tbEvent_TextChanged(sender, e);
            lbFilterEvent.ClearSelected();
        }

        private void btnAddMonitor_Click(object sender, EventArgs e)
        {
            int[] arr = new int[lbMonitor.SelectedItems.Count];
            lbMonitor.SelectedIndices.CopyTo(arr, 0);
            for (int index = lbMonitor.SelectedItems.Count - 1; index >= 0; index--)
            {
                string item = (string)lbMonitor.Items[arr[index]];
                listFilterMonitor.Add(item);
                listMonitor.Remove(item);
            }
            this.tbMonitor_TextChanged(sender, e);
            lbMonitor.ClearSelected();
        }

        private void btnAddAllMonitor_Click(object sender, EventArgs e)
        {
            foreach (string item in listMonitor) { listFilterMonitor.Add(item); }
            listMonitor.Clear();
            this.tbMonitor_TextChanged(sender, e);
            lbMonitor.ClearSelected();
        }

        private void btnDelMonitor_Click(object sender, EventArgs e)
        {
            int[] arr = new int[lbFilterMonitor.SelectedItems.Count];
            lbFilterMonitor.SelectedIndices.CopyTo(arr, 0);
            for (int index = lbFilterMonitor.SelectedItems.Count - 1; index >= 0; index--)
            {
                listMonitor.Add(listFilterMonitor[arr[index]]);
                listFilterMonitor.RemoveAt(arr[index]);
            }
            this.tbMonitor_TextChanged(sender, e);
            lbFilterMonitor.ClearSelected();
        }

        private void btnDelAllMonitor_Click(object sender, EventArgs e)
        {
            foreach (string item in listFilterMonitor) { listMonitor.Add(item); }
            listFilterMonitor.Clear();
            this.tbMonitor_TextChanged(sender, e);
            lbFilterMonitor.ClearSelected();
        }

        private void btnAddGCID_Click(object sender, EventArgs e)
        {
            int[] arr = new int[lbGCID.SelectedItems.Count];
            lbGCID.SelectedIndices.CopyTo(arr, 0);
            for (int index = lbGCID.SelectedItems.Count - 1; index >= 0; index--)
            {
                string item = (string)lbGCID.Items[arr[index]];
                listFilterGCID.Add(item);
                listGCID.Remove(item);
            }
            this.tbGCID_TextChanged(sender, e);
            lbGCID.ClearSelected();
        }

        private void btnAddAllGCID_Click(object sender, EventArgs e)
        {
            foreach (string item in listGCID) { listFilterGCID.Add(item); }
            listGCID.Clear();
            this.tbGCID_TextChanged(sender, e);
            lbGCID.ClearSelected();
        }

        private void btnDelGCID_Click(object sender, EventArgs e)
        {
            int[] arr = new int[lbFilterGCID.SelectedItems.Count];
            lbFilterGCID.SelectedIndices.CopyTo(arr, 0);
            for (int index = lbFilterGCID.SelectedItems.Count - 1; index >= 0; index--)
            {
                listGCID.Add(listFilterGCID[arr[index]]);
                listFilterGCID.RemoveAt(arr[index]);
            }
            this.tbGCID_TextChanged(sender, e);
            lbFilterGCID.ClearSelected();
        }

        private void btnDelAllGCID_Click(object sender, EventArgs e)
        {
            foreach (string item in listFilterGCID) { listGCID.Add(item); }
            listFilterGCID.Clear();
            this.tbGCID_TextChanged(sender, e);
            lbFilterGCID.ClearSelected();
        }


        private void HandleFilter(CustomBindingList<string> sender, ListChangedEventArgs e, string AttrName)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    for (int index = listNode.Count - 1; index >= 0; index--)
                    {
                        if ((string)listNode[index].GetType().GetProperty(AttrName).GetValue(listNode[index], null) == sender[e.NewIndex])
                        {
                            listFilterNode.Add(listNode[index]);
                            listNode.RemoveAt(index);
                        }
                    }
                    break;
                case ListChangedType.ItemDeleted:
                    for (int index = listFilterNode.Count - 1; index >= 0; index--)
                    {
                        if ((string)listFilterNode[index].GetType().GetProperty(AttrName).GetValue(listFilterNode[index], null) == sender.RemovedItem)
                        {
                            listNode.Add(listFilterNode[index]);
                            listFilterNode.RemoveAt(index);
                        }
                    }
                    break;
                case ListChangedType.Reset:
                    foreach (EventNode item in listFilterNode)
                    {
                        listNode.Add(item);
                    }
                    listFilterNode.Clear();
                    break;
                default:
                    break;
            }
        }

        private void listFilterEvent_ListChanged(object sender, ListChangedEventArgs e)
        {
            CustomBindingList<string> handler = (CustomBindingList<string>)sender;
            HandleFilter(handler, e, "EventType");
        }

        private void listFilterMonitor_ListChanged(object sender, ListChangedEventArgs e)
        {
            CustomBindingList<string> handler = (CustomBindingList<string>)sender;
            HandleFilter(handler, e, "Monitor");
        }

        private void listFilterGCID_ListChanged(object sender, ListChangedEventArgs e)
        {
            CustomBindingList<string> handler = (CustomBindingList<string>)sender;
            HandleFilter(handler, e, "GCID");
        }

        private void listNode_ListChanged(object sender, ListChangedEventArgs e)
        {
            CustomBindingList<EventNode> handler = (CustomBindingList<EventNode>)sender;
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                treeLog.Nodes.Add(listNode[e.NewIndex]);
            }
            else if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                treeLog.Nodes.Remove(handler.RemovedItem);

            }
            else if (e.ListChangedType == ListChangedType.Reset)
            {
                treeLog.Nodes.Clear();
            }
            UpdateStatusBar();
        }

        private void listFilterNode_ListChanged(object sender, ListChangedEventArgs e)
        {
            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            tsslEvents.Text = String.Format("Event count = {0}, Filtered event count = {1}",
                this.listNode.Count, this.listFilterNode.Count);
        }
    }
}
