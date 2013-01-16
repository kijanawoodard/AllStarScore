using System.Linq;
using System.Web.Mvc;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.ViewModels;
using AllStarScore.Models;
using AllStarScore.Models.Commands;
using Raven.Client.Linq;

namespace AllStarScore.Admin.Controllers
{
    public class RegistrationController : RavenController
    {
        public ActionResult Index(RegistrationIndexRequestModel request)
        {
            var competition =
                RavenSession
                    .Load<Competition>(request.CompetitionId);

            var model = new RegistrationIndexViewModel(competition, request.GymId);
            return View(model);
        }

        [HttpGet]
        public ActionResult Teams(string competitionId, string gymId)
        {
            var divisions =
                RavenSession
                    .Query<Division, DivisionsWithLevels>()
                    .Where(x => x.CompanyId == CurrentCompanyId)
                    .Take(int.MaxValue) //there shouldn't be very many of these in practice
                    .As<DivisionViewModel>()
                    .Lazily();

            var teams =
                RavenSession
                    .Query<Registration>()
                    .Where(t => t.CompetitionId == competitionId)
                    .Where(t => t.GymId == gymId)
                    .Select(
                    t =>
                    new TeamRegistrationViewModel()
                    {
                        Id = t.Id,
                        DivisionId = t.DivisionId,
                        TeamName = t.TeamName,
                        ParticipantCount = t.ParticipantCount,
                        IsShowTeam = t.IsShowTeam,
						IsWorldsTeam = t.IsWorldsTeam
                    })
                    .Take(int.MaxValue) //there shouldn't be very many of these in practice
                    .ToList();

            var model = new RegistrationTeamsViewModel(competitionId, gymId);
            model.Teams = teams;
            model.Divisions = divisions.Value.ToList();

            return PartialView(model);
        }

        [HttpPost]
        public JsonDotNetResult Create(RegistrationCreateCommand command)
        {
            return Execute(
                action: () =>
                            {
                                var registration = new Registration();
                                registration.Update(command);

                                RavenSession.Store(registration);
                                RavenSession.SaveChanges();
                                return new JsonDotNetResult(registration);
                            });
        }

        [HttpPost]
        public JsonDotNetResult Edit(RegistrationEditCommand command)
        {
            return Execute(
                action: () =>
                {
                    var registration = RavenSession.Load<Registration>(command.Id);
                    registration.Update(command); //fyi; the registration has Id instead of RegistrationId, but we're not using this class on the client

                    RavenSession.SaveChanges();
                    return new JsonDotNetResult(true); 
                });
        }

		[HttpPost]
		public JsonDotNetResult Delete(RegistrationDeleteCommand command)
		{
			return Execute(
				action: () =>
				{
					RavenSession.Advanced.DocumentStore.DatabaseCommands.Delete(command.Id, null);
					return new JsonDotNetResult(true);
				});
		}
    }
}