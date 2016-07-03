// This file is part of the MS.Gamification project
// 
// File: MissionController.cs  Created: 2016-07-01@00:05
// Last modified: 2016-07-03@00:58

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MS.Gamification.BusinessLogic.QuerySpecifications;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;

namespace MS.Gamification.Controllers
    {
    public class MissionController : UserController
        {
        private readonly IUnitOfWork uow;

        public MissionController(IUnitOfWork uow)
            {
            this.uow = uow;
            }

        // GET Mission/Level/1
        public ActionResult Level(int missionId, int? levelId)
            {
            // Validate the parameters and fetch the Mission from the database
            var model = new MissionProgressViewModel();
            model.Level = levelId ?? 1;
            var query = new MissionLevelProgress(missionId, model.Level);
            var maybeMission = uow.Missions.GetMaybe(query);
            if (maybeMission.None)
                return HttpNotFound("The specified Mission ID was not found");
            var mission = maybeMission.Single();
            model.Tracks = new List<MissionTrack>(mission.Tracks);
            // Get the logged in user and fetch the user's Observation log from the database
            var userId = User.Identity.GetUserId();
            var specification = new ObservationsForUserMission(userId, mission.Id);
            var userObservations = uow.Observations.AllSatisfying(specification);
            var missionTotalChallenges = 0;
            mission.Tracks.ForEach(p => missionTotalChallenges += p.Challenges.Count);
            var percentComplete = userObservations.Count() / missionTotalChallenges * 100;
            if (percentComplete > 100) percentComplete = 100;
            model.OverallProgressPercent = percentComplete;
            return View(model);
            }
        }
    }