using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Controllers
{
    public class TutorialController : Controller
    {
        // GET: Tutorial
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Order()
        {
            return View();
        }
        public ActionResult ViewPDFOrder()
        {
            
            return File("~/File/HDSD.pdf", "application/pdf");
        }
    }
}