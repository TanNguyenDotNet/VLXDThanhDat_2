﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MVCProject.Models;
using MVCProject.Common;
using System.Data.Entity;
using MVCProject.Models.AccessData;
using PagedList;

namespace MVCProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        Models.aspnetEntities db = new Models.aspnetEntities();
        public ActionResult AccessDenied(string id)
        {
            return View();
        }
        public async Task<ActionResult> RestPassword(string id)
        {
            if (!Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "26"))
                return RedirectToAction("AccessDenied", "Account");
            var user = db.AspNetUsers.Where(a => a.UserName == id).FirstOrDefault();
            IdentityResult result = await UserManager.RemovePasswordAsync(user.Id);
            IdentityResult resultChange = await UserManager.AddPasswordAsync(user.Id, DateTime.Now.ToString("ddMMyyyy"));
            return RedirectToAction("Index");
        }
        public ActionResult ActiveAccount(string id)
        {
            if (!Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "13"))
                return RedirectToAction("AccessDenied", "Account");
            var user = db.AppNetUserTypes.Where(a => a.UserOfName == id).FirstOrDefault();
            user.IsActive = user.IsActive == true ? false : true;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Agent(string id)
        {
            if (!Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;

            string enu = Security.EncryptString("User:" + id + "~FrontendUser", false, EncryptType.TripleDES);
            var u = db.AppNetUserTypes.Find(enu);
            if (u == null)
            {
                Response.Redirect("~/Account/Index/?Err=0");
                return null;
            }
            ViewBag.LocationList = Commons.GetLocationList(db);
            ViewBag.LocationSubList = Params.listItemLocationSub;
            ViewData["UserName"] = id;
            return View(u);
        }

        [HttpPost]
        public ActionResult Agent([Bind(Include = "Username,Email,Fax,Address,Phone,UserType,DateCreate,Expire,LocationID,LocationSubID," +
            "District,State,TaxID,DisplayName,UserOfName,UserOfType,IsDel,IsActive")] AppNetUserType appnetusertype)
        {
            if (!Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;

            if (ModelState.IsValid)
            {
                appnetusertype.Expire = DateTime.Now.AddYears(1).ToString("yyyyMMddHHmm");
                appnetusertype.State = "1";
                db.Entry(appnetusertype).State = EntityState.Modified;
                db.SaveChanges();
                Response.Redirect("~/Account/Index");
            }
            else
                Response.Redirect("~/Account/Index/?Err=1");
            return null;
        }

        [HttpPost]
        public ActionResult Roles(string UserName, string EncrytUser, string Password, string[] Roles)
        {
            if (!Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;

            if (Password != null && Password != "")
            {
                UserManager.AddPasswordAsync(UserName, Password);
            }

            var uroles = db.UserRoles.Where(c => c.UserName == EncrytUser);
            if (uroles != null && uroles.Count() > 0)
                foreach (var item in uroles)
                    db.UserRoles.Remove(item);
            db.SaveChanges();

            foreach (string s in Roles)
            {
                UserRole u = new UserRole() { RoleId = s, UserName = EncrytUser };
                db.UserRoles.Add(u);
            }
            db.SaveChanges();

            return RedirectToAction("Index", "Account");
        }

        public ActionResult Roles(string id)
        {
            if (!Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "14"))
                return RedirectToAction("AccessDenied", "Account");

            return View(GetFormInfo(id));
        }

        private RegisterViewModel GetFormInfo(string id)
        {
            string enu = Security.EncryptString("User:" + id + "~BackendUser", false, EncryptType.TripleDES);
            var u = db.AppNetUserTypes.Find(enu);
            if (u == null)
            {
                Response.Redirect("~/Account/Index/?Err=0");
                return null;
            }

            ViewData["UserName"] = id;
            ViewData["EncrytUser"] = enu;
            ViewBag.RoleList = db.AspNetRoles.OrderBy(o => o.GroupName).ToList();
            ViewBag.UserRoles = GetRoles(enu);
            ViewBag.LocationList = Commons.GetLocationList(db);
            ViewBag.LocationSubList = Params.listItemLocationSub;

            Models.RegisterViewModel model = new RegisterViewModel();
            model.LocationID = u.LocationID.ToString();
            return model;
        }

        private List<string> GetRoles(string enu)
        {
            List<string> us = new List<string>();
            var roles = db.UserRoles.Where(c => c.UserName == enu);
            if (roles != null && roles.Count() > 0)
            {
                foreach (var i in roles)
                {
                    us.Add(i.RoleId);
                }
            }
            return us;
        }

        public ActionResult Index(string page = "", string size = "", string filter = "", string LocalID = "", string type = "")
        {
            if (!Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "19"))
                return RedirectToAction("AccessDenied", "Account");

            ViewBag.LocationList = Params.listLocation;
            return View(AAppNetUserType.Instance.GetUserListPaging(User.Identity.GetUserName(), page, size, filter, LocalID, type));
        }
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    string en = Security.EncryptString("User:" + model.UserName + "~BackendUser", false, EncryptType.TripleDES);
                    var db = new Models.aspnetEntities();
                    var l = db.AppNetUserTypes.Where(d => d.UserOfName == user.UserName).FirstOrDefault();
                    returnUrl = "~/Product/Index";
                    if (l.UserOfType == "FrontendUser")
                    {
                        if (l.IsActive == false)
                        {
                            ModelState.AddModelError("", "Tài khoản chưa được kích hoạt");

                        }
                        else
                        { returnUrl = "~/Product/Home"; await SignInAsync(user, model.RememberMe); return RedirectToLocal(returnUrl); }
                    }
                    else
                    { await SignInAsync(user, model.RememberMe); return RedirectToLocal(returnUrl); }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (!Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "12"))
                return RedirectToAction("AccessDenied", "Account");

            ViewBag.LocationList = Commons.GetLocationList(db);
            ViewBag.LocationSubList = Params.listItemLocationSub;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "12"))
                return RedirectToAction("AccessDenied", "Account");

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    string enu = Security.EncryptString("User:" + model.UserName + "~" +
                        model.UserType, false, EncryptType.TripleDES);
                    string en = Security.EncryptString(model.UserName + "~" +
                        model.UserType, false, EncryptType.TripleDES);
                    int lid = int.Parse(model.LocationID == null || model.LocationID == "" ? "0" : model.LocationID);

                    Models.AppNetUserType ut = new AppNetUserType
                    {
                        Username = enu,
                        UserType = en,
                        Email = "",
                        Address = "",
                        Phone = "",
                        DateCreate = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        Expire = DateTime.Now.AddDays(365).ToString("yyyyMMddHHmmss"),
                        LocationID = lid,
                        LocationSubID = int.Parse(model.LocationSubID),
                        State = "0",
                        TaxID = "0",
                        DisplayName = model.FullName,
                        District = "",
                        Fax = "",
                        IsActive = false,
                        IsDel = false,
                        UserOfName = model.UserName,
                        UserOfType = model.UserType
                    };

                    db.AppNetUserTypes.Add(ut);
                    db.SaveChanges();

                    //await SignInAsync(user, isPersistent: false);
                    //return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                    return RedirectToLocal("~/Account/Index?msg=" + "Tài khoản " + model.UserName + " đã tồn tại");//result.Errors.ToList()[0]);
                }
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Index", "Account");
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            if (!Request.IsAuthenticated)
                Response.Redirect("~/Account/Login");

            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.RemoveAll();
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}