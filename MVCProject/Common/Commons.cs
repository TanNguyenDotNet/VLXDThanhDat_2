using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace MVCProject.Common
{
    public class Commons
    {
        public static UserType UT { set; get; }
        public static IEnumerable<SelectListItem> GetLocationList(Models.aspnetEntities db)
        {
            return db.Locations.OrderBy(d=>d.Order).AsEnumerable()
                .Select(d => new SelectListItem
                {
                    Value = d.ID.ToString(),
                    Text = d.LocationName
                });
        }

        public static IEnumerable<SelectListItem> GetCatalogList(Models.aspnetEntities db, int? IgnoreID)
        {
            List<Models.Catalog> result = db.Catalogs.Where(d => d.ID != IgnoreID).ToList();

            return result.AsQueryable()
                .Select(d => new SelectListItem
                {
                    Value = d.ID.ToString(),
                    Text = d.Title
                });
        }

        public static IEnumerable<SelectListItem> GetPromotionTypeList(Models.aspnetEntities db)
        {
            return db.PromotionTypes.AsEnumerable()
                .Select(d => new SelectListItem
                {
                    Value = d.ID.ToString(),
                    Text = d.ProTypeName
                });
        }


        public static IEnumerable<SelectListItem> GetProductList(Models.aspnetEntities db)
        {
            return db.Products.OrderBy(c=>c.ProductName).AsEnumerable()
                .Select(d => new SelectListItem
                {
                    Value = d.ID.ToString(),
                    Text = d.ProductName
                });
        }

        public static IEnumerable<SelectListItem> GetSupplierList(Models.aspnetEntities db)
        {
            return db.Suppliers.AsEnumerable()
                .Select(d => new SelectListItem
                {
                    Value = d.ID.ToString(),
                    Text = d.Name
                });
        }

        public static IEnumerable<SelectListItem> GetWarrantyList(Models.aspnetEntities db)
        {
            return db.Warranties.AsEnumerable()
                .Select(d => new SelectListItem
                {
                    Value = d.Value + d.DVT,
                    Text = d.Title
                });
        }

        public static dynamic GetCatalogCodeList(Models.aspnetEntities db)
        {
            return db.Catalogs.AsEnumerable()
                .Select(d => new SelectListItem
                {
                    Value = d.Title,
                    Text = d.Code
                });
        }
        public static dynamic GetTaxList(Models.aspnetEntities db)
        {
            return db.Taxes.AsEnumerable()
                .Select(d => new SelectListItem
                {
                    Value = d.ID.ToString(),
                    Text = d.Desc
                });
        }

        public static string GenItemCode(Models.aspnetEntities db, out int useCatCode, string itemType)
        {
            Models.ProductCode pc = db.ProductCodes.Single(d => d.Active == true && d.CatCode == itemType);
            int s = pc.ScrollNumber + 1;
            pc.ScrollNumber = s;
            db.SaveChanges();
            useCatCode = ((bool) pc.CatID) ? 1 : 0;
            string code = (pc.Group1 != "" ? pc.Group1 + "." : "") +
                (pc.Group2 != "" ? pc.Group2 + "." : "") +
                string.Format("{0:000000}", pc.ScrollNumber);
            return code;
        }

        public static bool CheckLogin(HttpRequestBase req, HttpResponseBase res, string username)
        {
            if (!req.IsAuthenticated)
                res.Redirect("~/Account/Login");

            string en = Security.EncryptString("User:" + username + "~BackendUser", false, EncryptType.TripleDES);
            Models.aspnetEntities db = new Models.aspnetEntities();

            try
            {
                var i = db.AppNetUserTypes.Where(d => d.Username == en).ToList();
                if (i == null || i.Count == 0)
                {
                    res.Redirect("~/Account/Login");
                    return false;
                }
                return true;
            }
            catch { return false; }
        }

        public static UserType GetUserType(HttpRequestBase req, HttpResponseBase res, string username, Models.aspnetEntities db)
        {
            try
            {
                string ena = Security.EncryptString("User:" + username + "~BackendUser", false, EncryptType.TripleDES);
                string end = Security.EncryptString("User:" + username + "~DeliveryMan", false, EncryptType.TripleDES);
                var i = db.AppNetUserTypes.Where(d => d.Username == ena).ToList();
                var id = db.AppNetUserTypes.Where(d => d.Username == end).ToList();
                UserType rval = UserType.User;
                if (i != null && i.Count > 0)
                    rval = UserType.Administrator;
                else if (id != null && id.Count > 0)
                    rval = UserType.Delivery;
                return rval;
            }
            catch { return UserType.None; }
        }
        // Save upload file
        public static string Save(HttpPostedFileBase file, string path, string name)
        {
            string reval = "";
            try
            {
                if (file.ContentLength > 0)
                {
                    string ogrName = Path.GetFileName(file.FileName);
                    reval = String.Format("{0}.{1}", name, ogrName.Split('.')[1]);
                    string tmpPath = Path.Combine(path, reval);
                    file.SaveAs(tmpPath);
                }
                return reval;
            }
            catch { return ""; }
        }

        public static Dictionary<string, string> GetFullName(List<Models.AppNetUserType> types)
        {
            try
            {
                Dictionary<string, string> list = new Dictionary<string, string>();
                foreach (Models.AppNetUserType type in types)
                    if (type.DisplayName != null && type.DisplayName != "")
                        list.Add(type.Username, type.DisplayName);
                    else
                        list.Add(type.Username, "");
                return list;
            }
            catch { return null; }
        }

        public static Dictionary<long, string> GetPriceList(Models.aspnetEntities db)
        {
            try
            {
                Dictionary<long, string> list = new Dictionary<long, string>();
                var pl = db.Products.ToList();
                foreach (Models.Product p in pl)
                    if (p.Price != null)
                        list.Add(p.ID, double.Parse(p.Price.ToString()).ToString("#,###"));
                return list;
            }
            catch { return null; }
        }

        public static Dictionary<string, string> GetDeliveryList(Models.aspnetEntities db)
        {
            try
            {
                Dictionary<string, string> list = new Dictionary<string, string>();
                foreach (Models.AspNetUser u in db.AspNetUsers)
                {
                    string enu = Security.EncryptString("User:" + u.UserName + "~DeliveryMan", false, EncryptType.TripleDES);
                    List<Models.AppNetUserType> utl = db.AppNetUserTypes.Where(c => c.Username == enu).ToList();
                    
                    if (utl != null && utl.Count > 0)
                    {
                        if (utl[0].DisplayName != null && utl[0].DisplayName != "")
                            list.Add(u.Id, utl[0].DisplayName);
                        else
                            list.Add(u.Id, u.UserName);
                    }
                }

                return list;
            }
            catch { return null; }
        }

        public static Dictionary<int, string> GetCityName(List<Models.Location> citys)
        {
            try
            {
                Dictionary<int, string> list = new Dictionary<int, string>();
                foreach (Models.Location type in citys)
                    if (type.LocationName != null && type.LocationName != "")
                        list.Add(type.ID, type.LocationName);
                return list;
            }
            catch { return null; }
        }

        public static bool CheckPermission(ViewDataDictionary ViewData, Models.aspnetEntities db, 
            string username, string role)
        {
            string enu = Security.EncryptString("User:" + username + "~BackendUser", false, EncryptType.TripleDES);
            var list = db.UserRoles.Where(c => c.UserName == enu).ToList();
            List<string> access = new List<string>();
            bool canaccess = false;
            if (role == null || role == "")
                canaccess = true;

            if (list != null && list.Count() > 0)
            {
                foreach (var i in list)
                {
                    access.Add(i.RoleId);
                    if (role != null && role != "" && role == i.RoleId)
                        canaccess = true;
                }
            }
            ViewData["AccessList"] = access;

            return canaccess;
        }
    }
}