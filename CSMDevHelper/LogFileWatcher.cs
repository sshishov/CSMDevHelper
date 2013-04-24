using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CSMDevHelper
{
    delegate void LogUpdateDelegate(LogResult logResult);

    class LogFileWatcher: Control
    {
        Thread m_tWorker;
        private bool m_bStop = false;
        private bool m_bTail = false;
        private string m_sFileName = string.Empty;
        private enumDriverVersion m_eDriverVersion;
        frmCSMDH m_fParentForm;
        ManualResetEvent m_oPauseEvent = new ManualResetEvent(true);

        public LogFileWatcher(string filename, enumDriverVersion driverVersion, bool tail, frmCSMDH form)
        {
            m_sFileName = filename;
            m_eDriverVersion = driverVersion;
            m_bTail = tail;
            LogReader.reset();
            m_fParentForm = form;
        }

        public void run()
        {
            m_tWorker = new Thread(thread_run);
            m_tWorker.IsBackground = true;
            m_tWorker.Start();
        }

        public void stop()
        {
            if (IsHandleCreated)
            {
                Invoke(new EventHandler(delegate { thread_stop(); }));
            }
            else
            {
                thread_stop();
            }
        }

        public void pause()
        {
            m_oPauseEvent.Reset();
        }

        public void resume()
        {
            m_oPauseEvent.Set();
        }

        private void thread_stop()
        {
            m_bStop = true;
        }

        private void thread_run()
        {
            while (!m_bStop)
            {
                m_oPauseEvent.WaitOne(Timeout.Infinite);
                if (File.Exists(m_sFileName) && LogReader.s_readPosition != new FileInfo(m_sFileName).Length)
                {
                    if (LogReader.s_readPosition > new FileInfo(m_sFileName).Length)
                    {
                        LogReader.reset();
                    }
                    LogReader logReader = null;
                    if (m_eDriverVersion == enumDriverVersion.CP5000)
                    {
                        logReader = new LogCPReader(m_sFileName, m_bTail);
                    }
                    else if (m_eDriverVersion == enumDriverVersion.MCD5x)
                    {
                        logReader = new LogMCDReader(m_sFileName, m_bTail);
                    }
                    Thread logThread = new Thread(new ParameterizedThreadStart(ThreadLogUpdate));
                    logThread.Name = "LogReaderThread";
                    logThread.Priority = ThreadPriority.Lowest;
                    logThread.IsBackground = true;
                    logThread.Start(logReader);
                    while (!logThread.IsAlive) ;
                    logThread.Join();
                }
                Thread.Sleep(100);
            }
        }

        void ThreadLogUpdate(object logReader)
        {
            LogResult logResult;
            for (;;)
            {
                logResult = ((LogReader)logReader).Process();
                if (logResult.code == LogCode.LOG_EOF) { break; }
                m_fParentForm.LogUpdateHandler(logResult);
            }
            ((LogReader)logReader).Close();
        }
    }
}
