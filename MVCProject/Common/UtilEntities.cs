using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace MVCProject.Common
{
    public class UtilEntities
    {
        public static List<ExpandoObject> modelDynamic<T>(List<T> query)
        {
            List<ExpandoObject> joinData = new List<ExpandoObject>();
            foreach (var item in query)
            {
                IDictionary<string, object> itemExpando = new ExpandoObject();
                foreach (PropertyDescriptor property
                         in
                         TypeDescriptor.GetProperties(item.GetType()))
                {
                    itemExpando.Add(property.Name, property.GetValue(item));
                }
                joinData.Add(itemExpando as ExpandoObject);
            }
            return joinData;
        }
    }
}