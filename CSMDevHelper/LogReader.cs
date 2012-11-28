﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace CSMDevHelper
{
    class LogReader
    {
        private FileStream m_fileStream;
        private StreamReader m_streamReader;
        private static string logPattern = @"
            (?ix)
            (?<DATE>\d{2}/\d{2}/\d{4})\s+
            (?<TIME>\d{1,2}:\d{1,2}:\d{1,2}\s(PM|AM))\s+
            (?<UNK1>\d+)\s+
            (?<UNK2>\w+)\s+
            :\s+Lay7Dec\|\s*
            (?<MESSAGE>{\s*""MitaiEvent"".*)";

        public LogReader(string filename)
        {
            m_fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            m_streamReader = new StreamReader(m_fileStream);
            m_fileStream.Seek(0, SeekOrigin.End);
        }

        ~LogReader() { }

        public string Process()
        {
            string result;
            result = m_streamReader.ReadLine();
            if (result != null)
            {
                Console.WriteLine(result);
                Match logMatch = Regex.Match(result, logPattern, RegexOptions.IgnorePatternWhitespace);
                if (logMatch.Success)
                {
                    result = logMatch.Groups["MESSAGE"].Value;
                    // Workaround for incorrectly boolean variables
                    result = result.Replace("FALSE", "false");
                    result = result.Replace("TRUE", "true");
                }
                else
                {
                    result = "";
                }
            }
            return result;
        }
    }
}