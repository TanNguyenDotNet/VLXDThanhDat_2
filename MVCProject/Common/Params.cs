using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Common
{
    public class Params
    {
        private static List<Location> _listLocation = null;
        private static aspnetEntities _aspnetEntities;
        private static retailEntities _retailEntities;
        public static List<Location> listLocation
        {
            get
            {
                if(_listLocation==null)
                {
                    _listLocation = ModelaspnetEntities.Locations.ToList();
                }
                return _listLocation;
            }
        }
        public static aspnetEntities ModelaspnetEntities
        {
            get
            {
                _aspnetEntities = new aspnetEntities();
                return _aspnetEntities;
            }
        }
        public static retailEntities ModelRetail
        {
            get
            {
                _retailEntities = new retailEntities();
                return _retailEntities;
            }
        }
    }
}