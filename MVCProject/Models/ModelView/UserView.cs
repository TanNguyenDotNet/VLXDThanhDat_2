using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Models.ModelView
{
    public class UserView
    {
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        private string displayname;

        public string Displayname
        {
            get { return displayname; }
            set { displayname = value; }
        }

        private string locationname;

        public string Locationname
        {
            get { return locationname; }
            set { locationname = value; }
        }
        private string locationsubname;

        public string Locationsubname
        {
            get { return locationsubname; }
            set { locationsubname = value; }
        }
        
    }
}