using System.Web.Mvc;
using AllStarScore.Admin.ViewModels;
using AllStarScore.Models;
using AllStarScore.Models.Commands;

namespace AllStarScore.Admin.Controllers
{
    public class SchedulingController : RavenController
    {
		[HttpGet]
        public ActionResult Edit(string id)
        {
            var competition =
                RavenSession
                    .Load<Competition>(id);

			var model = new SchedulingEditViewModel(competition);
            return View(model);
        }

		[HttpGet]
		public ActionResult Print(string id)
		{
			var competition =
				RavenSession
					.Load<Competition>(id);

			var model = new SchedulingEditViewModel(competition);
			return View(model);
		}

        [HttpPost]
        public JsonDotNetResult Edit(SchedulingEditCommand command)
        {
            return Execute(
                action: () =>
                {
                    var schedule = RavenSession.Load<Schedule>(command.Id);
                    schedule.Update(command);

                    RavenSession.SaveChanges();

                    return new JsonDotNetResult(true);
                });
        }
    }
}
