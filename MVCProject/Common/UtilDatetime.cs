using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Common
{
    public class UtilDatetime
    {
        private static DateTime _datetime;
        public static DateTime FromTime(string date)
        {
            _datetime = StringToDateTime(date);
            _datetime = new DateTime(_datetime.Year, _datetime.Month, _datetime.Day, 0, 0, 0);
            return _datetime;
        }
        public static DateTime ToTime(string date)
        {
            _datetime = StringToDateTime(date);
            _datetime = new DateTime(_datetime.Year, _datetime.Month, _datetime.Day, 23, 59, 59);
            return _datetime;
        }
        private static DateTime StringToDateTime(string date)
        {
            return DateTime.Parse(date, new System.Globalization.CultureInfo("vi-VN"));
        }
    }
}