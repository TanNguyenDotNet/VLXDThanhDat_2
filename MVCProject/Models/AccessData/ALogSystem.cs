using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCProject.Common;
using System.Reflection;

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">Table Name</param>
        /// <param name="_params">F1:User thay đổi</param>
        public void save(string code, string datetime,params string[] _params)
        {
            using (var model = Params.ModelaspnetEntities)
            {
                LogSystem log = new LogSystem();
                log.Code = code;
                log.Date = datetime;
                for (int i = 0; i < _params.Length; i++)
                {
                    PropertySet(log, "F" + (i + 1).ToString(), _params[i].ToString());
                }
                model.LogSystems.Add(log); model.SaveChanges();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="propName">object name of property</param>
        /// <param name="value"></param>
        static void PropertySet(object p, string propName, object value)
        {
            Type t = p.GetType();
            PropertyInfo info = t.GetProperty(propName);
            if (info == null)
                return;
            if (!info.CanWrite)
                return;
            info.SetValue(p, value, null);
        }
    }
}