using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Common
{
    public class Params
    {
        private static List<Location> _listLocation = null;
        private static List<LocationSub> _listLocationSub = null;
        private static IEnumerable<SelectListItem> _listItemLocationSub = null;
        private static IEnumerable<SelectListItem> _listItemCatalogUnit = null;
        private static aspnetEntities _aspnetEntities;
        private static retailEntities _retailEntities;
        public static IEnumerable<SelectListItem> listItemCatalogUnit
        {
            get
            {
                if (_listItemCatalogUnit == null)
                {
                    _listItemCatalogUnit = ModelaspnetEntities.CatalogUnits.Where(a => a.Show == true).OrderBy(d => d.Name).AsEnumerable().Select(d => new SelectListItem
                    {
                        Value = d.ID.ToString(),
                        Text = d.Name
                    });
                }
                return _listItemCatalogUnit;
            }
        }
        public static List<Location> listLocation
        {
            get
            {
                if (_listLocation == null)
                {
                    _listLocation = ModelaspnetEntities.Locations.ToList();
                }
                return _listLocation;
            }
        }
        public static List<LocationSub> listLocationSub
        {
            get
            {
                if (_listLocationSub == null)
                {
                    _listLocationSub = ModelaspnetEntities.LocationSubs.ToList();
                }
                return _listLocationSub;
            }
        }
        public static IEnumerable<SelectListItem> listItemLocationSub
        {
            get
            {
                if (_listItemLocationSub == null)
                {
                    _listItemLocationSub = ModelaspnetEntities.LocationSubs.Where(a => a.IsDel != 1 & a.Show == true).OrderBy(d => d.Name).AsEnumerable().Select(d => new SelectListItem
                    {
                        Value = d.ID.ToString(),
                        Text = d.Name
                    });
                }
                return _listItemLocationSub;
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