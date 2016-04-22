using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Models;
using Microsoft.AspNet.Identity;
using MVCProject.Common;

namespace MVCProject.Controllers
{
    public class CatalogUnitController : Controller
    {
        aspnetEntities modelAspnet = Params.ModelaspnetEntities;
        // GET: CatalogUnit
        public ActionResult Index()
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, modelAspnet, User.Identity.GetUserName(), null))
                return RedirectToAction("AccessDenied", "Account");

            return View(modelAspnet.CatalogUnits.ToList());
        }
    }
}