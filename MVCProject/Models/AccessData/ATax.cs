using MVCProject.Common;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Models.AccessData
{
    public class ATax
    {
        private volatile static ATax _instance;
        static object locked = new object();
        public static ATax Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locked)
                        _instance = new ATax();
                }
                return _instance;
            }

        }
        private ATax()
        { }
        public List<Tax> GetList()
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var list = model.Taxes.ToList();
                return list;
            }
        }
    }
}