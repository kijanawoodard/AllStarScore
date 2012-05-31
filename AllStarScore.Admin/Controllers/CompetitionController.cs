using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Infrastructure.Utilities;
using AllStarScore.Admin.Models;
using AllStarScore.Admin.ViewModels;
using Raven.Client.Linq;

namespace AllStarScore.Admin.Controllers
{
    public class CompetitionController : RavenController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListHarness()
        {
            return PartialView();
        }
        public ActionResult List()
        {
            var competitions = RavenSession
                                .Query<Competition>()
                                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                                .ToList();

            var model = new CompetitionListViewModel(competitions);
            return PartialView(model);
        }

        public ActionResult Details(int id)
        {
            var competition = RavenSession.Load<Competition>(id);

            var identifier = "competitions/" + id;
            var stats =
                RavenSession
                    .Query<TeamRegistration, TeamRegistrationByGym>()
                    .Where(x => x.CompetitionId == identifier)
                    .As<TeamRegistrationByGym.Results>()
                    .ToList();

            var model = new CompetitionDetailsViewModel(competition, stats);
            return View(model);
        }

        public ActionResult CreateHarness()
        {
            return PartialView();
        }
        public ActionResult Create()
        {
            var model = new CompetitionCreateCommand();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Create(CompetitionCreateCommand command)
        {
            return Execute(
                action: () => {
                                var competion = new Competition();
                                competion.Update(command);
                                RavenSession.Store(competion);
                              },
                onsuccess: () => PartialView("CreateSuccessful", command),
                onfailure: () => PartialView(command));
        }

        //
        // GET: /Competition/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Competition/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Competition/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Competition/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
