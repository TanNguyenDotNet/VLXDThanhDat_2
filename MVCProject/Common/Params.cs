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
                if (_aspnetEntities==null 
                    ||_aspnetEntities.Database.Connection.State == System.Data.ConnectionState.Closed
                    || _aspnetEntities.Database.Connection.State == System.Data.ConnectionState.Broken)
                {
                    _aspnetEntities = new aspnetEntities();
                }
                return _aspnetEntities;
            }
        }
    }
}