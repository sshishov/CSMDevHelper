using System.Collections.Generic;

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
            this.cblstEvents = new System.Windows.Forms.CheckedListBox();
            this.tbMonitorFilterPage = new System.Windows.Forms.TabPage();
            this.cblstMonitors = new System.Windows.Forms.CheckedListBox();
            this.tbGCIDPage = new System.Windows.Forms.TabPage();
            this.cblstGCID = new System.Windows.Forms.CheckedListBox();
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
            this.tcLogFilters.Location = new System.Drawing.Point(853, 35);
            this.tcLogFilters.Name = "tcLogFilters";
            this.tcLogFilters.SelectedIndex = 0;
            this.tcLogFilters.Size = new System.Drawing.Size(274, 413);
            this.tcLogFilters.TabIndex = 7;
            // 
            // tbEventFilterPage
            // 
            this.tbEventFilterPage.Controls.Add(this.cblstEvents);
            this.tbEventFilterPage.Location = new System.Drawing.Point(4, 22);
            this.tbEventFilterPage.Name = "tbEventFilterPage";
            this.tbEventFilterPage.Padding = new System.Windows.Forms.Padding(3);
            this.tbEventFilterPage.Size = new System.Drawing.Size(266, 409);
            this.tbEventFilterPage.TabIndex = 0;
            this.tbEventFilterPage.Text = "Events";
            this.tbEventFilterPage.UseVisualStyleBackColor = true;
            // 
            // cblstEvents
            // 
            this.cblstEvents.CheckOnClick = true;
            this.cblstEvents.FormattingEnabled = true;
            this.cblstEvents.Location = new System.Drawing.Point(6, 6);
            this.cblstEvents.Name = "cblstEvents";
            this.cblstEvents.Size = new System.Drawing.Size(254, 379);
            this.cblstEvents.Sorted = true;
            this.cblstEvents.TabIndex = 5;
            this.cblstEvents.SelectedIndexChanged += new System.EventHandler(this.cblstEvents_SelectedIndexChanged);
            // 
            // tbMonitorFilterPage
            // 
            this.tbMonitorFilterPage.Controls.Add(this.cblstMonitors);
            this.tbMonitorFilterPage.Location = new System.Drawing.Point(4, 22);
            this.tbMonitorFilterPage.Name = "tbMonitorFilterPage";
            this.tbMonitorFilterPage.Padding = new System.Windows.Forms.Padding(3);
            this.tbMonitorFilterPage.Size = new System.Drawing.Size(266, 409);
            this.tbMonitorFilterPage.TabIndex = 1;
            this.tbMonitorFilterPage.Text = "Monitors";
            this.tbMonitorFilterPage.UseVisualStyleBackColor = true;
            // 
            // cblstMonitors
            // 
            this.cblstMonitors.CheckOnClick = true;
            this.cblstMonitors.FormattingEnabled = true;
            this.cblstMonitors.Location = new System.Drawing.Point(6, 7);
            this.cblstMonitors.Name = "cblstMonitors";
            this.cblstMonitors.Size = new System.Drawing.Size(254, 379);
            this.cblstMonitors.Sorted = true;
            this.cblstMonitors.TabIndex = 6;
            // 
            // tbGCIDPage
            // 
            this.tbGCIDPage.Controls.Add(this.cblstGCID);
            this.tbGCIDPage.Location = new System.Drawing.Point(4, 22);
            this.tbGCIDPage.Name = "tbGCIDPage";
            this.tbGCIDPage.Size = new System.Drawing.Size(266, 387);
            this.tbGCIDPage.TabIndex = 2;
            this.tbGCIDPage.Text = "GCID";
            this.tbGCIDPage.UseVisualStyleBackColor = true;
            // 
            // cblstGCID
            // 
            this.cblstGCID.CheckOnClick = true;
            this.cblstGCID.FormattingEnabled = true;
            this.cblstGCID.Location = new System.Drawing.Point(3, 7);
            this.cblstGCID.Name = "cblstGCID";
            this.cblstGCID.Size = new System.Drawing.Size(257, 379);
            this.cblstGCID.Sorted = true;
            this.cblstGCID.TabIndex = 7;
            this.cblstGCID.SelectedIndexChanged += new System.EventHandler(this.cblstGCID_SelectedIndexChanged);
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
            this.treeLog.Size = new System.Drawing.Size(844, 442);
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
            this.tbGeneralPage.Size = new System.Drawing.Size(816, 476);
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
            this.tbMonitorFilterPage.ResumeLayout(false);
            this.tbGCIDPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcCSMDH;
        private System.Windows.Forms.TabPage tbGeneralPage;
        private System.Windows.Forms.TabPage tbLogPage;
        private System.Windows.Forms.Button btnLogStart;
        private System.Windows.Forms.Button btnLogStop;
        private System.Windows.Forms.TreeView treeLog;

        private bool isLogUpdate;
        private EventNode rootNode;
        private System.Threading.Thread logThread;
        private System.Windows.Forms.CheckedListBox cblstEvents;
        private System.Windows.Forms.Button btnLogPause;
        private System.Windows.Forms.TabControl tcLogFilters;
        private System.Windows.Forms.TabPage tbEventFilterPage;
        private System.Windows.Forms.TabPage tbMonitorFilterPage;
        private System.Windows.Forms.CheckedListBox cblstMonitors;
        private System.Windows.Forms.TabPage tbGCIDPage;
        private System.Windows.Forms.CheckedListBox cblstGCID;
        private Dictionary<string, HashSet<string>> dictGCID;
        private System.Windows.Forms.StatusStrip ssEvents;
        private System.Windows.Forms.ToolStripStatusLabel tsslEvents;
    }
}

