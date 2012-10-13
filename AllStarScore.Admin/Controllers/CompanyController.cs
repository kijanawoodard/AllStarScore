using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Admin.ViewModels;
using AllStarScore.Models;

namespace AllStarScore.Admin.Controllers
{
    public class CompanyController : RavenController
    {
		[HttpGet, ChildActionOnly, AllowAnonymous]
        public ActionResult Name()
        {
        	var company =
        		RavenSession.Load<Company>(CurrentCompanyId);

        	var model = new CompanyNameViewModel(company);
            return PartialView(model);
        }

    }
}
