// This file is part of the MS.Gamification project
// 
// File: MissionController.cs  Created: 2016-07-09@20:14
// Last modified: 2016-07-28@16:46

using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic;
using MS.Gamification.GameLogic.QuerySpecifications;
using MS.Gamification.Models;
using MS.Gamification.ViewModels.Mission;

namespace MS.Gamification.Controllers
    {
    public class MissionController : RequiresAuthorization
        {
        private readonly IMapper mapper;
        private readonly ICurrentUser requestingUser;
        private readonly GameRulesService rules;
        private readonly IUnitOfWork uow;

        public MissionController(IUnitOfWork uow, ICurrentUser user, GameRulesService rules, IMapper mapper)
            {
            this.uow = uow;
            requestingUser = user;
            this.rules = rules;
            this.mapper = mapper;
            }

        // GET Mission/Progress/1
        public ActionResult Progress(int id)
            {
            //ToDo - this is not very 'clean' and really needs some TLC
            // Validate the parameters and fetch the Mission from the database
            var query = new MissionLevelProgress(id);
            var maybeMission = uow.Missions.GetMaybe(query);
            if (maybeMission.None)
                return HttpNotFound("The specified Mission ID was not found");
            var mission = maybeMission.Single();
            var missionModel = mapper.Map<Mission, MissionProgressViewModel>(mission);
            var challengeSpecification = new ChallengesInMissionLevel(mission.Id);
            var challengesInMission = uow.Challenges.AllSatisfying(challengeSpecification);
            // Compute the % complete for each level overall and its individual tracks
            foreach (var missionLevel in missionModel.Levels)
                {
                missionLevel.Unlocked = rules.IsLevelUnlockedForUser(missionLevel, requestingUser.UniqueId);
                // Only count one observation towards each challenge, for the purposes of computing progress.
                var eligibleObservationsForMission = new EligibleObservationsForChallenges(challengesInMission,
                    requestingUser.UniqueId);
                var observationsForMission = uow.Observations.AllSatisfying(eligibleObservationsForMission);
                var missionTotalChallenges = missionLevel.Tracks.Sum(track => track.Challenges.Count);
                missionLevel.OverallProgressPercent = rules.ComputePercentComplete(challengesInMission, observationsForMission);
                // Compute individual track progress
                //var trackPercentComplete = new List<int>(levelModel.Tracks.Count());
                foreach (var track in missionLevel.Tracks)
                    {
                    var challengesInTrack = challengesInMission.Where(p => p.MissionTrackId == track.Id).ToList();
                    var eligibleObservationsForTrack = new EligibleObservationsForChallenges(challengesInTrack,
                        requestingUser.UniqueId);
                    var observations = uow.Observations.AllSatisfying(eligibleObservationsForTrack);
                    track.PercentComplete = rules.ComputePercentComplete(challengesInTrack, observations);
                    // Iterate through the challenges to determine whether or not each one is "complete"
                    foreach (var challenge in track.Challenges)
                        {
                        challenge.HasObservation = observations.Any(p => p.ChallengeId == challenge.Id);
                        }
                    }
                }
            return View(missionModel);
            }
        }
    }