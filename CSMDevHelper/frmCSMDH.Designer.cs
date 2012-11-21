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
            this.tbGeneralPage = new System.Windows.Forms.TabPage();
            this.tbLogPage = new System.Windows.Forms.TabPage();
            this.treeLog = new System.Windows.Forms.TreeView();
            this.btnLogStop = new System.Windows.Forms.Button();
            this.btnLogStart = new System.Windows.Forms.Button();
            this.btnLogRestart = new System.Windows.Forms.Button();
            this.tcCSMDH.SuspendLayout();
            this.tbLogPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcCSMDH
            // 
            this.tcCSMDH.Controls.Add(this.tbGeneralPage);
            this.tcCSMDH.Controls.Add(this.tbLogPage);
            this.tcCSMDH.Location = new System.Drawing.Point(13, 13);
            this.tcCSMDH.Name = "tcCSMDH";
            this.tcCSMDH.SelectedIndex = 0;
            this.tcCSMDH.Size = new System.Drawing.Size(535, 322);
            this.tcCSMDH.TabIndex = 0;
            // 
            // tbGeneralPage
            // 
            this.tbGeneralPage.Location = new System.Drawing.Point(4, 22);
            this.tbGeneralPage.Name = "tbGeneralPage";
            this.tbGeneralPage.Padding = new System.Windows.Forms.Padding(3);
            this.tbGeneralPage.Size = new System.Drawing.Size(527, 296);
            this.tbGeneralPage.TabIndex = 0;
            this.tbGeneralPage.Text = "General";
            this.tbGeneralPage.UseVisualStyleBackColor = true;
            // 
            // tbLogPage
            // 
            this.tbLogPage.Controls.Add(this.treeLog);
            this.tbLogPage.Controls.Add(this.btnLogStop);
            this.tbLogPage.Controls.Add(this.btnLogStart);
            this.tbLogPage.Controls.Add(this.btnLogRestart);
            this.tbLogPage.Location = new System.Drawing.Point(4, 22);
            this.tbLogPage.Name = "tbLogPage";
            this.tbLogPage.Padding = new System.Windows.Forms.Padding(3);
            this.tbLogPage.Size = new System.Drawing.Size(527, 296);
            this.tbLogPage.TabIndex = 1;
            this.tbLogPage.Text = "Logging";
            this.tbLogPage.UseVisualStyleBackColor = true;
            // 
            // treeLog
            // 
            this.treeLog.Location = new System.Drawing.Point(7, 6);
            this.treeLog.Name = "treeLog";
            this.treeLog.Size = new System.Drawing.Size(424, 284);
            this.treeLog.TabIndex = 4;
            // 
            // btnLogStop
            // 
            this.btnLogStop.Location = new System.Drawing.Point(437, 62);
            this.btnLogStop.Name = "btnLogStop";
            this.btnLogStop.Size = new System.Drawing.Size(84, 22);
            this.btnLogStop.TabIndex = 3;
            this.btnLogStop.Text = "Stop";
            this.btnLogStop.UseVisualStyleBackColor = true;
            this.btnLogStop.Click += new System.EventHandler(this.btnLogStop_Click);
            // 
            // btnLogStart
            // 
            this.btnLogStart.Location = new System.Drawing.Point(437, 6);
            this.btnLogStart.Name = "btnLogStart";
            this.btnLogStart.Size = new System.Drawing.Size(84, 22);
            this.btnLogStart.TabIndex = 1;
            this.btnLogStart.Text = "Start";
            this.btnLogStart.UseVisualStyleBackColor = true;
            this.btnLogStart.Click += new System.EventHandler(this.btnLogStart_Click);
            // 
            // btnLogRestart
            // 
            this.btnLogRestart.Location = new System.Drawing.Point(437, 34);
            this.btnLogRestart.Name = "btnLogRestart";
            this.btnLogRestart.Size = new System.Drawing.Size(84, 22);
            this.btnLogRestart.TabIndex = 2;
            this.btnLogRestart.Text = "Restart";
            this.btnLogRestart.UseVisualStyleBackColor = true;
            this.btnLogRestart.Click += new System.EventHandler(this.btnLogRestart_Click);
            // 
            // frmCSMDH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 347);
            this.Controls.Add(this.tcCSMDH);
            this.Name = "frmCSMDH";
            this.Text = "CSM Development Helper";
            this.tcCSMDH.ResumeLayout(false);
            this.tbLogPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcCSMDH;
        private System.Windows.Forms.TabPage tbGeneralPage;
        private System.Windows.Forms.TabPage tbLogPage;
        private System.Windows.Forms.Button btnLogStart;
        private System.Windows.Forms.Button btnLogRestart;
        private System.Windows.Forms.Button btnLogStop;
        private System.Windows.Forms.TreeView treeLog;

        private System.Windows.Forms.TreeNodeCollection treeLogCollection;
        private bool isLogUpdate;
        private System.Threading.Thread logThread;
    }
}

