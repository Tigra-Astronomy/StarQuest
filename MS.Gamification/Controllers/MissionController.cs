// This file is part of the MS.Gamification project
// 
// File: MissionController.cs  Created: 2016-07-01@00:05
// Last modified: 2016-07-04@00:34

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MS.Gamification.BusinessLogic.QuerySpecifications;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;

namespace MS.Gamification.Controllers
    {
    public class MissionController : UserController
        {
        private readonly ICurrentUser requestingUser;
        private readonly IUnitOfWork uow;

        public MissionController(IUnitOfWork uow, ICurrentUser user)
            {
            this.uow = uow;
            requestingUser = user;
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
            // Get the logged in requestingUser and fetch the requestingUser's Observation log from the database
            var specification = new ObservationsForUserMission(requestingUser.UniqueId, mission.Id);
            var userObservations = uow.Observations.AllSatisfying(specification);
            var missionTotalChallenges = 0;
            mission.Tracks.ForEach(p => missionTotalChallenges += p.Challenges.Count);
            // Take care to avoid divide-by-zero error if there are no challenges.
            var percentComplete = missionTotalChallenges > 0
                ? userObservations.Count() * 100 / missionTotalChallenges
                : 0;
            if (percentComplete > 100) percentComplete = 100;
            model.OverallProgressPercent = percentComplete;
            return View(model);
            }
        }
    }