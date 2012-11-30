using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CSMDevHelper
{
    public static class LogCode
    {
        public const int LOG_NOTHING = 0;
        public const int LOG_MITAI = 1;
        public const int LOG_LEG = 2;
        public const int LOG_MODELING = 3;
    };

    public class LogResult
    {
        public int code;
        public string result;

        public LogResult(int code, string result)
        {
            this.code = code;
            this.result = result;
        }
    }

    class LogReader
    {
        private FileStream m_fileStream;
        private StreamReader m_streamReader;
        private static string logHeaderPattern = @"
            (?ix)
            (?<DATE>\d{2}/\d{2}/\d{4})\s+
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
            int code = -1;
            Match logMatch;
            result = this.m_streamReader.ReadLine();
            if (result != null && result != String.Empty)
            {
                // Console.WriteLine(result);
                logMatch = Regex.Match(result, LogReader.logHeaderPattern, RegexOptions.IgnorePatternWhitespace);
                if (logMatch.Success)
                {
                    result = logMatch.Groups["MESSAGE"].Value;
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
                    else if (this.m_isModeling)
                    {
                        code = LogCode.LOG_MODELING;
                    }
                    else
                    {
                        code = LogCode.LOG_NOTHING;
                    }
                }
            }
            return new LogResult(code, result);
        }
    }
}
