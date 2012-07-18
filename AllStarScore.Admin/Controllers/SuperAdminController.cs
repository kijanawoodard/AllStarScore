using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using AllStarScore.Admin.Models;
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
            return ExecuteInExplicitTransaction(
                action: () =>
                {
                    var company = new Company();

                    company.Update(command);
                    RavenSession.Store(company);
                    RavenSession.SaveChanges();

                    _tenants.SetCompanyId(Request.Url, company.Id);
                    RavenSession.SaveChanges();

                    HackLevels(company.Id);
                    HackDivisions(company.Id, command);
                },
                onsuccess: () => RedirectToAction("Index"));
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Init()
        {
            //should only need this once
            HackSecurity();
            return RedirectToAction("Index");
        }

        private void HackSecurity()
        {
            var any = RavenSession.Query<User>().Any();
            if (any) return;

            var command = new UserCreateCommand
                          {
                              Email = "admin@wyldeye.com",
                              UserName = AccountController.Administrator,
                              Password = "hello",
                              CommandCompanyId = AccountController.AdministratorCompany,
                              CommandByUser = "system",
                              CommandWhen = DateTime.UtcNow
                          };

            var admin = new User();
            admin.Update(command);

            RavenSession.Store(admin);
            RavenSession.SaveChanges();
        }

        private void HackLevels(string companyId)
        {
            var levels = new List<Level>()
                         {
                             new Level
                             {
                                 Id = "1",
                                 Name = "All-Star Level 1",
                                 ScoringDefinition = "scoring-level1"
                             },
                             new Level
                             {
                                 Id = "2",
                                 Name = "All-Star Level 2",
                                 ScoringDefinition = "scoring-level2"
                             },
                             new Level
                             {
                                 Id = "3",
                                 Name = "All-Star Level 3",
                                 ScoringDefinition = "scoring-level3"
                             },
                             new Level
                             {
                                 Id = "4",
                                 Name = "All-Star Level 4",
                                 ScoringDefinition = "scoring-level4"
                             },
                             new Level
                             {
                                 Id = "5",
                                 Name = "All-Star Level 5",
                                 ScoringDefinition = "scoring-level5"
                             },
                             new Level
                             {
                                 Id = "6",
                                 Name = "All-Star Level 6",
                                 ScoringDefinition = "scoring-level6"
                             },
                             new Level
                             {
                                 Id = "dance", 
                                 Name = "Dance", 
                                 ScoringDefinition = "scoring-dance"
                             },
                             new Level  
                             {
                                 Id = "school", 
                                 Name = "School", 
                                 ScoringDefinition = "scoring-school"
                             },
                             new Level
                             {
                                 Id = "individual",
                                 Name = "Individual",
                                 ScoringDefinition = "scoring-individual"
                             }
                         };

            levels.ForEach(x =>
            {
                x.Id = companyId + "/level/" + x.Id;
                x.CompanyId = companyId;
            });

            levels.ForEach(RavenSession.Store);
            RavenSession.SaveChanges();
        }

        private void HackDivisions(string companyId, ICommand src)
        {
            var commands = new List<DivisionCreateCommand>()
                            {
                                new DivisionCreateCommand {Name = "Small Youth", LevelId = "1"},
                                new DivisionCreateCommand {Name = "Large Youth", LevelId = "2"},
                                new DivisionCreateCommand {Name = "Small Junior", LevelId = "3"},
                                new DivisionCreateCommand {Name = "Large Junior", LevelId = "4"},
                                new DivisionCreateCommand {Name = "Small Senior", LevelId = "5"},
                                new DivisionCreateCommand {Name = "Large Senior", LevelId = "6"}
                            };

            commands.ForEach(x =>
            {
                x.LevelId = companyId + "/level/" + x.LevelId;
                x.CommandCompanyId = companyId;
                x.CommandByUser = src.CommandByUser;
                x.CommandWhen = src.CommandWhen;

            });

            var divisions =
                commands
                    .Select(command =>
                    {
                        var division = new Division();
                        division.Update(command);
                        return division;
                    })
                    .ToList();

            divisions.ForEach(RavenSession.Store);
            RavenSession.SaveChanges();
        }
    }
}
