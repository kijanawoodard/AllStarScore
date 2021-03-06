﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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

	public static class SecurityExtensions
	{
		public static bool IsTabulator(this IPrincipal target)
		{
			return target.Identity.Name == AccountController.Tabulator;
		}

		public static bool IsJudge(this IPrincipal target)
		{
			return !target.IsTabulator();
		}

		public static bool CanActAsJudge(this IPrincipal target, string judgeid)
		{
			return target.IsTabulator() || target.Identity.Name.EndsWith(judgeid);
		}
	}

	public class SecurityContext
	{
		public bool IsTabulator { get; private set; }
		public bool IsJudge { get; private set; }

		public string Id { get; private set; }
		public string Panel { get; private set; }
		public string JudgeId { get; private set; }
		public SecurityContext()
		{
			var user = HttpContext.Current.User;
			IsTabulator = user.IsTabulator();
			IsJudge = user.IsJudge();
			Id = user.Identity.Name;
			Panel = IsJudge ? Id.Substring(0, 1) : "";
			JudgeId = IsJudge ? Id.Substring(1, 1) : "";
		}
	}
}
