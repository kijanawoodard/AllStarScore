using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Models;
using AllStarScore.Scoring.Models;
using Newtonsoft.Json;

namespace AllStarScore.Scoring.Controllers
{
    public class SyncController : RavenController
    {
        public ActionResult Import(string id)
        {
            var client = new WebClient();
            var data = client.DownloadString(id);
            var model = JsonConvert.DeserializeObject<CompetitionInfo>(data);

            RavenSession.Store(model); 
            UpdatePerformances(model);

            return RedirectToAction("Index", "Performance", new {id = model.CompetitionId});
        }

        public void UpdatePerformances(CompetitionInfo model)
        {
            var performances =
                RavenSession
                    .Query<Performance>()
                    .Where(x => x.CompetitionId == model.CompetitionId)
                    .Take(int.MaxValue) //not expecting more than 100s, but likely slighly more than 128
                    .ToList();

            var scores =
                RavenSession
                    .Query<JudgeScore>()
                    .Where(x => x.CompetitionId == model.CompetitionId)
                    .Take(int.MaxValue) //TODO: page through these, could be more than 1024
                    .ToList();

            foreach (var performance in performances)
            {
                RavenSession.Delete(performance);
            }

            foreach (var score in scores)
            {
                RavenSession.Delete(score);
            }

            foreach (var performance in model.Performances)
            {
                RavenSession.Store(performance);
            }
        }
    }

    public class Competition
    {
        public string Id { get; set; }
        public string CompetitionId { get { return Id; } }

        public string CompanyName { get; set; } //denormalized here for events run on-behalf of another company
        public string CompetitionName { get; set; }
        public string CompetitionDescription { get; set; }
        public string CompetitionDisplay { get { return string.IsNullOrWhiteSpace(CompetitionDescription) ? CompetitionName : CompetitionDescription; } }
        public List<DateTime> Days { get; set; } 
    }
}
