using MVCProject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCProject.Models.ModelView;
using System.Data.Entity.Validation;

namespace MVCProject.Models.AccessData
{
    public class AOrdersDetail
    {
        private volatile static AOrdersDetail _instance;
        static object locked = new object();
        public static AOrdersDetail Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locked)
                        _instance = new AOrdersDetail();
                }
                return _instance;
            }

        }
        private AOrdersDetail() { }
        public List<OrdersDetail> GetListByCode(string code)
        {
            using(var model = Params.ModelRetail)
            {
                return model.OrdersDetails.Where(a => a.OrderCode == code).ToList();
            }
        }

        public List<OrdersDetail> GetListProductCode(List<string> lstOrderCode)
        {
            using (var model = Params.ModelRetail)
            {
                return model.OrdersDetails.Where(a=> lstOrderCode.Contains(a.OrderCode)).ToList();
            }
        }
    }
}