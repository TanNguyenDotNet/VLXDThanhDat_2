using MVCProject.Common;
using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Controllers
{
    public class PaymentController : Controller
    {
        private retailEntities modelRetail = Params.ModelRetail;
        private aspnetEntities modelAspnet = Params.ModelaspnetEntities;
        // GET: Payment
        public ActionResult Index()
        {
            string userEnctypt = "";
            var listUser = modelAspnet.AspNetUsers.ToList();
            var listUserType = modelAspnet.AppNetUserTypes.ToList();
            List<string> lstType = new List<string>();
            List<string> lstUser = new List<string>();
            for (int i = 0; i < listUser.Count; i++)
            {
                userEnctypt = MVCProject.Common.Security.EncryptString("User:" + listUser[i].UserName + "~FrontendUser",
                            false, MVCProject.Common.EncryptType.TripleDES);
                if(listUserType.Where(a=>a.Username==userEnctypt).FirstOrDefault()!=null)
                {
                    lstType.Add(listUserType.Where(a => a.Username == userEnctypt).FirstOrDefault().Username);
                    lstUser.Add(listUser[i].Id);
                }
            }
            
            return View();
        }
    }
}