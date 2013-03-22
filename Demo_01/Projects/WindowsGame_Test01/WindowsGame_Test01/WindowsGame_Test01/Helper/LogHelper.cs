using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace WindowsGame_Test01.Helper
{
    class LogHelper
    {
        private static StreamWriter writer = null;
        private const string logFileName = "c:\\log.txt";


        #region Static constructor to create log file
        static LogHelper()
        {
            FileStream file = new FileStream(logFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            writer = new StreamWriter(file);
            writer.BaseStream.Seek(0, SeekOrigin.End);
            writer.AutoFlush = true;
            writer.WriteLine("/// Session started at: " + DateTime.Now.ToString());
        }
        #endregion

        #region Write log entry
        public static void Write(string message)
        {
            DateTime ct = DateTime.Now;
            string s = "[" + ct.Hour.ToString("00") + ":" +
              ct.Minute.ToString("00") + ":" +
              ct.Second.ToString("00") + "] " +
              message;
            writer.WriteLine(s);
#if DEBUG
            // In debug mode write that message to the console as well!
            System.Console.WriteLine(s);
#endif
        }
        #endregion
    }
}
