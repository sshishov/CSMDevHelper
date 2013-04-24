using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;

namespace CSMDevHelper
{
    partial class frmCSMDH
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tcCSMDH = new System.Windows.Forms.TabControl();
            this.tbLogPage = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLogPause = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbtnAuto = new System.Windows.Forms.RadioButton();
            this.rbtnCP = new System.Windows.Forms.RadioButton();
            this.rbtnMCD = new System.Windows.Forms.RadioButton();
            this.chkTailing = new System.Windows.Forms.CheckBox();
            this.btnLogStop = new System.Windows.Forms.Button();
            this.ssEvents = new System.Windows.Forms.StatusStrip();
            this.tsslEvents = new System.Windows.Forms.ToolStripStatusLabel();
            this.tcLogFilters = new System.Windows.Forms.TabControl();
            this.tbEventPage = new System.Windows.Forms.TabPage();
            this.tbFilterEvent = new System.Windows.Forms.TextBox();
            this.btnDelAllEvent = new System.Windows.Forms.Button();
            this.btnDelEvent = new System.Windows.Forms.Button();
            this.btnAddAllEvent = new System.Windows.Forms.Button();
            this.btnAddEvent = new System.Windows.Forms.Button();
            this.lbEvent = new System.Windows.Forms.ListBox();
            this.lbFilterEvent = new System.Windows.Forms.ListBox();
            this.tbEvent = new System.Windows.Forms.TextBox();
            this.tbMonitorPage = new System.Windows.Forms.TabPage();
            this.tbFilterMonitor = new System.Windows.Forms.TextBox();
            this.btnDelAllMonitor = new System.Windows.Forms.Button();
            this.btnDelMonitor = new System.Windows.Forms.Button();
            this.btnAddAllMonitor = new System.Windows.Forms.Button();
            this.btnAddMonitor = new System.Windows.Forms.Button();
            this.lbMonitor = new System.Windows.Forms.ListBox();
            this.lbFilterMonitor = new System.Windows.Forms.ListBox();
            this.tbMonitor = new System.Windows.Forms.TextBox();
            this.tbGCIDPage = new System.Windows.Forms.TabPage();
            this.tbFilterGCID = new System.Windows.Forms.TextBox();
            this.btnDelAllGCID = new System.Windows.Forms.Button();
            this.btnDelGCID = new System.Windows.Forms.Button();
            this.btnAddAllGCID = new System.Windows.Forms.Button();
            this.btnAddGCID = new System.Windows.Forms.Button();
            this.lbGCID = new System.Windows.Forms.ListBox();
            this.lbFilterGCID = new System.Windows.Forms.ListBox();
            this.tbGCID = new System.Windows.Forms.TextBox();
            this.btnLogStart = new System.Windows.Forms.Button();
            this.treeLog = new System.Windows.Forms.TreeView();
            this.tbGeneralPage = new System.Windows.Forms.TabPage();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fIleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tcCSMDH.SuspendLayout();
            this.tbLogPage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.ssEvents.SuspendLayout();
            this.tcLogFilters.SuspendLayout();
            this.tbEventPage.SuspendLayout();
            this.tbMonitorPage.SuspendLayout();
            this.tbGCIDPage.SuspendLayout();
            this.tbGeneralPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcCSMDH
            // 
            this.tcCSMDH.Controls.Add(this.tbLogPage);
            this.tcCSMDH.Controls.Add(this.tbGeneralPage);
            this.tcCSMDH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcCSMDH.Location = new System.Drawing.Point(10, 34);
            this.tcCSMDH.Name = "tcCSMDH";
            this.tcCSMDH.SelectedIndex = 0;
            this.tcCSMDH.Size = new System.Drawing.Size(1212, 610);
            this.tcCSMDH.TabIndex = 0;
            // 
            // tbLogPage
            // 
            this.tbLogPage.Controls.Add(this.panel1);
            this.tbLogPage.Controls.Add(this.treeLog);
            this.tbLogPage.Location = new System.Drawing.Point(4, 22);
            this.tbLogPage.Name = "tbLogPage";
            this.tbLogPage.Padding = new System.Windows.Forms.Padding(10);
            this.tbLogPage.Size = new System.Drawing.Size(1204, 584);
            this.tbLogPage.TabIndex = 1;
            this.tbLogPage.Text = "Logging";
            this.tbLogPage.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnLogPause);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.chkTailing);
            this.panel1.Controls.Add(this.btnLogStop);
            this.panel1.Controls.Add(this.ssEvents);
            this.panel1.Controls.Add(this.tcLogFilters);
            this.panel1.Controls.Add(this.btnLogStart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(656, 10);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(538, 564);
            this.panel1.TabIndex = 9;
            // 
            // btnLogPause
            // 
            this.btnLogPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogPause.Enabled = false;
            this.btnLogPause.Location = new System.Drawing.Point(404, 13);
            this.btnLogPause.Name = "btnLogPause";
            this.btnLogPause.Size = new System.Drawing.Size(60, 22);
            this.btnLogPause.TabIndex = 11;
            this.btnLogPause.Text = "Pause";
            this.btnLogPause.UseVisualStyleBackColor = true;
            this.btnLogPause.Click += new System.EventHandler(this.btnLogPause_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbtnAuto);
            this.panel2.Controls.Add(this.rbtnCP);
            this.panel2.Controls.Add(this.rbtnMCD);
            this.panel2.Location = new System.Drawing.Point(13, 10);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(203, 25);
            this.panel2.TabIndex = 10;
            // 
            // rbtnAuto
            // 
            this.rbtnAuto.AutoSize = true;
            this.rbtnAuto.Checked = true;
            this.rbtnAuto.Location = new System.Drawing.Point(3, 6);
            this.rbtnAuto.Name = "rbtnAuto";
            this.rbtnAuto.Size = new System.Drawing.Size(47, 17);
            this.rbtnAuto.TabIndex = 10;
            this.rbtnAuto.TabStop = true;
            this.rbtnAuto.Text = "Auto";
            this.rbtnAuto.UseVisualStyleBackColor = true;
            // 
            // rbtnCP
            // 
            this.rbtnCP.AutoSize = true;
            this.rbtnCP.Location = new System.Drawing.Point(136, 6);
            this.rbtnCP.Name = "rbtnCP";
            this.rbtnCP.Size = new System.Drawing.Size(63, 17);
            this.rbtnCP.TabIndex = 12;
            this.rbtnCP.Text = "CP5000";
            this.rbtnCP.UseVisualStyleBackColor = true;
            this.rbtnCP.Visible = false;
            // 
            // rbtnMCD
            // 
            this.rbtnMCD.AutoSize = true;
            this.rbtnMCD.Location = new System.Drawing.Point(57, 6);
            this.rbtnMCD.Name = "rbtnMCD";
            this.rbtnMCD.Size = new System.Drawing.Size(73, 17);
            this.rbtnMCD.TabIndex = 11;
            this.rbtnMCD.Text = "MCD3300";
            this.rbtnMCD.UseVisualStyleBackColor = true;
            this.rbtnMCD.Visible = false;
            // 
            // chkTailing
            // 
            this.chkTailing.AutoSize = true;
            this.chkTailing.Checked = true;
            this.chkTailing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTailing.Location = new System.Drawing.Point(222, 17);
            this.chkTailing.Name = "chkTailing";
            this.chkTailing.Size = new System.Drawing.Size(81, 17);
            this.chkTailing.TabIndex = 9;
            this.chkTailing.Text = "Tailing read";
            this.chkTailing.UseVisualStyleBackColor = true;
            // 
            // btnLogStop
            // 
            this.btnLogStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogStop.Enabled = false;
            this.btnLogStop.Location = new System.Drawing.Point(470, 13);
            this.btnLogStop.Name = "btnLogStop";
            this.btnLogStop.Size = new System.Drawing.Size(55, 22);
            this.btnLogStop.TabIndex = 3;
            this.btnLogStop.Text = "Stop";
            this.btnLogStop.UseVisualStyleBackColor = true;
            this.btnLogStop.Click += new System.EventHandler(this.btnLogStop_Click);
            // 
            // ssEvents
            // 
            this.ssEvents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslEvents});
            this.ssEvents.Location = new System.Drawing.Point(10, 532);
            this.ssEvents.Name = "ssEvents";
            this.ssEvents.Size = new System.Drawing.Size(518, 22);
            this.ssEvents.TabIndex = 8;
            this.ssEvents.Text = "statusStrip1";
            // 
            // tsslEvents
            // 
            this.tsslEvents.Name = "tsslEvents";
            this.tsslEvents.Size = new System.Drawing.Size(0, 17);
            // 
            // tcLogFilters
            // 
            this.tcLogFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcLogFilters.Controls.Add(this.tbEventPage);
            this.tcLogFilters.Controls.Add(this.tbMonitorPage);
            this.tcLogFilters.Controls.Add(this.tbGCIDPage);
            this.tcLogFilters.Location = new System.Drawing.Point(13, 41);
            this.tcLogFilters.Name = "tcLogFilters";
            this.tcLogFilters.SelectedIndex = 0;
            this.tcLogFilters.Size = new System.Drawing.Size(512, 488);
            this.tcLogFilters.TabIndex = 7;
            // 
            // tbEventPage
            // 
            this.tbEventPage.Controls.Add(this.tbFilterEvent);
            this.tbEventPage.Controls.Add(this.btnDelAllEvent);
            this.tbEventPage.Controls.Add(this.btnDelEvent);
            this.tbEventPage.Controls.Add(this.btnAddAllEvent);
            this.tbEventPage.Controls.Add(this.btnAddEvent);
            this.tbEventPage.Controls.Add(this.lbEvent);
            this.tbEventPage.Controls.Add(this.lbFilterEvent);
            this.tbEventPage.Controls.Add(this.tbEvent);
            this.tbEventPage.Location = new System.Drawing.Point(4, 22);
            this.tbEventPage.Name = "tbEventPage";
            this.tbEventPage.Padding = new System.Windows.Forms.Padding(10);
            this.tbEventPage.Size = new System.Drawing.Size(504, 462);
            this.tbEventPage.TabIndex = 0;
            this.tbEventPage.Text = "Event";
            this.tbEventPage.UseVisualStyleBackColor = true;
            // 
            // tbFilterEvent
            // 
            this.tbFilterEvent.Location = new System.Drawing.Point(281, 13);
            this.tbFilterEvent.Name = "tbFilterEvent";
            this.tbFilterEvent.Size = new System.Drawing.Size(210, 20);
            this.tbFilterEvent.TabIndex = 22;
            this.tbFilterEvent.TextChanged += new System.EventHandler(this.tbFilterEvent_TextChanged);
            // 
            // btnDelAllEvent
            // 
            this.btnDelAllEvent.Location = new System.Drawing.Point(229, 308);
            this.btnDelAllEvent.Name = "btnDelAllEvent";
            this.btnDelAllEvent.Size = new System.Drawing.Size(46, 60);
            this.btnDelAllEvent.TabIndex = 21;
            this.btnDelAllEvent.Text = "<<";
            this.btnDelAllEvent.UseVisualStyleBackColor = true;
            this.btnDelAllEvent.Click += new System.EventHandler(this.btnDelAllEvent_Click);
            // 
            // btnDelEvent
            // 
            this.btnDelEvent.Location = new System.Drawing.Point(229, 242);
            this.btnDelEvent.Name = "btnDelEvent";
            this.btnDelEvent.Size = new System.Drawing.Size(46, 60);
            this.btnDelEvent.TabIndex = 20;
            this.btnDelEvent.Text = "<";
            this.btnDelEvent.UseVisualStyleBackColor = true;
            this.btnDelEvent.Click += new System.EventHandler(this.btnDelEvent_Click);
            // 
            // btnAddAllEvent
            // 
            this.btnAddAllEvent.Location = new System.Drawing.Point(229, 176);
            this.btnAddAllEvent.Name = "btnAddAllEvent";
            this.btnAddAllEvent.Size = new System.Drawing.Size(46, 60);
            this.btnAddAllEvent.TabIndex = 19;
            this.btnAddAllEvent.Text = ">>";
            this.btnAddAllEvent.UseVisualStyleBackColor = true;
            this.btnAddAllEvent.Click += new System.EventHandler(this.btnAddAllEvent_Click);
            // 
            // btnAddEvent
            // 
            this.btnAddEvent.Location = new System.Drawing.Point(229, 110);
            this.btnAddEvent.Name = "btnAddEvent";
            this.btnAddEvent.Size = new System.Drawing.Size(46, 60);
            this.btnAddEvent.TabIndex = 18;
            this.btnAddEvent.Text = ">";
            this.btnAddEvent.UseVisualStyleBackColor = true;
            this.btnAddEvent.Click += new System.EventHandler(this.btnAddEvent_Click);
            // 
            // lbEvent
            // 
            this.lbEvent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbEvent.FormattingEnabled = true;
            this.lbEvent.Location = new System.Drawing.Point(13, 39);
            this.lbEvent.Name = "lbEvent";
            this.lbEvent.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbEvent.Size = new System.Drawing.Size(210, 381);
            this.lbEvent.TabIndex = 17;
            // 
            // lbFilterEvent
            // 
            this.lbFilterEvent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFilterEvent.FormattingEnabled = true;
            this.lbFilterEvent.Location = new System.Drawing.Point(281, 39);
            this.lbFilterEvent.Name = "lbFilterEvent";
            this.lbFilterEvent.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbFilterEvent.Size = new System.Drawing.Size(210, 381);
            this.lbFilterEvent.TabIndex = 16;
            // 
            // tbEvent
            // 
            this.tbEvent.Location = new System.Drawing.Point(13, 13);
            this.tbEvent.Name = "tbEvent";
            this.tbEvent.Size = new System.Drawing.Size(210, 20);
            this.tbEvent.TabIndex = 15;
            this.tbEvent.TextChanged += new System.EventHandler(this.tbEvent_TextChanged);
            // 
            // tbMonitorPage
            // 
            this.tbMonitorPage.Controls.Add(this.tbFilterMonitor);
            this.tbMonitorPage.Controls.Add(this.btnDelAllMonitor);
            this.tbMonitorPage.Controls.Add(this.btnDelMonitor);
            this.tbMonitorPage.Controls.Add(this.btnAddAllMonitor);
            this.tbMonitorPage.Controls.Add(this.btnAddMonitor);
            this.tbMonitorPage.Controls.Add(this.lbMonitor);
            this.tbMonitorPage.Controls.Add(this.lbFilterMonitor);
            this.tbMonitorPage.Controls.Add(this.tbMonitor);
            this.tbMonitorPage.Location = new System.Drawing.Point(4, 22);
            this.tbMonitorPage.Name = "tbMonitorPage";
            this.tbMonitorPage.Padding = new System.Windows.Forms.Padding(3);
            this.tbMonitorPage.Size = new System.Drawing.Size(504, 462);
            this.tbMonitorPage.TabIndex = 1;
            this.tbMonitorPage.Text = "Monitor";
            this.tbMonitorPage.UseVisualStyleBackColor = true;
            // 
            // tbFilterMonitor
            // 
            this.tbFilterMonitor.Location = new System.Drawing.Point(249, 9);
            this.tbFilterMonitor.Name = "tbFilterMonitor";
            this.tbFilterMonitor.Size = new System.Drawing.Size(203, 20);
            this.tbFilterMonitor.TabIndex = 22;
            this.tbFilterMonitor.TextChanged += new System.EventHandler(this.tbFilterMonitor_TextChanged);
            // 
            // btnDelAllMonitor
            // 
            this.btnDelAllMonitor.Location = new System.Drawing.Point(211, 255);
            this.btnDelAllMonitor.Name = "btnDelAllMonitor";
            this.btnDelAllMonitor.Size = new System.Drawing.Size(32, 32);
            this.btnDelAllMonitor.TabIndex = 21;
            this.btnDelAllMonitor.Text = "<<";
            this.btnDelAllMonitor.UseVisualStyleBackColor = true;
            this.btnDelAllMonitor.Click += new System.EventHandler(this.btnDelAllMonitor_Click);
            // 
            // btnDelMonitor
            // 
            this.btnDelMonitor.Location = new System.Drawing.Point(211, 217);
            this.btnDelMonitor.Name = "btnDelMonitor";
            this.btnDelMonitor.Size = new System.Drawing.Size(32, 32);
            this.btnDelMonitor.TabIndex = 20;
            this.btnDelMonitor.Text = "<";
            this.btnDelMonitor.UseVisualStyleBackColor = true;
            this.btnDelMonitor.Click += new System.EventHandler(this.btnDelMonitor_Click);
            // 
            // btnAddAllMonitor
            // 
            this.btnAddAllMonitor.Location = new System.Drawing.Point(211, 134);
            this.btnAddAllMonitor.Name = "btnAddAllMonitor";
            this.btnAddAllMonitor.Size = new System.Drawing.Size(32, 32);
            this.btnAddAllMonitor.TabIndex = 19;
            this.btnAddAllMonitor.Text = ">>";
            this.btnAddAllMonitor.UseVisualStyleBackColor = true;
            this.btnAddAllMonitor.Click += new System.EventHandler(this.btnAddAllMonitor_Click);
            // 
            // btnAddMonitor
            // 
            this.btnAddMonitor.Location = new System.Drawing.Point(211, 96);
            this.btnAddMonitor.Name = "btnAddMonitor";
            this.btnAddMonitor.Size = new System.Drawing.Size(32, 32);
            this.btnAddMonitor.TabIndex = 18;
            this.btnAddMonitor.Text = ">";
            this.btnAddMonitor.UseVisualStyleBackColor = true;
            this.btnAddMonitor.Click += new System.EventHandler(this.btnAddMonitor_Click);
            // 
            // lbMonitor
            // 
            this.lbMonitor.FormattingEnabled = true;
            this.lbMonitor.Location = new System.Drawing.Point(2, 35);
            this.lbMonitor.Name = "lbMonitor";
            this.lbMonitor.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbMonitor.Size = new System.Drawing.Size(203, 342);
            this.lbMonitor.Sorted = true;
            this.lbMonitor.TabIndex = 17;
            // 
            // lbFilterMonitor
            // 
            this.lbFilterMonitor.FormattingEnabled = true;
            this.lbFilterMonitor.Location = new System.Drawing.Point(249, 35);
            this.lbFilterMonitor.Name = "lbFilterMonitor";
            this.lbFilterMonitor.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbFilterMonitor.Size = new System.Drawing.Size(203, 342);
            this.lbFilterMonitor.Sorted = true;
            this.lbFilterMonitor.TabIndex = 16;
            // 
            // tbMonitor
            // 
            this.tbMonitor.Location = new System.Drawing.Point(2, 9);
            this.tbMonitor.Name = "tbMonitor";
            this.tbMonitor.Size = new System.Drawing.Size(203, 20);
            this.tbMonitor.TabIndex = 15;
            this.tbMonitor.TextChanged += new System.EventHandler(this.tbMonitor_TextChanged);
            // 
            // tbGCIDPage
            // 
            this.tbGCIDPage.Controls.Add(this.tbFilterGCID);
            this.tbGCIDPage.Controls.Add(this.btnDelAllGCID);
            this.tbGCIDPage.Controls.Add(this.btnDelGCID);
            this.tbGCIDPage.Controls.Add(this.btnAddAllGCID);
            this.tbGCIDPage.Controls.Add(this.btnAddGCID);
            this.tbGCIDPage.Controls.Add(this.lbGCID);
            this.tbGCIDPage.Controls.Add(this.lbFilterGCID);
            this.tbGCIDPage.Controls.Add(this.tbGCID);
            this.tbGCIDPage.Location = new System.Drawing.Point(4, 22);
            this.tbGCIDPage.Name = "tbGCIDPage";
            this.tbGCIDPage.Size = new System.Drawing.Size(504, 462);
            this.tbGCIDPage.TabIndex = 2;
            this.tbGCIDPage.Text = "GCID";
            this.tbGCIDPage.UseVisualStyleBackColor = true;
            // 
            // tbFilterGCID
            // 
            this.tbFilterGCID.Location = new System.Drawing.Point(249, 9);
            this.tbFilterGCID.Name = "tbFilterGCID";
            this.tbFilterGCID.Size = new System.Drawing.Size(203, 20);
            this.tbFilterGCID.TabIndex = 29;
            this.tbFilterGCID.TextChanged += new System.EventHandler(this.tbFilterGCID_TextChanged);
            // 
            // btnDelAllGCID
            // 
            this.btnDelAllGCID.Location = new System.Drawing.Point(211, 255);
            this.btnDelAllGCID.Name = "btnDelAllGCID";
            this.btnDelAllGCID.Size = new System.Drawing.Size(32, 32);
            this.btnDelAllGCID.TabIndex = 28;
            this.btnDelAllGCID.Text = "<<";
            this.btnDelAllGCID.UseVisualStyleBackColor = true;
            this.btnDelAllGCID.Click += new System.EventHandler(this.btnDelAllGCID_Click);
            // 
            // btnDelGCID
            // 
            this.btnDelGCID.Location = new System.Drawing.Point(211, 217);
            this.btnDelGCID.Name = "btnDelGCID";
            this.btnDelGCID.Size = new System.Drawing.Size(32, 32);
            this.btnDelGCID.TabIndex = 27;
            this.btnDelGCID.Text = "<";
            this.btnDelGCID.UseVisualStyleBackColor = true;
            this.btnDelGCID.Click += new System.EventHandler(this.btnDelGCID_Click);
            // 
            // btnAddAllGCID
            // 
            this.btnAddAllGCID.Location = new System.Drawing.Point(211, 134);
            this.btnAddAllGCID.Name = "btnAddAllGCID";
            this.btnAddAllGCID.Size = new System.Drawing.Size(32, 32);
            this.btnAddAllGCID.TabIndex = 26;
            this.btnAddAllGCID.Text = ">>";
            this.btnAddAllGCID.UseVisualStyleBackColor = true;
            this.btnAddAllGCID.Click += new System.EventHandler(this.btnAddAllGCID_Click);
            // 
            // btnAddGCID
            // 
            this.btnAddGCID.Location = new System.Drawing.Point(211, 96);
            this.btnAddGCID.Name = "btnAddGCID";
            this.btnAddGCID.Size = new System.Drawing.Size(32, 32);
            this.btnAddGCID.TabIndex = 25;
            this.btnAddGCID.Text = ">";
            this.btnAddGCID.UseVisualStyleBackColor = true;
            this.btnAddGCID.Click += new System.EventHandler(this.btnAddGCID_Click);
            // 
            // lbGCID
            // 
            this.lbGCID.FormattingEnabled = true;
            this.lbGCID.Location = new System.Drawing.Point(2, 35);
            this.lbGCID.Name = "lbGCID";
            this.lbGCID.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbGCID.Size = new System.Drawing.Size(203, 342);
            this.lbGCID.TabIndex = 24;
            // 
            // lbFilterGCID
            // 
            this.lbFilterGCID.FormattingEnabled = true;
            this.lbFilterGCID.Location = new System.Drawing.Point(249, 35);
            this.lbFilterGCID.Name = "lbFilterGCID";
            this.lbFilterGCID.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbFilterGCID.Size = new System.Drawing.Size(203, 342);
            this.lbFilterGCID.Sorted = true;
            this.lbFilterGCID.TabIndex = 23;
            // 
            // tbGCID
            // 
            this.tbGCID.Location = new System.Drawing.Point(2, 9);
            this.tbGCID.Name = "tbGCID";
            this.tbGCID.Size = new System.Drawing.Size(203, 20);
            this.tbGCID.TabIndex = 22;
            this.tbGCID.TextChanged += new System.EventHandler(this.tbGCID_TextChanged);
            // 
            // btnLogStart
            // 
            this.btnLogStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogStart.Location = new System.Drawing.Point(333, 13);
            this.btnLogStart.Name = "btnLogStart";
            this.btnLogStart.Size = new System.Drawing.Size(60, 22);
            this.btnLogStart.TabIndex = 1;
            this.btnLogStart.Text = "Start";
            this.btnLogStart.UseVisualStyleBackColor = true;
            this.btnLogStart.Click += new System.EventHandler(this.btnLogStart_Click);
            // 
            // treeLog
            // 
            this.treeLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeLog.Location = new System.Drawing.Point(10, 10);
            this.treeLog.Name = "treeLog";
            this.treeLog.Size = new System.Drawing.Size(640, 564);
            this.treeLog.TabIndex = 4;
            this.treeLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeLog_KeyDown);
            this.treeLog.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeLog_MouseDown);
            // 
            // tbGeneralPage
            // 
            this.tbGeneralPage.Controls.Add(this.comboBox1);
            this.tbGeneralPage.Controls.Add(this.groupBox1);
            this.tbGeneralPage.Controls.Add(this.button1);
            this.tbGeneralPage.Location = new System.Drawing.Point(4, 22);
            this.tbGeneralPage.Name = "tbGeneralPage";
            this.tbGeneralPage.Padding = new System.Windows.Forms.Padding(10);
            this.tbGeneralPage.Size = new System.Drawing.Size(1204, 584);
            this.tbGeneralPage.TabIndex = 0;
            this.tbGeneralPage.Text = "General";
            this.tbGeneralPage.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DisplayMember = "(none)";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "6.0.1.1",
            "6.0.1.1_1",
            "6.0.1.1_2",
            "6.0.1.1_3",
            "6.0.1.1_4",
            "6.0.1.1_5",
            "6.0.1.1_6",
            "6.0.1.1_7",
            "6.0.1.1_8",
            "6.1.0.0"});
            this.comboBox1.Location = new System.Drawing.Point(262, 25);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(153, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(195, 150);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Versions";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 112);
            this.label1.TabIndex = 1;
            this.label1.Text = "DataManager: x.xx.xxxx\r\n2\r\n3\r\n4\r\n5\r\n6\r\n7\r\n";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(423, 318);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fIleToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(10, 10);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1212, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fIleToolStripMenuItem
            // 
            this.fIleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openLogFileToolStripMenuItem});
            this.fIleToolStripMenuItem.Name = "fIleToolStripMenuItem";
            this.fIleToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fIleToolStripMenuItem.Text = "FIle";
            // 
            // openLogFileToolStripMenuItem
            // 
            this.openLogFileToolStripMenuItem.Name = "openLogFileToolStripMenuItem";
            this.openLogFileToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.openLogFileToolStripMenuItem.Text = "Open log file...";
            this.openLogFileToolStripMenuItem.Click += new System.EventHandler(this.openLogFileToolStripMenuItem_Click);
            // 
            // frmCSMDH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 654);
            this.Controls.Add(this.tcCSMDH);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmCSMDH";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "CSM Development Helper";
            this.tcCSMDH.ResumeLayout(false);
            this.tbLogPage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ssEvents.ResumeLayout(false);
            this.ssEvents.PerformLayout();
            this.tcLogFilters.ResumeLayout(false);
            this.tbEventPage.ResumeLayout(false);
            this.tbEventPage.PerformLayout();
            this.tbMonitorPage.ResumeLayout(false);
            this.tbMonitorPage.PerformLayout();
            this.tbGCIDPage.ResumeLayout(false);
            this.tbGCIDPage.PerformLayout();
            this.tbGeneralPage.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tcCSMDH;
        private System.Windows.Forms.TabPage tbGeneralPage;
        private System.Windows.Forms.TabPage tbLogPage;
        private System.Windows.Forms.TreeView treeLog;

        private System.Windows.Forms.StatusStrip ssEvents;
        private System.Windows.Forms.ToolStripStatusLabel tsslEvents;
        private RegistryHandler registryHandler;

        private string log_filename;
        private const string default_log_filename = @"C:\ProgramData\Mitel\Customer Service Manager\Server\Logs\TelDrv.log";
        private TreeNode rootNode;
        private Dictionary<string, HashSet<string>> dictGCID;
        private CustomBindingList<string> listMonitor;
        private CustomBindingList<string> listGCID;
        private CustomBindingList<string> listEvent;
        private CustomBindingList<string> listFilterMonitor;
        private CustomBindingList<string> listFilterGCID;
        private CustomBindingList<string> listFilterEvent;
        private CustomBindingList<TreeNode> listNode;
        private CustomBindingList<TreeNode> listFilterNode;
        private Form ModelingForm;
        private Panel panel1;
        private Button btnLogStop;
        private Button btnLogStart;
        private TabControl tcLogFilters;
        private TabPage tbEventPage;
        private TextBox tbFilterEvent;
        private Button btnDelAllEvent;
        private Button btnDelEvent;
        private Button btnAddAllEvent;
        private Button btnAddEvent;
        private ListBox lbEvent;
        private ListBox lbFilterEvent;
        private TextBox tbEvent;
        private TabPage tbMonitorPage;
        private TextBox tbFilterMonitor;
        private Button btnDelAllMonitor;
        private Button btnDelMonitor;
        private Button btnAddAllMonitor;
        private Button btnAddMonitor;
        private ListBox lbMonitor;
        private ListBox lbFilterMonitor;
        private TextBox tbMonitor;
        private TabPage tbGCIDPage;
        private TextBox tbFilterGCID;
        private Button btnDelAllGCID;
        private Button btnDelGCID;
        private Button btnAddAllGCID;
        private Button btnAddGCID;
        private ListBox lbGCID;
        private ListBox lbFilterGCID;
        private TextBox tbGCID;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fIleToolStripMenuItem;
        private ToolStripMenuItem openLogFileToolStripMenuItem;
        private CheckBox chkTailing;
        private Panel panel2;
        private RadioButton rbtnAuto;
        private RadioButton rbtnCP;
        private RadioButton rbtnMCD;
        private Button button1;
        private Label label1;
        private GroupBox groupBox1;
        private ComboBox comboBox1;
        private Button btnLogPause;
    }
}

