using System.Diagnostics;
using System.Web.Mvc;
using AllStarScore.Admin.ViewModels;
using AllStarScore.Library;
using AllStarScore.Models;
using AllStarScore.Models.Commands;


namespace AllStarScore.Admin.Controllers
{
    public class SuperAdminController : RavenController
    {
        private readonly ITenantProvider _tenants;

        public SuperAdminController(ITenantProvider tenants)
        {
            _tenants = tenants;
        }

        [HttpGet]
        public ActionResult Index()
        {
            _tenants.EnsureTenantMapExists(); //no real home for this, but doesn't matter right now

            var model = new SuperAdminIndexViewModel();
            model.Domain = _tenants.GetKey(Request.Url);
            model.Company = _tenants.GetCompanyId(Request.Url);
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CompanyCreateCommand command)
        {
            return Execute(
                action: () =>
                {
                    var company = new Company();

                    RavenSession.Store(company);
                    RavenSession.SaveChanges();

                    _tenants.SetCompanyId(Request.Url, company.Id);
                },
                onsuccess: () => RedirectToAction("Index"));
        }
    }
}
