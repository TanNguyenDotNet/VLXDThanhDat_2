using System;
using System.Collections.Generic;
using System.Web;

namespace MVCProject.Models.ModelView
{
    public class UsersView
    {
        private List<AspNetUser> _ListUsers;

        public List<AspNetUser> ListUsers
        {
            get { return _ListUsers; }
            set { _ListUsers = value; }
        }
        private List<AppNetUserType> _ListAppUsers;

        public List<AppNetUserType> ListAppUsers
        {
            get { return _ListAppUsers; }
            set { _ListAppUsers = value; }
        }
    }
}