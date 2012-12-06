using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CSMDevHelper
{
    public enum LogCode: int
    {
        LOG_NOTHING = 0,
        LOG_MITAI = 1,
        LOG_LEG = 2,
        LOG_MODELING = 3,
    };

    public class LogResult
    {
        public LogCode code;
        public string result;
        public string timestamp;

        public LogResult(LogCode code, string result, string timestamp)
        {
            this.code = code;
            this.result = result;
            this.timestamp = timestamp;
        }
    }

    class LogReader
    {
        private FileStream m_fileStream;
        private StreamReader m_streamReader;
        private static string logHeaderPattern = @"
            (?ix)
            (?<DATE>\d{1,2}/\d{1,2}/\d{1,4})\s+
            (?<TIME>\d{1,2}:\d{1,2}:\d{1,2}\s(PM|AM|))\s*
            (?<UNK1>\d+)\s+
            (?<UNK2>\w+)\s+:s*
            (?<MESSAGE>.*)";
        private static string logMitaiPattern = @"\s+Lay7Dec\|\s*(?<MESSAGE>{\s*""MitaiEvent"".*)";
        private static string logModelingPattern = @"^=+";
        private bool m_isModeling;

        public LogReader(string filename, bool fromBeginning)
        {
            this.m_fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            this.m_streamReader = new StreamReader(m_fileStream);
            this.m_isModeling = false;
            if (fromBeginning)
            {
                this.m_fileStream.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                this.m_fileStream.Seek(0, SeekOrigin.End);
            }
        }

        ~LogReader() { }

        public LogResult Process()
        {
            string result = String.Empty;
            string timestamp = String.Empty;
            LogCode code = LogCode.LOG_NOTHING;
            Match logMatch;
            result = this.m_streamReader.ReadLine();
            if (result != null && result != String.Empty)
            {
                // Console.WriteLine(result);
                logMatch = Regex.Match(result, LogReader.logHeaderPattern, RegexOptions.IgnorePatternWhitespace);
                if (logMatch.Success)
                {
                    result = logMatch.Groups["MESSAGE"].Value;
                    timestamp = logMatch.Groups["DATE"].Value + " " + logMatch.Groups["TIME"].Value; ;
                    // Workaround for incorrectly boolean variables
                    result = result.Replace("FALSE", "false");
                    result = result.Replace("TRUE", "true");
                    logMatch = Regex.Match(result, LogReader.logMitaiPattern, RegexOptions.IgnorePatternWhitespace);
                    if (logMatch.Success)
                    {
                        code = LogCode.LOG_MITAI;
                        result = logMatch.Groups["MESSAGE"].Value;
                    }
                    else if(result == String.Empty);
                    {
                        
                    }
                }
                else
                {
                    if (Regex.Match(result, LogReader.logModelingPattern, RegexOptions.IgnorePatternWhitespace).Success)
                    {
                        this.m_isModeling = !this.m_isModeling;
                    }
                    if (this.m_isModeling)
                    {
                        code = LogCode.LOG_MODELING;
                    }
                }
            }
            return new LogResult(code, result, timestamp);
        }
    }
}
