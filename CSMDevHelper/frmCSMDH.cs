using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;

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
            this.toolTip = null;
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

            this.listNode = new CustomBindingList<TreeNode>();
            this.listNode.ListChanged += new ListChangedEventHandler(listNode_ListChanged);
            this.listFilterNode = new CustomBindingList<TreeNode>();
            this.listFilterNode.ListChanged += new ListChangedEventHandler(listFilterNode_ListChanged);
            
        }

        private void btnLogStart_Click(object sender, EventArgs e)
        {
            listNode.Clear();
            listFilterNode.Clear();
            tbEvent.Clear();
            listEvent.Clear();
            tbFilterEvent.Clear();
            listFilterEvent.Clear();
            tbMonitor.Clear();
            listMonitor.Clear();
            tbFilterMonitor.Clear();
            listFilterMonitor.Clear();
            tbGCID.Clear();
            listGCID.Clear();
            tbFilterGCID.Clear();
            listFilterGCID.Clear();

            treeLog.Enabled = false;
            tbMonitorPage.Enabled = false;
            tbEventPage.Enabled = false;
            tbGCIDPage.Enabled = false;
            //tcLogFilters.Enabled = false;

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
            tbMonitorPage.Enabled = true;
            tbEventPage.Enabled = true;
            tbGCIDPage.Enabled = true;

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
            LogResult logResult;
            //LogReader logReader = new LogReader(@"C:\GA_logs.txt", false);
            LogReader logReader = new LogCPReader(@"C:\ProgramData\Mitel\Customer Service Manager\Server\Logs\TelDrv.log", false);
            //LogReader logReader = new LogMCDReader(@"C:\ProgramData\Mitel\Customer Service Manager\Server\Logs\TelDrv.log", false);
            //LogReader logReader = new LogReader(@"C:\ClearedTelDrv.txt", true);
            while(this.isLogUpdate)
            {
                logResult = logReader.Process();
                Invoke(this.myDelegate, logResult);
            }
        }

        void LogUpdate(LogResult logResult)
        {
            if (this.isLogUpdate)
            {
                CSMEvent tag;
                switch (logResult.code)
                {
                    case LogCode.LOG_EVENT:
                        rootNode = new TreeNode();
                        rootNode.Tag = logResult.result;
                        tag = (CSMEvent)this.rootNode.Tag;
                        rootNode.Nodes.AddRange(tag.node);
                        // Updating Event checklist
                        if (!lbEvent.Items.Contains(tag.eventInfo.Type))
                        {
                            this.listEvent.Add(tag.eventInfo.Type);
                        }
                        // Updating Monitor checklist
                        if (!listMonitor.Contains(tag.Monitor))
                        {
                            listMonitor.Add(tag.Monitor);
                        }
                        //Updating GCID checklist
                        foreach (string gcid in tag.GCID)
                        {
                            if (this.dictGCID.ContainsKey(gcid))
                            {
                                this.dictGCID[gcid].Union(tag.GCID);
                            }
                            else
                            {
                                this.dictGCID[gcid] = tag.GCID;
                            }
                            if (!listGCID.Contains(gcid))
                            {
                                listGCID.Add(gcid);
                            }
                        }
                        rootNode.Name = tag.eventInfo.Type;
                        rootNode.Text = String.Format("{0,4}> {1,-25}: {2,-20} ({3,15})", treeLog.Nodes.Count + 1, tag.eventInfo.Type, tag.eventInfo.Cause, tag.eventInfo.TimeStamp);
                        if (tag.Parked)
                        {
                            rootNode.Text = String.Format("{0} <== Parked", this.Text);
                        }
                        rootNode.ForeColor = tag.GetColor();
                        listNode.Add(rootNode);
                        break;
                    case LogCode.LOG_MODELING:
                        this.rootNode.BackColor = Color.LightGoldenrodYellow;
                        break;
                    case LogCode.LOG_LEG:
                        break;
                    case LogCode.LOG_NOTHING:
                    default:
                        break;
                }
            }
        }

        private void tbEvent_TextChanged(object sender, EventArgs e)
        {
            TextboxUpdate(sender, e, lbEvent, tbEvent, listEvent);
        }

        private void tbFilterEvent_TextChanged(object sender, EventArgs e)
        {
            TextboxUpdate(sender, e, lbFilterEvent, tbFilterEvent, listFilterEvent);
        }

        private void tbMonitor_TextChanged(object sender, EventArgs e)
        {
            TextboxUpdate(sender, e, lbMonitor, tbMonitor, listMonitor);
        }

        private void tbFilterMonitor_TextChanged(object sender, EventArgs e)
        {
            TextboxUpdate(sender, e, lbFilterMonitor, tbFilterMonitor, listFilterMonitor);
        }

        private void tbGCID_TextChanged(object sender, EventArgs e)
        {
            TextboxUpdate(sender, e, lbGCID, tbGCID, listGCID);
        }

        private void tbFilterGCID_TextChanged(object sender, EventArgs e)
        {
            TextboxUpdate(sender, e, lbFilterGCID, tbFilterGCID, listFilterGCID);
        }

        private void TextboxUpdate(object sender, EventArgs e, ListBox lb, TextBox tb, BindingList<string> lst)
        {
            if (tb.Text != String.Empty)
            {
                IEnumerable<string> filtered = lst.Where((i) => i.IndexOf(tb.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                if (filtered.Count<string>() != 0)
                {
                    lb.DataSource = new BindingSource(filtered, "");
                }
                else
                {
                    lb.DataSource = null;
                }
            }
            else
            {
                lb.DataSource = lst;
            }
        }

        private void btnAddEvent_Click(object sender, EventArgs e)
        {
            btnFilter(sender, e, lbEvent, tbEvent, tbFilterEvent, listEvent, listFilterEvent);
        }

        private void btnAddAllEvent_Click(object sender, EventArgs e)
        {
            btnFilterAll(sender, e, lbEvent, tbEvent, tbFilterEvent, listEvent, listFilterEvent);
        }

        private void btnDelEvent_Click(object sender, EventArgs e)
        {
            btnFilter(sender, e, lbFilterEvent, tbEvent, tbFilterEvent, listFilterEvent, listEvent);
        }

        private void btnDelAllEvent_Click(object sender, EventArgs e)
        {
            btnFilterAll(sender, e, lbFilterEvent, tbEvent, tbFilterEvent, listFilterEvent, listEvent);
        }


        private void btnAddMonitor_Click(object sender, EventArgs e)
        {
            btnFilter(sender, e, lbMonitor, tbMonitor, tbFilterMonitor, listMonitor, listFilterMonitor);
        }

        private void btnAddAllMonitor_Click(object sender, EventArgs e)
        {
            btnFilterAll(sender, e, lbMonitor, tbMonitor, tbFilterMonitor, listMonitor, listFilterMonitor);
        }

        private void btnDelMonitor_Click(object sender, EventArgs e)
        {
            btnFilter(sender, e, lbFilterMonitor, tbMonitor, tbFilterMonitor, listFilterMonitor, listMonitor);
        }

        private void btnDelAllMonitor_Click(object sender, EventArgs e)
        {
            btnFilterAll(sender, e, lbFilterMonitor, tbMonitor, tbFilterMonitor, listFilterMonitor, listMonitor);
        }

        private void btnAddGCID_Click(object sender, EventArgs e)
        {
            btnFilter(sender, e, lbGCID, tbGCID, tbFilterGCID, listGCID, listFilterGCID);
        }

        private void btnAddAllGCID_Click(object sender, EventArgs e)
        {
            btnFilterAll(sender, e, lbGCID, tbGCID, tbFilterGCID, listGCID, listFilterGCID);
        }

        private void btnDelGCID_Click(object sender, EventArgs e)
        {
            btnFilter(sender, e, lbFilterGCID, tbGCID, tbFilterGCID, listFilterGCID, listGCID);
        }

        private void btnDelAllGCID_Click(object sender, EventArgs e)
        {
            btnFilterAll(sender, e, lbFilterGCID, tbGCID, tbFilterGCID, listFilterGCID, listGCID);
        }

        private void btnFilter(object sender, EventArgs e, ListBox lb, TextBox tb1, TextBox tb2, BindingList<string> lstDel, BindingList<string> lstAdd)
        {
            int[] arr = new int[lb.SelectedItems.Count];
            lb.SelectedIndices.CopyTo(arr, 0);
            for (int index = lb.SelectedItems.Count - 1; index >= 0; index--)
            {
                string item = (string)lb.Items[arr[index]];
                lstAdd.Add(item);
                lstDel.Remove(item);
            }
            FireEvent(tb1, "TextChanged", e);
            FireEvent(tb2, "TextChanged", e);
            lb.ClearSelected();
        }

        private void btnFilterAll(object sender, EventArgs e, ListBox lb, TextBox tb1, TextBox tb2, BindingList<string> lstDel, BindingList<string> lstAdd)
        {
            for (int index = 0; index < lb.Items.Count; index++)
            {
                lb.SetSelected(index, true);
            }
            btnFilter(sender, e, lb, tb1, tb2, lstDel, lstAdd);
        }


        private void listFilterEvent_ListChanged(object sender, ListChangedEventArgs e)
        {
            CustomBindingList<string> handler = (CustomBindingList<string>)sender;
            HandleFilter(handler, e, FilterComparator.EVENT_TYPE);
        }

        private void listFilterMonitor_ListChanged(object sender, ListChangedEventArgs e)
        {
            CustomBindingList<string> handler = (CustomBindingList<string>)sender;
            HandleFilter(handler, e, FilterComparator.EVENT_MONITOR);
        }

        private void listFilterGCID_ListChanged(object sender, ListChangedEventArgs e)
        {
            CustomBindingList<string> handler = (CustomBindingList<string>)sender;
            HandleFilter(handler, e, FilterComparator.EVENT_GCID);
        }

        private void HandleFilter(CustomBindingList<string> sender, ListChangedEventArgs e, FilterComparator compareAttribute)
        {
            CSMEvent tag;
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    foreach (TreeNode node in listFilterNode)
                    {
                        tag = (CSMEvent)node.Tag;
                        if (tag.Compare(compareAttribute, sender[e.NewIndex]))
                            tag.filterCount += 1;
                    }

                    for (int index = listNode.Count - 1; index >= 0; index--)
                    {
                        tag = (CSMEvent)listNode[index].Tag;
                        if (tag.Compare(compareAttribute, sender[e.NewIndex]))
                        {
                            tag.filterCount += 1;
                            listFilterNode.Add(listNode[index]);
                            listNode.RemoveAt(index);
                        }
                    }
                    break;
                case ListChangedType.ItemDeleted:
                    for (int index = listFilterNode.Count - 1; index >= 0; index--)
                    {
                        tag = (CSMEvent)listFilterNode[index].Tag;
                        if (tag.Compare(compareAttribute, sender.RemovedItem))
                        {
                            tag.filterCount -= 1;
                            if (tag.filterCount == 0)
                            {
                                listNode.Add(listFilterNode[index]);
                                listFilterNode.RemoveAt(index);
                            }
                        }
                    }
                    break;
                case ListChangedType.Reset:
                    foreach (TreeNode item in listFilterNode)
                    {
                        listNode.Add(item);
                    }
                    listFilterNode.Clear();
                    break;
                default:
                    break;
            }
        }


        private void listNode_ListChanged(object sender, ListChangedEventArgs e)
        {
            CustomBindingList<TreeNode> handler = (CustomBindingList<TreeNode>)sender;
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


        private static void FireEvent(Object targetObject, string eventName, EventArgs e)
        {
            String methodName = "On" + eventName;

            MethodInfo mi = targetObject.GetType().GetMethod(
                  methodName,
                  BindingFlags.Instance | BindingFlags.NonPublic);

            if (mi == null)
                throw new ArgumentException("Cannot find event thrower named " + methodName);

            mi.Invoke(targetObject, new object[] { e });
        }

        private void treeLog_MouseDown(object sender, MouseEventArgs e)
        {
            if (toolTip == null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    TreeNode node = treeLog.GetNodeAt(e.X, e.Y);
                    if (node != null && node.Parent == null)
                    {
                        CSMEvent tag = (CSMEvent)node.Tag;
                        string msgToolTip = String.Format("{0:4}Monitor: {1}", String.Empty, tag.eventInfo.MonitorHandlerExtension);
                        if (tag.eventInfo.CGCID != default(string))
                        {
                            msgToolTip += String.Format("{0}{1:4}CGCID: {2}", Environment.NewLine, String.Empty, tag.eventInfo.CGCID);
                        }
                        if (tag.eventInfo.PGCID != default(string))
                        {
                            msgToolTip += String.Format("{0}{1:4}PGCID: {2}", Environment.NewLine, String.Empty, tag.eventInfo.PGCID);
                        }
                        if (tag.eventInfo.SGCID != default(string))
                        {
                            msgToolTip += String.Format("{0}{1:4}SGCID: {2}", Environment.NewLine, String.Empty, tag.eventInfo.SGCID);
                        }
                        msgToolTip += String.Format("{0}{1}", Environment.NewLine, tag.eventInfo.Modeling);
                        toolTip = new ToolTip();
                        toolTip.ToolTipTitle = String.Format("Event: {0}", tag.eventInfo.Type);
                        toolTip.UseAnimation = false;
                        toolTip.UseFading = false;
                        toolTip.Show(msgToolTip, (IWin32Window)sender, e.X, e.Y);
                        treeLog.SelectedNode = node;
                    }
                }
            }
            else
            {
                toolTip.Hide(treeLog);
                toolTip = null;
            }
        }

        private void treeLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (toolTip != null)
            {
                toolTip.Hide(treeLog);
                toolTip = null;
            }
        }
    }
}
