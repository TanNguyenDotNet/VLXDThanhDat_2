using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCProject.Common;

namespace MVCProject.Models.AccessData
{
    public class AUsers
    {
        private volatile static AUsers _instance;
        static object locked = new object();
        public static AUsers Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locked)
                        _instance = new AUsers();
                }
                return _instance;
            }

        }
        private AUsers()
        { }

        public IEnumerable<AspNetUser> GetListByUsername(List<string> usersname)
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var list = from u in model.AspNetUsers.Where(a=> usersname.Contains(a.UserName)) select u;
                return list.ToList();
            }
        }
    }
}