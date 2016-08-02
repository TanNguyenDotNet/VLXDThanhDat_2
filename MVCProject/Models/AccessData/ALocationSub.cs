using MVCProject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Models.AccessData
{
    public class ALocationSub
    {
        private volatile static ALocationSub _instance;
        static object locked = new object();
        public static ALocationSub Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locked)
                        _instance = new ALocationSub();
                }
                return _instance;
            }

        }
        private ALocationSub()
        { }
        public List<LocationSub> GetList()
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var list = model.LocationSubs.ToList();
                return list;
            }
        }
        public List<LocationSub> GetList(bool is_show = true, string id = "", string idlocation = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var list = from l in model.LocationSubs select l;
                list = list.Where(l => l.Show == is_show);
                if (!string.IsNullOrEmpty(id))
                { int _id = int.Parse(id); list = list.Where(a => a.ID == _id); }
                if (!string.IsNullOrEmpty(idlocation))
                { int _id = int.Parse(idlocation); list = list.Where(a => a.IDLocation == _id); }
                return list.ToList();
            }
        }
    }
}