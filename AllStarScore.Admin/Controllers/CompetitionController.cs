using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Admin.Models;
using AllStarScore.Admin.ViewModels;

namespace AllStarScore.Admin.Controllers
{
    public class CompetitionController : RavenController
    {
        //
        // GET: /Competition/

        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult List()
        {
            var competitions = RavenSession.Query<Competition>()
                                .ToList();

            var model = new CompetitionList(competitions);
            return PartialView(model);
        }
        //
        // GET: /Competition/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Competition/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Competition/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
