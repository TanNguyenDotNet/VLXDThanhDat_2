using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Models.ModelView
{
    public class LoginHistoryView
    {
        public string account { get; set; }
        public int count { get; set; }
        public Nullable<System.DateTime> datelogin { get; set; }
        public string locationname { set; get; }
    }
}