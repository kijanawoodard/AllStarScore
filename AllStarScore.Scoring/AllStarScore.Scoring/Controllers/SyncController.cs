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
    public class SyncController : RavenController
    {
        public ActionResult Import(string id)
        {
            var client = new WebClient();
            var data = client.DownloadString(id);
            var model = JsonConvert.DeserializeObject<ScoringImportData>(data);

            UpdateCompetition(model);
            UpdatePerformances(model);

            return RedirectToAction("Index", "Performance", new {id = model.CompetitionId});
        }

        public void UpdateCompetition(ScoringImportData model)
        {
            var competition = RavenSession.Load<Competition>(model.CompetitionId);

            if (competition == null)
            {
                competition = new Competition()
                              {
                                  Id = model.CompetitionId
                              };
                RavenSession.Store(competition);
            }

            competition.CompanyName = model.CompanyName;
            competition.CompanyName = model.CompetitionName;
            competition.CompetitionDescription = model.CompetitionDescription;
            competition.Days = model.Days;
        }

        public void UpdatePerformances(ScoringImportData model)
        {
            var performances =
                RavenSession
                    .Query<Performance>()
                    .Take(int.MaxValue) //not expecting more than 100s, but likely slighly more than 128
                    .ToList();

            foreach (var performance in performances)
            {
                RavenSession.Delete(performance);
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
