using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.ViewModels;
using AllStarScore.Library.RavenDB;
using AllStarScore.Models;
using Raven.Client.Linq;

namespace AllStarScore.Admin.Controllers
{
    public class SyncController : RavenController
    {
        [HttpGet]
        public ActionResult Index(string id)
        {
            var competition =
                RavenSession
                    .Load<Competition>(id);

            var model = new SyncIndexViewModel(competition);
            return View(model);
        }

		[HttpGet, AllowAnonymous]
        public ActionResult Download(DownloadRequestModel request)
		{
			var security =
				RavenSession
					.Load<Synchronization>(Synchronization.FormatId(CurrentCompanyId));

			var ok = security.Token.Equals(request.Token);
			if (!ok) return HttpNotFound();

			var company =
				RavenSession
					.Advanced.Lazily
					.Load<Company>(CurrentCompanyId);

			var competition =
				RavenSession
					.Advanced.Lazily
					.Load<Competition>(request.CompetitionId);

			var schedule =
				RavenSession
					.Advanced.Lazily
					.Load<Schedule>(Schedule.FormatId(request.CompetitionId));

			var levels =
				RavenSession
					.LoadStartingWith<Level>(Level.FormatId(CurrentCompanyId));

			var divisions =
				RavenSession
					.LoadStartingWith<Division>(Division.FormatId(CurrentCompanyId));

			var gyms =
				RavenSession
					.LoadStartingWith<Gym>(Gym.FormatId(CurrentCompanyId));

			var registrations =
				RavenSession
					.LoadStartingWith<Registration>(Registration.FormatId(request.CompetitionId));

            if (schedule == null)
                return new HttpNotFoundResult();

            var model = new CompetitionInfo
                        {
                            Company = company.Value,
                            Levels = levels.ToList(),
							Divisions = divisions.ToList(),
							Gyms = gyms.ToList(),
							Competition = competition.Value,
							Schedule = schedule.Value,
                            Registrations = registrations.ToList(),
                        };
            

            return new JsonDotNetResult(model);
        }

        public class DownloadRequestModel
        {
            public string CompetitionId { get; set; }
            public string Token { get; set; }
        }
    }
}
