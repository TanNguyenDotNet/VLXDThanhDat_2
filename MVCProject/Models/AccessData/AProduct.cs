using MVCProject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Models.AccessData
{
    public class AProduct
    {
        private volatile static AProduct _instance;
        static object locked = new object();
        public static AProduct Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locked)
                        _instance = new AProduct();
                }
                return _instance;
            }

        }
        private AProduct() { }

        public List<Product> GetList()
        {
            using (var model = Params.ModelaspnetEntities)
            {
                return model.Products.ToList();
            }
        }
    }
}