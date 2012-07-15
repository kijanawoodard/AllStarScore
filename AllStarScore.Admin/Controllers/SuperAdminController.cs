﻿using System.Collections.Generic;
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
            return Execute(
                action: () =>
                {
                    var company = new Company();

                    company.Update(command);
                    RavenSession.Store(company);
                    RavenSession.SaveChanges();

                    _tenants.SetCompanyId(Request.Url, company.Id);

                    HackSecurity(company.Id, command);
                    HackLevels(company.Id);
                    HackDivisions(company.Id, command);
                },
                onsuccess: () => RedirectToAction("Index"));
        }

        private void HackSecurity(string companyId, ICommand src)
        {
            var command = new UserCreateCommand
                          {
                              Email = "admin@wyldeye.com",
                              UserName = "administrator",
                              Password = "hello",
                              CommandCompanyId = companyId,
                              CommandByUser = src.CommandByUser,
                              CommandWhen = src.CommandWhen
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

            levels.ForEach(x => x.Id = companyId + "/level/" + x.Id);
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

            commands.ForEach(x => x.LevelId = companyId + "/level/" + x.LevelId);

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
