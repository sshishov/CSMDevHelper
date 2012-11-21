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
            treeLogCollection = this.treeLog.Nodes;
            isLogUpdate = false;
        }

        private void btnLogStart_Click(object sender, EventArgs e)
        {
            btnLogStart.Enabled = false;
            btnLogRestart.Enabled = true;
            btnLogStop.Enabled = true;
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
            treeLogCollection.Clear();
        }

        void ThreadLogUpdate()
        {
            string update = "";
            LogReader logReader = new LogReader(@"C:\DEV_logs.txt");
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
            if (isLogUpdate)
            {
                JavaScriptSerializer mySer = new JavaScriptSerializer();
                try
                {
                    Dictionary<string, object> dict = mySer.Deserialize<Dictionary<string, object>>(str);
                    this.treeLogCollection.Add(generate_tree(dict, null));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        TreeNode generate_tree(Dictionary<string, object> dict, TreeNode parent)
        {
            TreeNode currentNode = new TreeNode();
            string strNode = "";

            foreach(string dictKey in dict.Keys){
                object dictValue = dict[dictKey];
                if (parent == null)
                {
                    Dictionary<string, object> eee;
                    parent = currentNode;
                    object eventType;
                    if (((Dictionary<string, object>)dictValue).TryGetValue("Type", out eventType))
                        parent.Text = (string)eventType;
                }
                if ((object)dictValue is Dictionary<string, object>)
                {
                    strNode = dictKey;
                    parent.Nodes.Add(generate_tree((Dictionary<string, object>)dictValue, currentNode));
                }
                else if ((Object)dictValue is ArrayList)
                {
                    strNode = dictKey;
                    foreach(Dictionary<string, object> node in (ArrayList)dictValue)
                    {
                        parent.Nodes.Add(generate_tree((Dictionary<string, object>)node, currentNode));
                    }
                }
                else
                {
                    strNode = String.Format("{0}: {1}", dictKey, dict[dictKey]);
                    return new TreeNode(strNode);
                }
            }
            Console.WriteLine("Adding " + strNode);
            if (currentNode.Text == "")
                currentNode.Text = strNode;
            return currentNode;
        }
    }
}
