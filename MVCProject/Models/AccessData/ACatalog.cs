using MVCProject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Models.AccessData
{
    public class ACatalog
    {
        private volatile static ACatalog _instance;
        static object locked = new object();
        public static ACatalog Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locked)
                        _instance = new ACatalog();
                }
                return _instance;
            }

        }
        private ACatalog()
        { }
        public List<Catalog> GetList()
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var list = model.Catalogs.ToList();
                return list;
            }
        }
        public List<Catalog> GetList(string id = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                int _id = int.Parse(id);
                var list = model.Catalogs.Where(a => a.ID == _id).ToList();
                return list;
            }
        }
    }
}