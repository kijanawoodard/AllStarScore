using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Models;
using Newtonsoft.Json;

namespace AllStarScore.Scoring.Controllers
{
    public class SyncController : Controller
    {
        public ActionResult Import(string id)
        {
            var client = new WebClient();
            var data = client.DownloadString(id);
            var model = JsonConvert.DeserializeObject<ScoringImportData>(data);
            return new EmptyResult();
        }

    }
}
