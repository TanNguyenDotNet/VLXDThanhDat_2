﻿using MVCProject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using PagedList;

namespace MVCProject.Models.AccessData
{
    public class AAppNetUserType
    {
        private volatile static AAppNetUserType _instance;
        static object locked = new object();
        public static AAppNetUserType Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locked)
                        _instance = new AAppNetUserType();
                }
                return _instance;
            }

        }
        private AAppNetUserType()
        { }
        public IPagedList<AppNetUserType> GetUserListPaging(string userName = "", string page = "", string size = "", string filter = "", string localid = "", string type = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var list = from u in model.AppNetUserTypes
                           where u.UserOfName != userName && u.UserOfName != "admin"
                           select u;
                if (!string.IsNullOrEmpty(filter))
                    list = list.Where(a => a.UserOfName.Contains(filter) || a.DisplayName.Contains(filter));
                if (!string.IsNullOrEmpty(localid))
                { int b = int.Parse(localid); list = list.Where(a => a.LocationID == b); }
                if (!string.IsNullOrEmpty(type))
                    list = list.Where(a => a.UserOfType == type);
                list = list.OrderBy(a => a.DisplayName);
                int _page = page == "" ? 1 : int.Parse(page);
                int _size = size == "" ? 20 : int.Parse(size);
                return list.ToPagedList(_page, _size);
            }
        }
        public IEnumerable<AppNetUserType> GetUserList(string userName = "", string filter = "", string localid = "", string type = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var list = from u in model.AppNetUserTypes
                           where u.UserOfName != userName && u.UserOfName != "admin"
                           select u;
                if (!string.IsNullOrEmpty(filter))
                    list = list.Where(a => a.UserOfName.Contains(filter) || a.DisplayName.Contains(filter));
                if (!string.IsNullOrEmpty(localid))
                { int b = int.Parse(localid); list = list.Where(a => a.LocationID == b); }
                if (!string.IsNullOrEmpty(type))
                    list = list.Where(a => a.UserOfType == type);
                list = list.OrderBy(a => a.DisplayName);

                return list.ToList();
            }
        }
        public AppNetUserType GetUserById(string id)
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var user = model.AspNetUsers.Where(a => a.Id == id).FirstOrDefault();
                return model.AppNetUserTypes.Where(a => a.UserOfName == user.UserName).FirstOrDefault();
            }
        }
        public List<AppNetUserType> GetListUserFrontendByLocation(string idlocation = "", string idlocationsub = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var users = from u in model.AppNetUserTypes where u.UserOfType == "FrontendUser" select u;
                if (idlocation != "")
                { int _id = int.Parse(idlocation); users = users.Where(a => a.LocationID == _id); }
                if (idlocationsub != "")
                { int _id = int.Parse(idlocationsub); users = users.Where(a => a.LocationSubID == _id); }
                return users.ToList();
            }
        }
        public AppNetUserType GetUserByUserName(string username)
        {
            using (var model = Params.ModelaspnetEntities)
            {
                return model.AppNetUserTypes.Where(a => a.UserOfName == username).FirstOrDefault();
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