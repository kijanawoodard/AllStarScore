using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Admin.Infrastructure.AutoMapper;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Models;
using AllStarScore.Admin.ViewModels;

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
            var model = new CompetitionDetailsViewModel(competition);
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
            if (!ModelState.IsValid) return PartialView(command);

            try
            {
                var competion = new Competition();
                competion.Update(command);
                RavenSession.Store(competion);

                return PartialView("CreateSuccessful", command);
            }
            catch
            {
                return PartialView(command);
            }
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
