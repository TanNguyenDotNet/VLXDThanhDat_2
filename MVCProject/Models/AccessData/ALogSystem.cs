using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCProject.Common;

namespace MVCProject.Models.AccessData
{
    public class ALogSystem
    {
        private volatile static ALogSystem _instance;
        static object locked = new object();
        public static ALogSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locked)
                        _instance = new ALogSystem();
                }
                return _instance;
            }

        }
        private ALogSystem()
        { }
        public void save(string code, string datetime, string[] _params)
        {
            using (var model = Params.ModelaspnetEntities)
            {
                LogSystem log = new LogSystem();
                log.Code = code;
                log.Date = datetime;
                foreach (var item in _params)
                {
                    log.F1 = item.ToString();
                }
                model.LogSystems.Add(log); model.SaveChanges();
            }
        }
    }
}