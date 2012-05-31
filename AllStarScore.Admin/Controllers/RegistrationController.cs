﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Models;
using AllStarScore.Admin.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client.Linq;

namespace AllStarScore.Admin.Controllers
{
    public class RegistrationController : RavenController
    {
        public ActionResult Index(RegistrationIndexRequestModel request)
        {
            var competition = RavenSession
                                    .Load<Competition>(request.CompetitionId);

            var model = new RegistrationIndexViewModel(competition, request.GymId);
            return View(model);
        }

        [HttpGet]
        public ActionResult Teams(RegistrationTeamsViewModel model)
        {
            var divisions =
                RavenSession
                    .Query<Division, DivisionsWithLevels>()
                    .As<DivisionViewModel>()
                    .Lazily();

            var teams =
                RavenSession
                    .Query<TeamRegistration>()
                    .Where(t => t.CompetitionId == model.CompetitionId && t.GymId == model.GymId)
                    .Select(t => new TeamRegistrationViewModel(){ CompetitionId = t.CompetitionId, GymId = t.GymId, Id = t.Id, DivisionId = t.DivisionId, TeamName = t.TeamName, ParticipantCount = t.ParticipantCount, IsShowTeam = t.IsShowTeam})
                    .ToList();

            model.Teams = teams;
            model.Divisions = divisions.Value.ToList();

            return PartialView(model);
        }

        [HttpPost]
        public JsonDotNetResult Create(RegistrationCreateCommand command)
        {
            return Execute(() =>
                               {
                                   var registration = new TeamRegistration();
                                   registration.Update(command);

                                   RavenSession.Store(registration);
                                   return new JsonDotNetResult(registration);
                               });
        }

        [HttpGet]
        public ActionResult Register(RegistrationRegisterViewModel model)
        {
            return PartialView(model);
        }
    }
}