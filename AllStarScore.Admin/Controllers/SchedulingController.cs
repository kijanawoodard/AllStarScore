using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Admin.ViewModels;

namespace AllStarScore.Admin.Controllers
{
    public class SchedulingController : Controller
    {
        public ActionResult Edit(string id)
        {
            var model = new SchedulingEditViewModel();
            return View(model);
        }

    }
}
