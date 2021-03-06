﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace CSMDevHelper
{

    public partial class frmCSMDH : Form
    {
        private LogFileWatcher m_watcher;

        public frmCSMDH()
        {
            InitializeComponent();
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

            this.listNode = new CustomBindingList<TreeNode>();
            this.listNode.ListChanged += new ListChangedEventHandler(listNode_ListChanged);
            this.listFilterNode = new CustomBindingList<TreeNode>();
            this.listFilterNode.ListChanged += new ListChangedEventHandler(listFilterNode_ListChanged);

            this.log_filename = @"C:\ProgramData\Mitel\Customer Service Manager\Server\Logs\TelDrv.log";

            this.registryHandler = new RegistryHandler();

            updateVersionLabel();
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

            rbtnAuto.Enabled = false;
            rbtnCP.Enabled = false;
            rbtnMCD.Enabled = false;
            btnLogStart.Enabled = false;
            btnLogStop.Enabled = true;
            btnLogPause.Enabled = true;

            enumDriverVersion driverVersion;

            if ((rbtnAuto.Checked &&
                    registryHandler.DriverVersion == enumDriverVersion.CP5000) ||
                    rbtnCP.Checked)
            {
                driverVersion = enumDriverVersion.CP5000;
            }
            else if ((rbtnAuto.Checked &&
                (registryHandler.DriverVersion == enumDriverVersion.MCD4x ||
                registryHandler.DriverVersion == enumDriverVersion.MCD5x)) ||
                rbtnMCD.Checked)
            {
                driverVersion = enumDriverVersion.MCD5x;
            }
            else
            {
                //TODO Fill this section
                throw new Exception("The driver version should be checked!");
            }

            m_watcher = new LogFileWatcher(log_filename, driverVersion, !chkTailing.Checked, this);
            try
            {
                m_watcher.run();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                openLogFile(sender, e);
                btnLogStop_Click(sender, e);
                return;
            }

            blockControls(sender, e);
        }

        private void btnLogStop_Click(object sender, EventArgs e)
        {
            rbtnAuto.Enabled = true;
            rbtnCP.Enabled = true;
            rbtnMCD.Enabled = true;
            btnLogStart.Enabled = true;
            btnLogStop.Enabled = false;
            btnLogPause.Enabled = false;
            btnLogPause.Text = "Pause";
            unblockControls(sender, e);
            m_watcher.stop();
        }

        private void btnLogPause_Click(object sender, EventArgs e)
        {
            if (btnLogPause.Text == "Pause")
            {
                m_watcher.pause();
                btnLogPause.Text = "Resume";
                unblockControls(sender, e);
            }
            else
            {
                m_watcher.resume();
                btnLogPause.Text = "Pause";
                blockControls(sender, e);
            }
        }

        private void blockControls(object sender, EventArgs e)
        {
            treeLog.Enabled = false;
            tbMonitorPage.Enabled = false;
            tbEventPage.Enabled = false;
            tbGCIDPage.Enabled = false;
        }

        private void unblockControls(object sender, EventArgs e)
        {
            treeLog.Enabled = true;
            tbMonitorPage.Enabled = true;
            tbEventPage.Enabled = true;
            tbGCIDPage.Enabled = true;
        }

        public void LogUpdateHandler(LogResult logResult)
        {
            if (IsHandleCreated)
            {
                Invoke(new EventHandler(delegate { LogUpdate(logResult); }));
            }
            else
            {
                LogUpdate(logResult);
            }
        }

        private void LogUpdate(LogResult logResult)
        {
            CSMEvent tag;
            switch (logResult.code)
            {
                case LogCode.LOG_EVENT:
                    rootNode = new TreeNode();
                    rootNode.Tag = logResult.result;
                    tag = (CSMEvent)this.rootNode.Tag;
                    rootNode.Nodes.AddRange(tag.node.ToArray());
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
                    foreach (string gcid in tag.CID)
                    {
                        if (this.dictGCID.ContainsKey(gcid))
                        {
                            this.dictGCID[gcid].Union(tag.CID);
                        }
                        else
                        {
                            this.dictGCID[gcid] = tag.CID;
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
                        rootNode.Text = String.Format("{0} <== Parked", rootNode.Text);
                    }
                    rootNode.ForeColor = tag.GetColor();
                    listNode.Add(rootNode);
                    break;
                case LogCode.LOG_MODELING:
                    rootNode.BackColor = Color.Lavender;
                    break;
                case LogCode.LOG_LEG:
                    break;
                case LogCode.LOG_NOTHING:
                default:
                    break;
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
                        {
                            tag.filterSet |= (int)compareAttribute;
                        }
                    }

                    for (int index = listNode.Count - 1; index >= 0; index--)
                    {
                        tag = (CSMEvent)listNode[index].Tag;
                        if (tag.Compare(compareAttribute, sender[e.NewIndex]))
                        {
                            tag.filterSet |= (int)compareAttribute;
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
                            tag.filterSet &= (int)(~compareAttribute);
                            if (tag.filterSet == 0)
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
            if (ModelingForm ==null)
            {
                if (e.Button == MouseButtons.Middle)
                {
                    TreeNode node = treeLog.GetNodeAt(e.X, e.Y);
                    if (node != null && node.Parent == null)
                    {
                        treeLog.SelectedNode = node;
                        CSMEvent tag = (CSMEvent)node.Tag;
                        ModelingForm = new Form();
                        ModelingForm.SuspendLayout();
                        ModelingForm.StartPosition = FormStartPosition.Manual;
                        ModelingForm.Left = Cursor.Position.X + 5;
                        ModelingForm.Top = Cursor.Position.Y + 5;
                        ModelingForm.Width = 0;
                        ModelingForm.Height = 0;
                        ModelingForm.FormBorderStyle = FormBorderStyle.None;
                        ModelingForm.Name = "frmModeling";
                        ModelingForm.BackColor = System.Drawing.Color.LightGray;
                        RichTextBox eee = new RichTextBox();
                        eee.Margin = new System.Windows.Forms.Padding(0);
                        eee.Font = new System.Drawing.Font("Calibri", 10);
                        eee.Multiline = true;
                        eee.ReadOnly = true;
                        eee.Text = tag.eventInfo.Modeling;
                        int globalindex = 0;
                        foreach (string line in eee.Lines)
                        {
                            int index = line.IndexOf(" = ", 0);
                            if (index > -1)
                            {
                                eee.SelectionStart = globalindex + index + 3;
                                eee.SelectionLength = line.Length - index-3;
                                eee.SelectionFont = new System.Drawing.Font(eee.Font, FontStyle.Bold);
                            }
                            globalindex += line.Length+1;
                        }
                        if (tag.eventInfo.SGCID != default(string))
                        {
                            eee.SelectionStart = 0;
                            eee.SelectionLength = 0;
                            eee.SelectionFont = new System.Drawing.Font(eee.Font, FontStyle.Bold);
                            eee.SelectedText = String.Format("SGCID: {0}{1}", tag.eventInfo.SGCID, Environment.NewLine);
                        }
                        if (tag.eventInfo.PGCID != default(string))
                        {
                            eee.SelectionStart = 0;
                            eee.SelectionLength = 0;
                            eee.SelectionFont = new System.Drawing.Font(eee.Font, FontStyle.Bold);
                            eee.SelectedText = String.Format("PGCID: {0}{1}", tag.eventInfo.PGCID, Environment.NewLine);
                        }
                        if (tag.eventInfo.CGCID != default(string))
                        {
                            eee.SelectionStart = 0;
                            eee.SelectionLength = 0;
                            eee.SelectionFont = new System.Drawing.Font(eee.Font, FontStyle.Bold);
                            eee.SelectedText = String.Format("CGCID: {0}{1}", tag.eventInfo.CGCID, Environment.NewLine);
                        }
                        //Monitor
                        eee.SelectionStart = 0;
                        eee.SelectionLength = 0;
                        eee.SelectionFont = new System.Drawing.Font(eee.Font, FontStyle.Bold);
                        eee.SelectedText = String.Format("Monitor: {0}{1}", tag.Monitor, Environment.NewLine);
                        //Event
                        eee.SelectionStart = 0;
                        eee.SelectionLength = 0;
                        eee.SelectionFont = new System.Drawing.Font(eee.Font, FontStyle.Bold);
                        eee.SelectedText = String.Format("Event: {0}{1}", tag.eventInfo.Type, Environment.NewLine);

                        eee.Size = eee.PreferredSize;
                        ModelingForm.Controls.AddRange(new Control[] {eee});
                        ModelingForm.AutoSize = true;
                        ModelingForm.Show(this);
                        ModelingForm.ResumeLayout();
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    TreeNode node = treeLog.GetNodeAt(e.X, e.Y);
                    if (node != null && node.Parent == null)
                    {
                        treeLog.SelectedNode = node;
                        string result = (node.Text + Environment.NewLine).TrimStart();
                        foreach (TreeNode inode in node.Nodes)
                        {
                            result += "    " + inode.Text + Environment.NewLine;
                        }
                        Clipboard.SetText(result);
                    }
                }
            }
            else
            {
                ModelingForm.Close();
                ModelingForm = null;
            }
        }

        private void treeLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (ModelingForm != null)
            {
                ModelingForm.Close();
                ModelingForm = null;
            }
        }

        private void openLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openLogFile(sender, e);
        }

        private void openLogFile(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select LOG file";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                log_filename = dialog.FileName;
                if (log_filename.Equals(default_log_filename, StringComparison.CurrentCultureIgnoreCase))
                {
                    rbtnAuto.Visible = true;
                    rbtnMCD.Visible = false;
                    rbtnCP.Visible = false;
                }
                else
                {
                    rbtnAuto.Visible = false;
                    rbtnMCD.Visible = true;
                    rbtnCP.Visible = true;
                }
                this.btnLogStop_Click(sender, e);
            }
        }

        private void updateVersionLabel()
        {
            label1.Text = String.Format("{0,-13}{1}{14}{2,-13}{3}{14}{4,-13}{5}{14}{6,-13}{7}{14}{8,-13}{9}{14}{10,-13}{11}{14}{12,-13}{13}",
                "Client: ",
                registryHandler.ClientVersion,
                "DataManager: ",
                registryHandler.DataManagerVersion,
                "RealViewer: ",
                registryHandler.RealViewerVersion,
                "Reporter: ",
                registryHandler.ReporterVersion,
                "ReporterRT: ",
                registryHandler.ReporterRealTimeVersion,
                "Router: ",
                registryHandler.RouterVersion,
                "Server: ",
                registryHandler.ServerVersion,
                Environment.NewLine);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ModelingForm == null)
            {
                ModelingForm = new Form();
                ModelingForm.StartPosition = FormStartPosition.Manual;
                ModelingForm.Left = Cursor.Position.X + 5;
                ModelingForm.Top = Cursor.Position.Y + 5;
                ModelingForm.FormBorderStyle = FormBorderStyle.Sizable;
                ModelingForm.Name = "frmModeling";
                ModelingForm.BackColor = System.Drawing.Color.LightGray;
                RichTextBox eee = new RichTextBox();
                eee.Margin = new System.Windows.Forms.Padding(0);
                eee.Font = new System.Drawing.Font("Calibri", 10);
                eee.Multiline = true;
                eee.ReadOnly = true;
                TabControl tbc = new TabControl();
                SplitContainer sc = new SplitContainer();
                TableLayoutPanel tbl = new TableLayoutPanel();
                Button b = new Button();
                b.Text = "Hello";
                b.Dock = DockStyle.Fill;
                tbl.Controls.Add(b, 0, 0);
                tbl.Controls.Add(new Button(), 0, 1);
                tbl.Controls.Add(new Button(), 0, 2);
                tbl.Controls.Add(new Button(), 0, 3);
                tbl.AutoSize = true;
                TableLayoutPanel tbl2 = new TableLayoutPanel();
                b = new Button();
                b.Text = "Hello";
                tbl2.Controls.Add(b, 0, 0);
                tbl2.Controls.Add(new Button(), 0, 1);
                tbl2.Controls.Add(new Button(), 0, 2);
                tbl2.Controls.Add(new Button(), 0, 3);
                tbl2.AutoSize = true;
                tbl2.SuspendLayout();
                tbl.SuspendLayout();
                sc.Panel2.Controls.Add(tbl2);
                sc.Panel1.Controls.Add(tbl);
                sc.Panel1.AutoSize = true;
                sc.AutoSize = true;
                tbl2.ResumeLayout();
                tbl.ResumeLayout();
                sc.BorderStyle = BorderStyle.Fixed3D;
                sc.AutoSize = true;
                string aaa = @"
================================================================
MainCallEndByExtension - sCallIDTelSys = 9D1CE0BF47B8039EDB48 :
     byCallType = INTERNAL
     sDeviceExtension = 3312
     sDeviceDistantOptional = 
     sDeviceExtLst = 
     bIncludeDistConnectnsFromToDevIfInternal = True
     bSearchConnctnsForwards = False
     bOnlyConsiderNonAnsweredGroupCalls = False
     ppMainCallIDsDeleted = 
================================================================
DevCallConnctnDelete - sCallIDTelSys = 9D1CE0BF47B8039EDB48 :
     sDevice = [CONF]
     sCallIDTelSys = 9D1CE0BF47B8039EDB48
     sDeviceDistantEnd = 3312
     bUseDistantEndAlsoIfUsingCallID = True
     bCallInInfoOverride = False
     sCallInInfoGrp = 
     sCallInInfoDDIDigits = 
     bIgnoreNonCallConnections = True
     bSearchForwards = False
     bSearchAllCallConnections = True
     bClearByDevFirstRung = False
     bClearDistantEndConnectionIfFlagsMatch = False
     iClearDistantEndConnectionIfFlagsMatch = 0
     bDoNotAttemptOnHookCalculation = False
================================================================
DevCallConnctnDelete - sCallIDTelSys = 9D1CE0BF47B8039EDB48 :
     sDevice = 3312
     sCallIDTelSys = 9D1CE0BF47B8039EDB48
     sDeviceDistantEnd = 
     bUseDistantEndAlsoIfUsingCallID = False
     bCallInInfoOverride = False
     sCallInInfoGrp = 
     sCallInInfoDDIDigits = 
     bIgnoreNonCallConnections = True
     bSearchForwards = False
     bSearchAllCallConnections = True
     bClearByDevFirstRung = False
     bClearDistantEndConnectionIfFlagsMatch = False
     iClearDistantEndConnectionIfFlagsMatch = 0
     bDoNotAttemptOnHookCalculation = False
================================================================
MainCallEndByExtension - sCallIDTelSys = 9D1CE0BF47B8039EDB48 :
     byCallType = INTERNAL
     sDeviceExtension = 3321
     sDeviceDistantOptional = 
     sDeviceExtLst = 
     bIncludeDistConnectnsFromToDevIfInternal = True
     bSearchConnctnsForwards = False
     bOnlyConsiderNonAnsweredGroupCalls = False
     ppMainCallIDsDeleted = 
================================================================
DevCallConnctnDelete - sCallIDTelSys = 9D1CE0BF47B8039EDB48 :
     sDevice = [CONF]
     sCallIDTelSys = 9D1CE0BF47B8039EDB48
     sDeviceDistantEnd = 3321
     bUseDistantEndAlsoIfUsingCallID = True
     bCallInInfoOverride = False
     sCallInInfoGrp = 
     sCallInInfoDDIDigits = 
     bIgnoreNonCallConnections = True
     bSearchForwards = False
     bSearchAllCallConnections = True
     bClearByDevFirstRung = False
     bClearDistantEndConnectionIfFlagsMatch = False
     iClearDistantEndConnectionIfFlagsMatch = 0
     bDoNotAttemptOnHookCalculation = False
================================================================
DevCallConnctnDelete - sCallIDTelSys = 9D1CE0BF47B8039EDB48 :
     sDevice = 3321
     sCallIDTelSys = 9D1CE0BF47B8039EDB48
     sDeviceDistantEnd = 
     bUseDistantEndAlsoIfUsingCallID = False
     bCallInInfoOverride = False
     sCallInInfoGrp = 
     sCallInInfoDDIDigits = 
     bIgnoreNonCallConnections = True
     bSearchForwards = False
     bSearchAllCallConnections = True
     bClearByDevFirstRung = False
     bClearDistantEndConnectionIfFlagsMatch = False
     iClearDistantEndConnectionIfFlagsMatch = 0
     bDoNotAttemptOnHookCalculation = False
================================================================
DevCallConnctnDelete - sCallIDTelSys = 9D1CE0BF47B8039EDB48 :
     sDevice = [CONF]
     sCallIDTelSys = 9D1CE0BF47B8039EDB48
     sDeviceDistantEnd = 
     bUseDistantEndAlsoIfUsingCallID = False
     bCallInInfoOverride = False
     sCallInInfoGrp = 
     sCallInInfoDDIDigits = 
     bIgnoreNonCallConnections = True
     bSearchForwards = False
     bSearchAllCallConnections = True
     bClearByDevFirstRung = False
     bClearDistantEndConnectionIfFlagsMatch = False
     iClearDistantEndConnectionIfFlagsMatch = 0
     bDoNotAttemptOnHookCalculation = False
================================================================
DevCallAnswer - sCallIDTelSys = 9D1CE0BF47B8039EDB48 :
     sDeviceLocal = 3311, LegID = 
     sDeviceDistant = 3321, LegID = 
     sDeviceGroup = , LegID = 
     bAlwaysUseDevGrpToCalcGrpDistrib = False
     iCallModellingFlags = 15
     sCallIDTelSys = 9D1CE0BF47B8039EDB48
     bCalcMainCallIDFromConnectns = True
     bMarkAnsweredIn = True
     bMarkAnsweredOut = True
     bMarkUnHeldIn = True
     bMarkUnHeldOut = True
     bDeleteCallConnectnsElsewhere = False
     bUseCallAnswerFlagForCallConnectns = False
     bDegenerateCallConference = True
     bSegAlgrthmRun = True
     iSegAlgrthmFlags = 257
================================================================
MainCallEnd - sCallIDTelSys = 9D1CE0BF47B8039EDB48 :
     sCallIDTelSys = 9D1CE0BF47B8039EDB48
     pSDeviceDistant = [CONF], LegID = 
     pSDeviceLocal = 3311, LegID = 
     sDeviceExtLst = 
";
                string[] bbb = System.Text.RegularExpressions.Regex.Split(aaa, String.Format("{1}+{0}{1}+", @"={2,}\s*", Environment.NewLine));//, System.Text.RegularExpressions.RegexOptions.Multiline);
                int counter = 1;
                foreach (string item in bbb)
                {
                    eee.Text += item;
                    if (!String.IsNullOrWhiteSpace(item))
                    {
                        string []info = item.Split(new string[]{" - "}, StringSplitOptions.RemoveEmptyEntries);
                        if (info.Length == 2)
                        {
                            TabPage tb = new TabPage(counter.ToString());
                            RichTextBox tbText = new RichTextBox();
                            tbText.Text = info[0] + Environment.NewLine;
                            tbText.Text += "     " + info[1];
                            tbText.Size = tbText.PreferredSize;
                            tb.Controls.Add(tbText);
                            tbc.TabPages.Add(tb);
                            tb.Dock = DockStyle.Fill;
                            counter++;
                        }
                    }
                }
                //tbc.Size = tbc.GetPreferredSize(tbc.Size);
                tbc.Dock = DockStyle.Fill;
                //tbc.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                //int globalindex = 0;
                //foreach (string line in eee.Lines)
                //{
                //    int index = line.IndexOf(" = ", 0);
                //    if (index > -1)
                //    {
                //        eee.SelectionStart = globalindex + index + 3;
                //        eee.SelectionLength = line.Length - index - 3;
                //        eee.SelectionFont = new System.Drawing.Font(eee.Font, FontStyle.Bold);
                //    }
                //    globalindex += line.Length + 1;
                //}
                eee.Size = eee.PreferredSize;
                ModelingForm.SuspendLayout();
                sc.Size = sc.PreferredSize;
                sc.SuspendLayout();
                //ModelingForm.Controls.AddRange(new Control[] { eee, tbc });
                //ModelingForm.Controls.AddRange(new Control[] { tbc });
                ModelingForm.Controls.AddRange(new Control[] { sc });
                ModelingForm.Show(this);
                sc.ResumeLayout();
                ModelingForm.ResumeLayout();
            }
            else
            {
                ModelingForm.Close();
                ModelingForm = null;
            }
        }
    }
}
