using System.Linq;
using System.Web.Mvc;
using AllStarScore.Admin.ViewModels;
using AllStarScore.Library.RavenDB;
using AllStarScore.Models;

namespace AllStarScore.Admin.Controllers
{
	public class CompetitionDataController : RavenController
	{
		public ActionResult Index(string id)
		{
			var competition =
				RavenSession
					.Advanced.Lazily
					.Load<Competition>(id);

			var schedule =
				RavenSession
					.Advanced.Lazily
					.Load<Schedule>(Schedule.FormatId(id));

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
					.LoadStartingWith<Registration>(Registration.FormatId(id))
					.ToList();

			var performances =
				registrations
					.SelectMany(x => x.GetPerformances(competition.Value))
					.ToList();

			var model = new CompetitionDataIndexViewModel(schedule.Value, competition.Value, levels, divisions, gyms, registrations, performances);
			return PartialView(model);
		}
	}
}