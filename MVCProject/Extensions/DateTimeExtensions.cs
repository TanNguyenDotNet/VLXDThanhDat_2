using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Extensions
{
    public static class DateTimeExtensions
    {
        public static string GetDateTimeToString(this DateTime time)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}