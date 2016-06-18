using MVCProject.Common;
using MVCProject.Models.ModelView;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Models.AccessData
{
    public class ALoginHistory
    {
        private volatile static ALoginHistory _instance;
        static object locked = new object();
        public static ALoginHistory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locked)
                        _instance = new ALoginHistory();
                }
                return _instance;
            }

        }
        private ALoginHistory() { }

        public void save(LoginHistory obj)
        {
            try
            {
                using (var model = Params.ModelaspnetEntities)
                {
                    model.LoginHistories.Add(obj);
                    model.SaveChanges();
                }
            }
            catch
            {

            }
        }
        public IPagedList<LoginHistory> GetListDetail(string page = "", string size = "", string acc = "", string datefrom = "", string dateto = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var list = from l in model.LoginHistories select l;
                if (!string.IsNullOrEmpty(datefrom) & !string.IsNullOrEmpty(dateto))
                {
                    var _datefrom = UtilDatetime.FromTime(datefrom);
                    var _dateto = UtilDatetime.ToTime(dateto);
                    TimeSpan time = _dateto - _datefrom;
                    if (time.TotalDays > 93)
                    {
                        _dateto = _datefrom.AddDays(93);
                    }
                    list = list.Where(a => a.datelogin >= _datefrom && a.datelogin <= _dateto);
                }
                else if (!string.IsNullOrEmpty(datefrom))
                {
                    var _datefrom = UtilDatetime.FromTime(datefrom);
                    var _dateto = _datefrom.AddDays(93);
                    list = list.Where(a => a.datelogin >= _datefrom && a.datelogin <= _dateto);
                }
                else if (!string.IsNullOrEmpty(dateto))
                {
                    var _dateto = UtilDatetime.ToTime(dateto);
                    var _datefrom = _dateto.AddDays(-93);
                    list = list.Where(a => a.datelogin >= _datefrom && a.datelogin <= _dateto);
                }

                list = list.Where(a => a.username.Contains(acc));
                int _page = page == "" ? 1 : int.Parse(page);
                int _size = size == "" ? 20 : int.Parse(size);

                return list.OrderBy(a => new { a.username, a.datelogin }).ToPagedList(_page, _size);
            }
        }
        public IPagedList<LoginHistoryView> GetList(string page = "", string size = "", string acc = "", string datefrom = "", string dateto = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var list = from l in model.LoginHistories select l;
                if (!string.IsNullOrEmpty(datefrom) & !string.IsNullOrEmpty(dateto))
                {
                    var _datefrom = UtilDatetime.FromTime(datefrom);
                    var _dateto = UtilDatetime.ToTime(dateto);
                    TimeSpan time = _dateto - _datefrom;
                    if (time.TotalDays > 93)
                    {
                        _dateto = _datefrom.AddDays(93);
                    }
                    list = list.Where(a => a.datelogin >= _datefrom && a.datelogin <= _dateto);
                }
                else if (!string.IsNullOrEmpty(datefrom))
                {
                    var _datefrom = UtilDatetime.FromTime(datefrom);
                    var _dateto = _datefrom.AddDays(93);
                    list = list.Where(a => a.datelogin >= _datefrom && a.datelogin <= _dateto);
                }
                else if (!string.IsNullOrEmpty(dateto))
                {
                    var _dateto = UtilDatetime.ToTime(dateto);
                    var _datefrom = _dateto.AddDays(-93);
                    list = list.Where(a => a.datelogin >= _datefrom && a.datelogin <= _dateto);
                }

                list = list.Where(a => a.username.Contains(acc));
                int _page = page == "" ? 1 : int.Parse(page);
                int _size = size == "" ? 20 : int.Parse(size);

                var listGroup = from l in list
                                group l by l.username into gp
                                select new LoginHistoryView { account = gp.Key, count = gp.Count() };

                return listGroup.OrderBy(a => a.account).ToPagedList(_page, _size);
            }
        }
    }
}