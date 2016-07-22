// This file is part of the MS.Gamification project
// 
// File: MissionController.cs  Created: 2016-07-09@20:14
// Last modified: 2016-07-22@09:45

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic;
using MS.Gamification.GameLogic.QuerySpecifications;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;

namespace MS.Gamification.Controllers
    {
    public class MissionController : RequiresAuthorization
        {
        private readonly ICurrentUser requestingUser;
        private readonly GameRulesService rules;
        private readonly IUnitOfWork uow;

        public MissionController(IUnitOfWork uow, ICurrentUser user, GameRulesService rules)
            {
            this.uow = uow;
            requestingUser = user;
            this.rules = rules;
            }

        // GET Mission/Progress/1
        public ActionResult Progress(int id)
            {
            // Validate the parameters and fetch the Mission from the database
            var query = new MissionLevelProgress(id);
            var maybeMission = uow.Missions.GetMaybe(query);
            if (maybeMission.None)
                return HttpNotFound("The specified Mission ID was not found");
            var mission = maybeMission.Single();
            var missionModel = new MissionProgressViewModel {MissionTitle = mission.Title};
            foreach (var missionLevel in mission.MissionLevels)
                {
                var levelModel = new LevelProgressViewModel
                    {
                    Level = missionLevel.Level,
                    Unlocked = true // ToDo - evaluate preconditions
                    };
                levelModel.Level = missionLevel.Level;
                levelModel.Tracks = new List<MissionTrack>(missionLevel.Tracks);
                var challengeSpecification = new ChallengesInMissionLevel(mission.Id);
                var challengesInMission = uow.Challenges.AllSatisfying(challengeSpecification);
                // Only count one observation towards each challenge, for the purposes of computing progress.
                var eligibleObservationsForMission = new EligibleObservationsForChallenges(challengesInMission,
                    requestingUser.UniqueId);
                var observationsForMission = uow.Observations.AllSatisfying(eligibleObservationsForMission);
                var missionTotalChallenges = 0;
                missionLevel.Tracks.ForEach(p => missionTotalChallenges += p.Challenges.Count);
                levelModel.OverallProgressPercent = rules.ComputePercentComplete(challengesInMission, observationsForMission);
                // Compute individual track progress
                var trackPercentComplete = new List<int>(levelModel.Tracks.Count());
                foreach (var track in levelModel.Tracks)
                    {
                    var challengesInTrack = challengesInMission.Where(p => p.MissionTrackId == track.Id).ToList();
                    var eligibleObservationsForTrack = new EligibleObservationsForChallenges(challengesInTrack,
                        requestingUser.UniqueId);
                    var observations = uow.Observations.AllSatisfying(eligibleObservationsForTrack);
                    trackPercentComplete.Add(rules.ComputePercentComplete(challengesInTrack, observations));
                    }
                levelModel.TrackProgress = trackPercentComplete;
                missionModel.Levels.Add(levelModel);
                }
            return View(missionModel);
            }
        }
    }