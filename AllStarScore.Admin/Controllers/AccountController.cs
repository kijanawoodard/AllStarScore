﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AllStarScore.Admin.Models;
using AllStarScore.Models.Commands;

namespace AllStarScore.Admin.Controllers
{
    public class AccountController : RavenController
    {
        public static readonly string Administrator = "administrator";
        public static readonly string AdministratorCompany = "allstarscore";
        //
        // GET: /Account/Login

        [HttpGet, AllowAnonymous]
        public ActionResult Login()
        {
            var model = new LoginModel();
            model.ReturnUrl = Request.QueryString["returnUrl"];

            return View(model);
            //return ContextDependentView();
        }

        //
        // POST: /Account/JsonLogin

        [AllowAnonymous]
        [HttpPost]
        public JsonResult JsonLogin(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = RavenSession.Query<User>().FirstOrDefault(u => u.Name == model.UserName);

                if (user != null && user.ValidatePassword(model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    return Json(new { success = true, redirect = returnUrl });
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }

        //
        // POST: /Account/Login

		private User GetAdmin()
		{
			return GetUser(Administrator, AdministratorCompany);
		}

		private User GetUser(string name, string company)
		{
			if (name == Administrator)
			{
				name = Administrator;
				company = AdministratorCompany;
			}

			var unique =
				RavenSession
					.Include<UniqueUser>(x => x.UserId)
					.Load<UniqueUser>(UniqueUser.GenerateUniqueName(company, name));

			var user =
				RavenSession.Load<User>(unique.UserId);

			return user;
		}

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = GetUser(model.UserName, model.CommandCompanyId);

                if (user != null && user.ValidatePassword(model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Competition");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Competition");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return ContextDependentView();
        }

        //
        // POST: /Account/JsonRegister

        [AllowAnonymous]
        [HttpPost]
        public ActionResult JsonRegister(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, passwordQuestion: null, passwordAnswer: null, isApproved: true, providerUserKey: null, status: out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, createPersistentCookie: false);
                    return Json(new { success = true });
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }

        //
        // POST: /Account/Register

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
				var command = new UserCreateCommand
				{
					Email = model.Email,
					UserName = model.UserName,
					Password = model.Password,
					CommandCompanyId = model.CommandCompanyId,
					CommandByUser = model.CommandByUser,
					CommandWhen = model.CommandWhen
				};

				var user = new User();
				user.Update(command);

				RavenSession.Advanced.UseOptimisticConcurrency = true;
				RavenSession.Store(user);
				RavenSession.Store(UniqueUser.FromUser(user));
				RavenSession.SaveChanges();
            	return RedirectToAction("Register");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
				var user = GetUser(User.Identity.Name, model.CommandCompanyId);

				if (user != null && user.ValidatePassword(model.OldPassword))
				{
					user.Update(model);
					RavenSession.SaveChanges();
					return RedirectToAction("ChangePasswordSuccess");
				}
            	
				ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

		public ActionResult SetPassword()
		{
			return View();
		}

		[HttpPost]
		public ActionResult SetPassword(SetPasswordModel model)
		{
			if (ModelState.IsValid)
			{
				var admin = GetAdmin();
				if (admin == null  || User.Identity.Name != Administrator || !admin.ValidatePassword(model.YourPassword))
				{
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
				}

				var user = GetUser(model.UserName, model.CommandCompanyId);

				if (user != null)
				{
					user.Update(model);
					RavenSession.SaveChanges();
					return RedirectToAction("ChangePasswordSuccess");
				}

				ModelState.AddModelError("", "The user name is incorrect or the new password is invalid.");
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        private ActionResult ContextDependentView()
        {
            string actionName = ControllerContext.RouteData.GetRequiredString("action");
            if (Request.QueryString["content"] != null)
            {
                ViewBag.FormAction = "Json" + actionName;
                return PartialView();
            }
            else
            {
                ViewBag.FormAction = actionName;
                return View();
            }
        }

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
