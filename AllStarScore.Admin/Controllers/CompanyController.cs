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
		[ChildActionOnly, AllowAnonymous]
        public ActionResult Name()
		{
			var company = new Company() {Name = "Setup Phase"}; //This is only here for setting up a new company

			if (!string.IsNullOrWhiteSpace(CurrentCompanyId))
        		company = 
					RavenSession.Load<Company>(CurrentCompanyId);

        	var model = new CompanyNameViewModel(company);
            return PartialView(model);
        }

    }
}
