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
        private LogUpdateDelegate myDelegate;

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
            this.ssEvents = new System.Windows.Forms.StatusStrip();
            this.tsslEvents = new System.Windows.Forms.ToolStripStatusLabel();
            this.tcLogFilters = new System.Windows.Forms.TabControl();
            this.tbEventFilterPage = new System.Windows.Forms.TabPage();
            this.btnDelAllEvent = new System.Windows.Forms.Button();
            this.btnDelEvent = new System.Windows.Forms.Button();
            this.btnAddAllEvent = new System.Windows.Forms.Button();
            this.btnAddEvent = new System.Windows.Forms.Button();
            this.lbEvent = new System.Windows.Forms.ListBox();
            this.lbFilterEvent = new System.Windows.Forms.ListBox();
            this.tbEvent = new System.Windows.Forms.TextBox();
            this.tbMonitorFilterPage = new System.Windows.Forms.TabPage();
            this.btnDelAllMonitor = new System.Windows.Forms.Button();
            this.btnDelMonitor = new System.Windows.Forms.Button();
            this.btnAddAllMonitor = new System.Windows.Forms.Button();
            this.btnAddMonitor = new System.Windows.Forms.Button();
            this.lbMonitor = new System.Windows.Forms.ListBox();
            this.lbFilterMonitor = new System.Windows.Forms.ListBox();
            this.tbMonitor = new System.Windows.Forms.TextBox();
            this.tbGCIDPage = new System.Windows.Forms.TabPage();
            this.btnDelAllGCID = new System.Windows.Forms.Button();
            this.btnDelGCID = new System.Windows.Forms.Button();
            this.btnAddAllGCID = new System.Windows.Forms.Button();
            this.btnAddGCID = new System.Windows.Forms.Button();
            this.lbGCID = new System.Windows.Forms.ListBox();
            this.lbFilterGCID = new System.Windows.Forms.ListBox();
            this.tbGCID = new System.Windows.Forms.TextBox();
            this.btnLogPause = new System.Windows.Forms.Button();
            this.treeLog = new System.Windows.Forms.TreeView();
            this.btnLogStop = new System.Windows.Forms.Button();
            this.btnLogStart = new System.Windows.Forms.Button();
            this.tbGeneralPage = new System.Windows.Forms.TabPage();
            this.tcCSMDH.SuspendLayout();
            this.tbLogPage.SuspendLayout();
            this.ssEvents.SuspendLayout();
            this.tcLogFilters.SuspendLayout();
            this.tbEventFilterPage.SuspendLayout();
            this.tbMonitorFilterPage.SuspendLayout();
            this.tbGCIDPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcCSMDH
            // 
            this.tcCSMDH.Controls.Add(this.tbLogPage);
            this.tcCSMDH.Controls.Add(this.tbGeneralPage);
            this.tcCSMDH.Location = new System.Drawing.Point(13, 13);
            this.tcCSMDH.Name = "tcCSMDH";
            this.tcCSMDH.SelectedIndex = 0;
            this.tcCSMDH.Size = new System.Drawing.Size(1141, 502);
            this.tcCSMDH.TabIndex = 0;
            // 
            // tbLogPage
            // 
            this.tbLogPage.Controls.Add(this.ssEvents);
            this.tbLogPage.Controls.Add(this.tcLogFilters);
            this.tbLogPage.Controls.Add(this.btnLogPause);
            this.tbLogPage.Controls.Add(this.treeLog);
            this.tbLogPage.Controls.Add(this.btnLogStop);
            this.tbLogPage.Controls.Add(this.btnLogStart);
            this.tbLogPage.Location = new System.Drawing.Point(4, 22);
            this.tbLogPage.Name = "tbLogPage";
            this.tbLogPage.Padding = new System.Windows.Forms.Padding(3);
            this.tbLogPage.Size = new System.Drawing.Size(1133, 476);
            this.tbLogPage.TabIndex = 1;
            this.tbLogPage.Text = "Logging";
            this.tbLogPage.UseVisualStyleBackColor = true;
            // 
            // ssEvents
            // 
            this.ssEvents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslEvents});
            this.ssEvents.Location = new System.Drawing.Point(3, 451);
            this.ssEvents.Name = "ssEvents";
            this.ssEvents.Size = new System.Drawing.Size(1127, 22);
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
            this.tcLogFilters.Controls.Add(this.tbEventFilterPage);
            this.tcLogFilters.Controls.Add(this.tbMonitorFilterPage);
            this.tcLogFilters.Controls.Add(this.tbGCIDPage);
            this.tcLogFilters.Location = new System.Drawing.Point(665, 35);
            this.tcLogFilters.Name = "tcLogFilters";
            this.tcLogFilters.SelectedIndex = 0;
            this.tcLogFilters.Size = new System.Drawing.Size(462, 413);
            this.tcLogFilters.TabIndex = 7;
            // 
            // tbEventFilterPage
            // 
            this.tbEventFilterPage.Controls.Add(this.btnDelAllEvent);
            this.tbEventFilterPage.Controls.Add(this.btnDelEvent);
            this.tbEventFilterPage.Controls.Add(this.btnAddAllEvent);
            this.tbEventFilterPage.Controls.Add(this.btnAddEvent);
            this.tbEventFilterPage.Controls.Add(this.lbEvent);
            this.tbEventFilterPage.Controls.Add(this.lbFilterEvent);
            this.tbEventFilterPage.Controls.Add(this.tbEvent);
            this.tbEventFilterPage.Location = new System.Drawing.Point(4, 22);
            this.tbEventFilterPage.Name = "tbEventFilterPage";
            this.tbEventFilterPage.Padding = new System.Windows.Forms.Padding(3);
            this.tbEventFilterPage.Size = new System.Drawing.Size(454, 387);
            this.tbEventFilterPage.TabIndex = 0;
            this.tbEventFilterPage.Text = "Event";
            this.tbEventFilterPage.UseVisualStyleBackColor = true;
            // 
            // btnDelAllEvent
            // 
            this.btnDelAllEvent.Location = new System.Drawing.Point(211, 255);
            this.btnDelAllEvent.Name = "btnDelAllEvent";
            this.btnDelAllEvent.Size = new System.Drawing.Size(32, 32);
            this.btnDelAllEvent.TabIndex = 21;
            this.btnDelAllEvent.Text = "<<";
            this.btnDelAllEvent.UseVisualStyleBackColor = true;
            this.btnDelAllEvent.Click += new System.EventHandler(this.btnDelAllEvent_Click);
            // 
            // btnDelEvent
            // 
            this.btnDelEvent.Location = new System.Drawing.Point(211, 217);
            this.btnDelEvent.Name = "btnDelEvent";
            this.btnDelEvent.Size = new System.Drawing.Size(32, 32);
            this.btnDelEvent.TabIndex = 20;
            this.btnDelEvent.Text = "<";
            this.btnDelEvent.UseVisualStyleBackColor = true;
            this.btnDelEvent.Click += new System.EventHandler(this.btnDelEvent_Click);
            // 
            // btnAddAllEvent
            // 
            this.btnAddAllEvent.Location = new System.Drawing.Point(211, 134);
            this.btnAddAllEvent.Name = "btnAddAllEvent";
            this.btnAddAllEvent.Size = new System.Drawing.Size(32, 32);
            this.btnAddAllEvent.TabIndex = 19;
            this.btnAddAllEvent.Text = ">>";
            this.btnAddAllEvent.UseVisualStyleBackColor = true;
            this.btnAddAllEvent.Click += new System.EventHandler(this.btnAddAllEvent_Click);
            // 
            // btnAddEvent
            // 
            this.btnAddEvent.Location = new System.Drawing.Point(211, 96);
            this.btnAddEvent.Name = "btnAddEvent";
            this.btnAddEvent.Size = new System.Drawing.Size(32, 32);
            this.btnAddEvent.TabIndex = 18;
            this.btnAddEvent.Text = ">";
            this.btnAddEvent.UseVisualStyleBackColor = true;
            this.btnAddEvent.Click += new System.EventHandler(this.btnAddEvent_Click);
            // 
            // lbEvent
            // 
            this.lbEvent.FormattingEnabled = true;
            this.lbEvent.Location = new System.Drawing.Point(2, 35);
            this.lbEvent.Name = "lbEvent";
            this.lbEvent.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbEvent.Size = new System.Drawing.Size(203, 342);
            this.lbEvent.TabIndex = 17;
            // 
            // lbFilterEvent
            // 
            this.lbFilterEvent.FormattingEnabled = true;
            this.lbFilterEvent.Location = new System.Drawing.Point(249, 35);
            this.lbFilterEvent.Name = "lbFilterEvent";
            this.lbFilterEvent.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbFilterEvent.Size = new System.Drawing.Size(203, 342);
            this.lbFilterEvent.TabIndex = 16;
            // 
            // tbEvent
            // 
            this.tbEvent.Location = new System.Drawing.Point(2, 9);
            this.tbEvent.Name = "tbEvent";
            this.tbEvent.Size = new System.Drawing.Size(203, 20);
            this.tbEvent.TabIndex = 15;
            this.tbEvent.TextChanged += new System.EventHandler(this.tbEvent_TextChanged);
            // 
            // tbMonitorFilterPage
            // 
            this.tbMonitorFilterPage.Controls.Add(this.btnDelAllMonitor);
            this.tbMonitorFilterPage.Controls.Add(this.btnDelMonitor);
            this.tbMonitorFilterPage.Controls.Add(this.btnAddAllMonitor);
            this.tbMonitorFilterPage.Controls.Add(this.btnAddMonitor);
            this.tbMonitorFilterPage.Controls.Add(this.lbMonitor);
            this.tbMonitorFilterPage.Controls.Add(this.lbFilterMonitor);
            this.tbMonitorFilterPage.Controls.Add(this.tbMonitor);
            this.tbMonitorFilterPage.Location = new System.Drawing.Point(4, 22);
            this.tbMonitorFilterPage.Name = "tbMonitorFilterPage";
            this.tbMonitorFilterPage.Padding = new System.Windows.Forms.Padding(3);
            this.tbMonitorFilterPage.Size = new System.Drawing.Size(454, 387);
            this.tbMonitorFilterPage.TabIndex = 1;
            this.tbMonitorFilterPage.Text = "Monitor";
            this.tbMonitorFilterPage.UseVisualStyleBackColor = true;
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
            this.tbGCIDPage.Controls.Add(this.btnDelAllGCID);
            this.tbGCIDPage.Controls.Add(this.btnDelGCID);
            this.tbGCIDPage.Controls.Add(this.btnAddAllGCID);
            this.tbGCIDPage.Controls.Add(this.btnAddGCID);
            this.tbGCIDPage.Controls.Add(this.lbGCID);
            this.tbGCIDPage.Controls.Add(this.lbFilterGCID);
            this.tbGCIDPage.Controls.Add(this.tbGCID);
            this.tbGCIDPage.Location = new System.Drawing.Point(4, 22);
            this.tbGCIDPage.Name = "tbGCIDPage";
            this.tbGCIDPage.Size = new System.Drawing.Size(454, 387);
            this.tbGCIDPage.TabIndex = 2;
            this.tbGCIDPage.Text = "GCID";
            this.tbGCIDPage.UseVisualStyleBackColor = true;
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
            // btnLogPause
            // 
            this.btnLogPause.Enabled = false;
            this.btnLogPause.Location = new System.Drawing.Point(1043, 6);
            this.btnLogPause.Name = "btnLogPause";
            this.btnLogPause.Size = new System.Drawing.Size(84, 22);
            this.btnLogPause.TabIndex = 6;
            this.btnLogPause.Text = "Pause";
            this.btnLogPause.UseVisualStyleBackColor = true;
            this.btnLogPause.Click += new System.EventHandler(this.btnLogPause_Click);
            // 
            // treeLog
            // 
            this.treeLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeLog.Location = new System.Drawing.Point(3, 6);
            this.treeLog.Name = "treeLog";
            this.treeLog.ShowNodeToolTips = true;
            this.treeLog.ShowRootLines = false;
            this.treeLog.Size = new System.Drawing.Size(646, 442);
            this.treeLog.TabIndex = 4;
            // 
            // btnLogStop
            // 
            this.btnLogStop.Enabled = false;
            this.btnLogStop.Location = new System.Drawing.Point(949, 7);
            this.btnLogStop.Name = "btnLogStop";
            this.btnLogStop.Size = new System.Drawing.Size(88, 22);
            this.btnLogStop.TabIndex = 3;
            this.btnLogStop.Text = "Stop";
            this.btnLogStop.UseVisualStyleBackColor = true;
            this.btnLogStop.Click += new System.EventHandler(this.btnLogStop_Click);
            // 
            // btnLogStart
            // 
            this.btnLogStart.Location = new System.Drawing.Point(853, 6);
            this.btnLogStart.Name = "btnLogStart";
            this.btnLogStart.Size = new System.Drawing.Size(90, 22);
            this.btnLogStart.TabIndex = 1;
            this.btnLogStart.Text = "Start";
            this.btnLogStart.UseVisualStyleBackColor = true;
            this.btnLogStart.Click += new System.EventHandler(this.btnLogStart_Click);
            // 
            // tbGeneralPage
            // 
            this.tbGeneralPage.Location = new System.Drawing.Point(4, 22);
            this.tbGeneralPage.Name = "tbGeneralPage";
            this.tbGeneralPage.Padding = new System.Windows.Forms.Padding(3);
            this.tbGeneralPage.Size = new System.Drawing.Size(1133, 476);
            this.tbGeneralPage.TabIndex = 0;
            this.tbGeneralPage.Text = "General";
            this.tbGeneralPage.UseVisualStyleBackColor = true;
            // 
            // frmCSMDH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1166, 527);
            this.Controls.Add(this.tcCSMDH);
            this.Name = "frmCSMDH";
            this.Text = "CSM Development Helper";
            this.tcCSMDH.ResumeLayout(false);
            this.tbLogPage.ResumeLayout(false);
            this.tbLogPage.PerformLayout();
            this.ssEvents.ResumeLayout(false);
            this.ssEvents.PerformLayout();
            this.tcLogFilters.ResumeLayout(false);
            this.tbEventFilterPage.ResumeLayout(false);
            this.tbEventFilterPage.PerformLayout();
            this.tbMonitorFilterPage.ResumeLayout(false);
            this.tbMonitorFilterPage.PerformLayout();
            this.tbGCIDPage.ResumeLayout(false);
            this.tbGCIDPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcCSMDH;
        private System.Windows.Forms.TabPage tbGeneralPage;
        private System.Windows.Forms.TabPage tbLogPage;
        private System.Windows.Forms.Button btnLogStart;
        private System.Windows.Forms.Button btnLogStop;
        private System.Windows.Forms.TreeView treeLog;

        private System.Threading.Thread logThread;
        private System.Windows.Forms.Button btnLogPause;
        private System.Windows.Forms.TabControl tcLogFilters;
        private System.Windows.Forms.TabPage tbEventFilterPage;
        private System.Windows.Forms.TabPage tbMonitorFilterPage;
        private System.Windows.Forms.TabPage tbGCIDPage;
        private System.Windows.Forms.StatusStrip ssEvents;
        private System.Windows.Forms.ToolStripStatusLabel tsslEvents;

        private bool isLogUpdate;
        private EventNode rootNode;
        private Dictionary<string, HashSet<string>> dictGCID;
        private CustomBindingList<string> listMonitor;
        private CustomBindingList<string> listGCID;
        private CustomBindingList<string> listEvent;
        private CustomBindingList<string> listFilterMonitor;
        private CustomBindingList<string> listFilterGCID;
        private CustomBindingList<string> listFilterEvent;
        private CustomBindingList<EventNode> listNode;
        private CustomBindingList<EventNode> listFilterNode;

        private Button btnDelAllEvent;
        private Button btnDelEvent;
        private Button btnAddAllEvent;
        private Button btnAddEvent;
        private ListBox lbEvent;
        private ListBox lbFilterEvent;
        private TextBox tbEvent;
        private Button btnDelAllMonitor;
        private Button btnDelMonitor;
        private Button btnAddAllMonitor;
        private Button btnAddMonitor;
        private ListBox lbMonitor;
        private ListBox lbFilterMonitor;
        private TextBox tbMonitor;
        private Button btnDelAllGCID;
        private Button btnDelGCID;
        private Button btnAddAllGCID;
        private Button btnAddGCID;
        private ListBox lbGCID;
        private ListBox lbFilterGCID;
        private TextBox tbGCID;
    }
}

