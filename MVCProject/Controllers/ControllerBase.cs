using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Routing;

namespace MVCProject.Controllers
{
    public class ControllerBase : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Common.Commons.CheckLogin(requestContext.HttpContext.Request, requestContext.HttpContext.Response, User.Identity.GetUserName());

        }
    }
}