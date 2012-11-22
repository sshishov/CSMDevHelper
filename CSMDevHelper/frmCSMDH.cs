using System;
using System.Collections;
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
                logThread.Suspend();
            }
            else
            {
                btnLogPause.Text = "Pause";
                logThread.Resume();
            }
        }

        void ThreadLogUpdate()
        {
            string update = "";
            LogReader logReader = new LogReader(@"C:\DEV_logs1.txt");
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

        void LogUpdate(string str)
        {
            TreeNode rootNode = new TreeNode();
            object eventObject;
            string eventType;
            if (isLogUpdate)
            {
                JavaScriptSerializer mySer = new JavaScriptSerializer();
                try
                {
                    Dictionary<string, object> dict = mySer.Deserialize<Dictionary<string, object>>(str);
                    if (dict.TryGetValue("MitaiEvent", out eventObject))
                    {
                        dict = (Dictionary<string, object>)eventObject;
                    }
                    else
                    {
                        return;
                    }
                    if (dict.TryGetValue("Type", out eventObject))
                    {
                        eventType = (string)eventObject;
                        dict.Remove("Type");
                    }
                    else
                    {
                        eventType = "Unknown event type";
                    }
                    if (!cblstEvents.Items.Contains(eventType))
                    {
                        cblstEvents.Items.Add(eventType, true);
                    }
                    rootNode.Text = eventType;
                    rootNode.Nodes.Add(generate_tree(dict));
                    treeLog.Nodes.Add(rootNode);
                    treeLogCollection.Add(rootNode);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        TreeNode generate_tree(Dictionary<string, object> dict)
        {
            TreeNode currentNode = new TreeNode();
            string strNode = "";

            foreach(string dictKey in dict.Keys){
                object dictValue = dict[dictKey];
                if ((object)dictValue is Dictionary<string, object>)
                {
                    strNode = dictKey;
                    currentNode.Nodes.Add(generate_tree((Dictionary<string, object>)dictValue));
                }
                else if ((Object)dictValue is ArrayList)
                {
                    strNode = dictKey;
                    foreach(Dictionary<string, object> node in (ArrayList)dictValue)
                    {
                        currentNode.Nodes.Add(generate_tree((Dictionary<string, object>)node));
                    }
                }
                else
                {
                    strNode = String.Format("{0}: {1}", dictKey, dict[dictKey]);
                    return new TreeNode(strNode);
                }
            }
            currentNode.Text = strNode;
            return currentNode;
        }

        private void cblstEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawTreeLog();
        }

        void DrawTreeLog()
        {
            foreach (TreeNode node in treeLogCollection)
            {
                if (cblstEvents.CheckedItems.Contains(node.Text))
                    treeLog.Nodes.Add(node);
            }
        }

    }
}
