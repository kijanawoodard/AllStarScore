using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AllStarScore.Scoring.Controllers
{
    public class AccountController : Controller
    {
    	public static readonly string Tabulator = "Tabulator";

		[HttpGet, AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

		[HttpPost, AllowAnonymous]
		public ActionResult Login(string username, string returnurl)
		{
			FormsAuthentication.SetAuthCookie(username, true);
			if (string.IsNullOrWhiteSpace(returnurl))
				return RedirectToAction("Index", "Landing");

			return Redirect(returnurl);
		}

		public ActionResult LogOff()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Login", "Account");
		}
    }
}
