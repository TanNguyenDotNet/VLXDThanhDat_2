using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCProject.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using MVCProject.Common;

namespace MVCProject.Controllers
{
    public class ProductController : Controller
    {
        private aspnetEntities db = new aspnetEntities();
        private string reval = "";
        private string introImg = "";

        #region Local actions
        public ActionResult Home(int? page, int? size, string filter, string order, string catid)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            ViewBag.UserType = Common.Commons.GetUserType(Request, Response, User.Identity.GetUserName(), db);

            string enu = Security.EncryptString("User:" + User.Identity.GetUserName() + "~FrontendUser", false, EncryptType.TripleDES);
            var Users = db.AppNetUserTypes.Where(a => a.Username == enu).FirstOrDefault();
            int subid = (int)Users.LocationSubID;
            InitItem(false);
            var list = GetList(filter, order, catid == null || catid == "" ? "0" : catid, true);
            decimal priceSub = decimal.Parse((db.LocationSubs.Where(a => a.ID == subid).FirstOrDefault().LocationPrice)) / 100;
            list.ToList().ForEach(a => a.Price = a.Price + (a.Price * priceSub));
            var listProductPriceSub = db.ProductPrices.Where(a => a.LocationID == subid).ToList();
            if (listProductPriceSub.Count != 0)
            {
                var listpp = (from p in db.Products.ToList()
                              join pp in listProductPriceSub on p.ID equals pp.ProductID
                              select p).ToList();

                foreach (var item in listpp)
                {
                    item.Price = listProductPriceSub.Where(a => a.ProductID == item.ID).FirstOrDefault().Price;
                }

                list.ToList().AddRange(listpp.ToList());//Tai sao addrange chi? thay doi gia tri cua object?
            }
            return View(list.ToPagedList(page == null ||
                page == 0 ? 1 : (int)page, size == null || size == 0 ? 20 : (int)size));
        }

