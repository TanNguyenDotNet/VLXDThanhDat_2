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
        public static List<DateTime> ListGetBeginDayAndEndDayInMonth(string month, string year)
        {
            int day = DateTime.DaysInMonth(int.Parse(year), int.Parse(month));

            DateTime BeginDay = new DateTime(int.Parse(year), int.Parse(month), 1, 0, 0, 0);
            DateTime EndDay = new DateTime(int.Parse(year), int.Parse(month), day, 23, 59, 59);
            List<DateTime> lst = new List<DateTime>() { BeginDay, EndDay };
            return lst;
        }
        public static List<DateTime> ListGetBeginDayAndEndDayInYear(string year)
        {
            DateTime BeginDay = new DateTime(int.Parse(year), 1, 1, 0, 0, 0);
            DateTime EndDay = new DateTime(int.Parse(year), 12, 31, 23, 59, 59);
            List<DateTime> lst = new List<DateTime>() { BeginDay, EndDay };
            return lst;
        }
        public static List<DateTime> ListGetBeginDayAndEndDayInQuarter(string quarter, string year)
        {
            DateTime BeginDay = DateTime.Now;
            DateTime EndDay = DateTime.Now;
            int _year = int.Parse(year);
            switch (quarter)
            {
                case "1":
                    BeginDay = new DateTime(_year, 1, 1, 0, 0, 0);
                    EndDay = new DateTime(_year, 3, 31, 23, 59, 59);
                    break;
                case "2":
                    BeginDay = new DateTime(_year, 4, 1, 0, 0, 0);
                    EndDay = new DateTime(_year, 6, DateTime.DaysInMonth(_year, 6), 23, 59, 59);
                    break;
                case "3":
                    BeginDay = new DateTime(_year, 7, 1, 0, 0, 0);
                    EndDay = new DateTime(_year, 9, DateTime.DaysInMonth(_year, 9), 23, 59, 59);
                    break;
                case "4":
                    BeginDay = new DateTime(_year, 10, 1, 0, 0, 0);
                    EndDay = new DateTime(_year, 12, DateTime.DaysInMonth(_year, 12), 23, 59, 59);
                    break;
            }
            List<DateTime> lst = new List<DateTime>() { BeginDay, EndDay };
            return lst;
        }
    }
}