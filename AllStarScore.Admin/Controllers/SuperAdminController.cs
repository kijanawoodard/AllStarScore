using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AllStarScore.Admin.Models;
using AllStarScore.Admin.ViewModels;
using AllStarScore.Extensions;
using AllStarScore.Library;
using AllStarScore.Library.RavenDB;
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
            var model = new SuperAdminIndexViewModel();
            model.Domain = _tenants.GetKey(Request.Url);
            model.Company = _tenants.GetCompanyId(Request.Url);

        	var sync = 
				RavenSession.Load<Synchronization>(Synchronization.FormatId(CurrentCompanyId));
        	
			if (sync != null)
				model.Token = sync.Token;

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

					CheckSynchronization(company.Id);
                    HackLevels(company.Id);
                    HackDivisions(company.Id, command);
                },
                onsuccess: () => RedirectToAction("Index"));
        }

    	[HttpGet]
    	public ActionResult ResetLevels(ResetLevelsCommand command)
    	{
			HackLevels(CurrentCompanyId);
			HackDivisions(CurrentCompanyId, command);
    		return Content("Ok");
    	}

		[HttpGet]
		public ActionResult DeleteLevels(ResetLevelsCommand command)
		{
			if (command.CommandByUser != AccountController.Administrator)
				return Content("Yes");

			var levels =
				RavenSession
					.LoadStartingWith<Level>(Level.FormatId(CurrentCompanyId));

			levels.ToList().ForEach(RavenSession.Delete);
			RavenSession.SaveChanges();

			return Content("Ok");
		}

		[HttpGet]
		public ActionResult DeleteDivisions(ResetLevelsCommand command)
		{
			if (command.CommandByUser != AccountController.Administrator)
				return Content("Yes");

			var divisions =
				RavenSession
					.LoadStartingWith<Division>(Division.FormatId(CurrentCompanyId));

			divisions.ToList().ForEach(RavenSession.Delete);
			RavenSession.SaveChanges();

			return Content("Ok");
		}

        [HttpGet, AllowAnonymous]
        public ActionResult Init()
        {
            //should only need this once
            _tenants.EnsureTenantMapExists(); //no real home for this, but doesn't matter right now
            HackSecurity();
            
            return RedirectToAction("Index");
        }

		void CheckSynchronization(string companyid)
		{
			var doc = new Synchronization
			          {
			          	CompanyId = companyid,
			          	Token = ShortGuid.NewGuid(),
			          	Url = string.Empty
			          };

			doc.Token = doc.Token.Replace("-", "q"); //don't want hypens

			RavenSession.Store(doc);
			RavenSession.SaveChanges();
		}

        void HackSecurity()
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

        	RavenSession.Advanced.UseOptimisticConcurrency = true;
            RavenSession.Store(admin);
			RavenSession.Store(UniqueUser.FromUser(admin));
            RavenSession.SaveChanges();
        }

        void HackLevels(string companyId)
        {
        	var ok = RavenSession.Query<Level>().Any();
			if (ok) return;

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
                                 Id = "clubcheerbeginner",
                                 Name = "Club Cheer Beginner",
                                 ScoringDefinition = "scoring-level5"
                             },
							 new Level
                             {
                                 Id = "clubcheerintermediate",
                                 Name = "Club Cheer Intermediate",
                                 ScoringDefinition = "scoring-level5"
                             },
							 new Level
                             {
                                 Id = "clubcheeradvanced",
                                 Name = "Club Cheer Advanced",
                                 ScoringDefinition = "scoring-level5"
                             },
							 new Level
                             {
                                 Id = "gamerecbeginner",
                                 Name = "Game Rec Beginner",
                                 ScoringDefinition = "scoring-level5"
                             },
							 new Level
                             {
                                 Id = "gamerecintermediate",
                                 Name = "Game Rec Intermediate",
                                 ScoringDefinition = "scoring-level5"
                             },
							 new Level
                             {
                                 Id = "gamerecadvanced",
                                 Name = "Game Rec Advanced",
                                 ScoringDefinition = "scoring-level5"
                             },
							 new Level
                             {
                                 Id = "cheerprep",
                                 Name = "All Star Prep",
                                 ScoringDefinition = "scoring-level5"
                             },
                             new Level
                             {
                                 Id = "dance", 
                                 Name = "Dance", 
                                 ScoringDefinition = "scoring-dance"
                             },
							 new Level
                             {
                                 Id = "danceprep", 
                                 Name = "Dance Prep", 
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
                             },
                             new Level
                             {
                                 Id = "specialneeds",
                                 Name = "Special Needs",
                                 ScoringDefinition = "scoring-level6"
                             }
                         };

            levels.ForEach(x =>
            {
                x.Id = Level.FormatId(companyId) + x.Id;
                x.CompanyId = companyId;
            });

            levels.ForEach(RavenSession.Store);
            RavenSession.SaveChanges();
        }

		IEnumerable<DivisionCreateCommand> GenerateDivisionCommands(IEnumerable<string> sizes, IEnumerable<string> names, IEnumerable<string> levels)
		{
			var result = levels.Select(level => GenerateDivisionCommands(sizes, names, level)).SelectMany(x => x);
			return result;
		}

    	IEnumerable<DivisionCreateCommand> GenerateDivisionCommands(IEnumerable<string> sizes, IEnumerable<string> names, string levelId, string scoringDefinition = null)
		{
			var combos = from n in names
						 from s in sizes
						 select n + " " + s;

			var result = combos.Select(name => new DivisionCreateCommand {Name = name.Trim(), LevelId = levelId, ScoringDefinition = scoringDefinition});
			return result;
		}

        void HackDivisions(string companyId, ICommand src)
        {
			var ok = RavenSession.Query<Division>().Any();
			if (ok) return;
			
        	var commands = new List<DivisionCreateCommand>();
        	var sizes = new[] {"", "Small", "Large"};
			
			var names = new[] {"Tiny", "Mini", "Youth", "Junior", "Senior"};
        	var list = GenerateDivisionCommands(sizes, names, "1");
			commands.AddRange(list);

			names = new[] { "Mini", "Youth", "Junior", "Senior" };
			list = GenerateDivisionCommands(sizes, names, "2");
			commands.AddRange(list);

			names = new[] { "Youth", "Junior", "Senior", "Senior Coed", "Open" };
			list = GenerateDivisionCommands(sizes, names, "3");
			commands.AddRange(list);

			names = new[] { "Youth", "Junior", "Senior", "Senior Coed", "Open" };
			list = GenerateDivisionCommands(sizes, names, "4");
			commands.AddRange(list);

			names = new[] { "Senior 4.2" };
			list = GenerateDivisionCommands(sizes, names, "4", "scoring-division42");
			commands.AddRange(list);
			
			names = new[] { "Youth", "Junior", "Senior" };
			list = GenerateDivisionCommands(sizes, names, "5");
			commands.AddRange(list);

			names = new[] { "Senior Coed" };
			list = GenerateDivisionCommands(new[] { "", "Small", "Medium", "Large" }, names, "5");
			commands.AddRange(list);

			names = new[] { "International Open", "International Open Coed", "Worlds" };
			list = GenerateDivisionCommands(new[] { "" }, names, "5");
			commands.AddRange(list);

			names = new[] { "Senior Restricted" };
			list = GenerateDivisionCommands(new[] { "" }, names, "5", "scoring-restricted5" );
			commands.AddRange(list);

			names = new[] { "Open", "Open Coed" };
			list = GenerateDivisionCommands(new[] { "" }, names, "6");
			commands.AddRange(list);

			names = new[] { "Tiny", "Mini", "Youth", "Junior", "Senior" };
			var levels = new[] { "clubcheerbeginner", "clubcheerintermediate", "clubcheeradvanced",
								 "gamerecbeginner", "gamerecintermediate", "gamerecadvanced", 
								 "cheerprep"};
			list = GenerateDivisionCommands(new[] { "" }, names, levels);
			commands.AddRange(list);

			names = new[] { "Elementary School", "Junior High", "High School Novice", "High School Intermediate", "High School Advanced" };
			list = GenerateDivisionCommands(new[] { "" }, names, "school");
			commands.AddRange(list);

			var dances = new[] {"Jazz", "Pom", "Prop", "Novelty", "Ballet", "Lyrical", "Open", "Hip Hop", "Crew"};
			dances.ToList().ForEach(dance =>
        	{
				var scoringdef = "scoring-dance-" + dance.ToLower().Replace(" ", "");
				
				names = new[] { "Tiny", "Mini", "Youth", "Junior", "Senior", "Open" };
        		names = names.Select(name => dance + " " + name).ToArray();
				list = GenerateDivisionCommands(new[] { "", "Coed" }, names, "dance", scoringdef);
				commands.AddRange(list);

				names = new[] { "Tiny", "Mini", "Youth", "Junior" };
				names = names.Select(name => dance + " " + name).ToArray();
				list = GenerateDivisionCommands(new[] { "", "Coed" }, names, "danceprep", scoringdef);
				commands.AddRange(list);	
        	});

        	commands.ForEach(x =>
            {
                x.LevelId = Level.FormatId(companyId) + x.LevelId;
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
