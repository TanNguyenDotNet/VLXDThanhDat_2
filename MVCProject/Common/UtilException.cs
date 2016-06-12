using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MVCProject.Common
{
    public class UtilException
    {
        public static void ErrorLog(string sPathName, Exception ex)
        {
            if (!System.IO.Directory.Exists(sPathName))
                System.IO.Directory.CreateDirectory(sPathName);
            StreamWriter sw = new StreamWriter(sPathName + DateTime.Now.ToString("ddMMyyyyHHmm") + ".txt", true);
            sw.WriteLine(ex.InnerException.ToString());
            sw.Flush();
            sw.Close();
        }
        public static void ErrorLog(string sPathName, string str)
        {
            if (!System.IO.Directory.Exists(sPathName))
                System.IO.Directory.CreateDirectory(sPathName);
            StreamWriter sw = new StreamWriter(sPathName + DateTime.Now.ToString("ddMMyyyyHHmm") + ".txt", true);
            sw.WriteLine(str);
            sw.Flush();
            sw.Close();
        }
    }
}