        // GET: /Product/
        public ActionResult Index(int? page, int? size, string filter, string order, string catid)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), null))
                return RedirectToAction("AccessDenied", "Account");
            InitItem(false);
            var list = GetList(filter, order, catid == null || catid == "" ? "0" : catid);
            return View(list.ToPagedList(page == null ||
                page == 0 ? 1 : (int)page, size == null || size == 0 ? 20 : (int)size));
        }



        // GET: /Product/Details/5
        public ActionResult Details(long? id)
        {
            //if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
            //    return null;
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Product product = db.Products.Find(id);
            //if (product == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(product);
            return null;
        }

        // GET: /Product/Create
        public ActionResult Create()
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "1"))
                return RedirectToAction("AccessDenied", "Account");

            var p = new Models.ProductViewModel();
            int useCatCode = 0;
            p.Barcode = p.SKU = p.ItemCode = Common.Commons.GenItemCode(db, out useCatCode, "SP"); p.Show = true; p.IsDel = false;
            ViewBag.CatalogList = Common.Commons.GetCatalogList(db, 0);
            ViewBag.SupplierList = Common.Commons.GetSupplierList(db);
            ViewBag.WarrantyList = Common.Commons.GetWarrantyList(db);
            ViewBag.TaxList = Common.Commons.GetTaxList(db);
            ViewData["UseCatCode"] = useCatCode;
            ViewData["CatCode"] = db.Catalogs.ToList();
            return View(p);
        }

        // POST: /Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Price,TaxID,ID,ItemCode,Barcode,CatID,SKU,SupplierID,ImageLink,Adwords,Show,DateCreate,Color,Dimension,Unit,Warranty,IsDel,IsState,UserID,ProductName")] ProductViewModel _product)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "1"))
                return RedirectToAction("AccessDenied", "Account");
            Product product = new Product();
            if (ModelState.IsValid)
            {
                product = SetObj(_product);
                Upload();
                SaveImage(introImg, "Intro", product.ItemCode, "Product", "");
                SaveImage(reval, "Detail", product.ItemCode, "Product", product.ImageLink);
                product.UserID = User.Identity.GetUserId();
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(_product);
        }

        // GET: /Product/Edit/5
        public ActionResult Edit(long? id)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "2"))
                return RedirectToAction("AccessDenied", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ProductViewModel _productView = new ProductViewModel();
            _productView = SetObjViewModel(product);
            int useCatCode = 0;
            ViewBag.CatalogList = Common.Commons.GetCatalogList(db, 0);
            ViewBag.SupplierList = Common.Commons.GetSupplierList(db);
            ViewBag.TaxList = Common.Commons.GetTaxList(db);
            ViewBag.WarrantyList = Common.Commons.GetWarrantyList(db);
            ViewBag.Price = _productView.Price.Value.ToString("n0");
            ViewData["UseCatCode"] = useCatCode;
            ViewData["CatCode"] = db.Catalogs.Select(d => d).ToList();
            return View(_productView);
        }

        // POST: /Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Price,TaxID,ID,ItemCode,Barcode,CatID,SKU,SupplierID,ImageLink,Adwords,Show,DateCreate,Color,Dimension,Unit,Warranty,IsDel,IsState,UserID,ProductName")] ProductViewModel _product)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "2"))
                return RedirectToAction("AccessDenied", "Account");
            Product product = new Product();
            if (ModelState.IsValid)
            {
                product = SetObj(_product);
                Upload();
                SaveImage(introImg, "Intro", product.ItemCode, "Product", "");
                SaveImage(reval, "Detail", product.ItemCode, "Product", product.ImageLink);
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(_product);
        }

        // GET: /Product/Delete/5
        public ActionResult Delete(long? id)
        {
            //if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
            //    return null;
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Product product = db.Products.Find(id);
            //if (product == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(product);
            return null;
        }

        // POST: /Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            //if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
            //    return null;
            //Product product = db.Products.Find(id);
            //db.Products.Remove(product);
            //db.SaveChanges();
            //return RedirectToAction("Index");
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Functions
        private string Upload()
        {
            try
            {
                reval = "";
                introImg = "";
                Random r = new Random();
                string RootPath = HttpContext.Server.MapPath("~/Images/");
                foreach (string inputTagName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[inputTagName];

                    string name = "IMG_" + DateTime.Now.ToString("ddMMyyyy") + "_" +
                        r.Next(1000000, 9999999);

                    string fileName = Common.Commons.Save(file, RootPath + (inputTagName == "intro" ? "Intro" : "Details"), name);

                    if (fileName != "")
                    {
                        if (inputTagName == "intro")
                            introImg = fileName;
                        else
                        {
                            if (reval != "") reval += ",";
                            reval += fileName;
                        }
                    }
                }

                return reval;
            }
            catch { return null; }
        }


        private void SaveImage(string image, string addIn, string itemCode, string component, string imageLink)
        {
            if (image != "")
            {
                if (addIn == "Intro")
                {
                    var list = db.ProductImages.Where(c => c.ProductCode == itemCode
                        && c.Component == component && c.ImageAddIn == addIn).ToList();
                    if (list != null && list.Count > 0)
                    {
                        db.ProductImages.Remove(list[0]);
                        db.SaveChanges();
                    }
                }

                Models.ProductImage pi = new ProductImage();
                pi.Image = image;
                pi.ImageAddIn = addIn;
                pi.ProductCode = itemCode;
                pi.Component = component;
                pi.ImageLink = imageLink;
                db.ProductImages.Add(pi);
                db.SaveChanges();
            }
        }

        IEnumerable<Models.Product> GetList(string filter, string order, string cid, bool show = false)
        {
            var list = from p in db.Products
                       select p;
            if (!string.IsNullOrEmpty(filter))
                list = list.Where(a => a.ProductName.Contains(filter));
            if (cid != "0")
            { long cat = long.Parse(cid); list = list.Where(a => a.CatID == cat); }
            if (show)
                list.Where(a => a.Show == show);
            list.Where(a => a.IsDel == false);
            list = OrderList(list, order);

            ViewBag.Order = order == null ? "" : order;
            ViewBag.Filter = filter == null ? "" : filter;
            return list.ToList();
        }

        void InitItem(bool isAdmin)
        {
            if (!isAdmin)
            {
                string Cart = "";
                if (Request.QueryString["Cart"] != null) Cart = Request.QueryString["Cart"];

                if (Cart != "" && (Session["Cart"] == null || Session["Cart"].ToString() != Cart))
                    Session["Cart"] = Cart;
                if (Cart == "" && Session["Cart"] != null && Session["Cart"].ToString() != "")
                    Cart = Session["Cart"].ToString();

                ViewData["Cart"] = Cart;
                Session["CartCount"] = ViewData["CartCount"] = Cart != "" ? Cart.Split(',').Length.ToString() : "0";
                ViewData["ImageList"] = db.ProductImages.Where(c => c.Component == "Product").ToList();
            }

            ViewData["CatList"] = db.Catalogs.Select(d => d).ToList();
        }

        IQueryable<Models.Product> OrderList(IQueryable<Models.Product> list, string order)
        {
            if (list == null || list.Count() == 0)
                return list;

            switch (order)
            {
                case "price":
                    list = list.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    list = list.OrderByDescending(s => s.Price);
                    break;
                case "name_desc":
                    list = list.OrderByDescending(s => s.ProductName);
                    break;
                default:
                    list = list.OrderBy(s => s.ProductName);
                    break;
            }

            return list;
        }

        private void SetLocationDetail()
        {
            var priceList = (from price in db.ProductPrices
                             join p in db.Products
                             on price.ProductID equals p.ID
                             select new
                             {
                                 price.ProductID,
                                 price.Price
                             });

            Dictionary<long, decimal> pList = null;
            if (priceList != null && priceList.Count() > 0)
            {
                pList = new Dictionary<long, decimal>();
                foreach (var item in priceList)
                    pList.Add(item.ProductID, item.Price);
            }
            ViewData["PriceList"] = pList;

            var nameList = (from name in db.ProductNames
                            join p in db.Products
                            on name.ProductID equals p.ID
                            select new
                            {
                                name.ProductID,
                                name.Name
                            });

            Dictionary<long, string> nList = null;
            if (nameList != null && nameList.Count() > 0)
            {
                nList = new Dictionary<long, string>();
                foreach (var item in nameList)
                    nList.Add(item.ProductID, item.Name);
            }
            ViewData["NameList"] = nList;
        }
        private Product SetObj(ProductViewModel _product)
        {
            Product p = new Product();
            p.ID = _product.ID;
            p.Adwords = _product.Adwords;
            p.Barcode = _product.Barcode;
            p.CatID = _product.CatID;
            p.Color = _product.Color;
            p.DateCreate = _product.DateCreate;
            p.Dimension = _product.Dimension;
            p.ImageLink = _product.ImageLink;
            p.IsDel = _product.IsDel;
            p.IsState = _product.IsState;
            p.ItemCode = _product.ItemCode;
            p.Price = _product.Price;
            p.ProductName = _product.ProductName;
            p.Show = _product.Show;
            p.SKU = _product.SKU;
            p.SupplierID = _product.SupplierID;
            p.TaxID = _product.TaxID;
            p.Unit = _product.Unit;
            p.UserID = _product.UserID;
            p.Warranty = _product.Warranty;
            return p;
        }
        private ProductViewModel SetObjViewModel(Product _product)
        {
            ProductViewModel p = new ProductViewModel();
            p.ID = _product.ID;
            p.Adwords = _product.Adwords;
            p.Barcode = _product.Barcode;
            p.CatID = _product.CatID;
            p.Color = _product.Color;
            p.DateCreate = _product.DateCreate;
            p.Dimension = _product.Dimension;
            p.ImageLink = _product.ImageLink;
            p.IsDel = _product.IsDel;
            p.IsState = _product.IsState;
            p.ItemCode = _product.ItemCode;
            p.Price = _product.Price;
            p.ProductName = _product.ProductName;
            p.Show = _product.Show;
            p.SKU = _product.SKU;
            p.SupplierID = _product.SupplierID;
            p.TaxID = _product.TaxID;
            p.Unit = _product.Unit;
            p.UserID = _product.UserID;
            p.Warranty = _product.Warranty;
            return p;
        }
        #endregion
    }
}
