// This file is part of the MS.Gamification project
// 
// File: MissionController.cs  Created: 2016-08-20@23:12
// Last modified: 2016-08-24@23:56

using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic;
using MS.Gamification.GameLogic.QuerySpecifications;
using MS.Gamification.Models;
using MS.Gamification.ViewModels.Mission;
using MS.Gamification.ViewModels.UserProfile;

namespace MS.Gamification.Controllers
    {
    public class MissionController : RequiresAuthorization
        {
        private readonly GameRulesService gameEngine;
        private readonly IMapper mapper;
        private readonly ICurrentUser requestingUser;
        private readonly IUnitOfWork uow;

        public MissionController(IUnitOfWork uow, ICurrentUser user, GameRulesService gameEngine, IMapper mapper)
            {
            this.uow = uow;
            requestingUser = user;
            this.gameEngine = gameEngine;
            this.mapper = mapper;
            }

        // GET Mission/Index
        public ActionResult Index()
            {
            var specification = new SingleUserWithProfileInformation(requestingUser.UniqueId);
            var maybeUser = uow.Users.GetMaybe(specification);
            if (maybeUser.None)
                return HttpNotFound("User not found in the database");
            var appUser = maybeUser.Single();
            var missionSpec = new MissionProgressSummary();
            var missions = uow.Missions.AllSatisfying(missionSpec);
            var missionViewModel = missions.Select(p => mapper.Map<Mission, MissionProgressViewModel>(p)).ToList();
            var model = new UserProfileViewModel
                {
                UserId = requestingUser.UniqueId,
                UserName = requestingUser.DisplayName,
                EmailAddress = requestingUser.LoginName,
                Titles = Enumerable.Empty<string>(), //ToDo: coming soon...
                Missions = missionViewModel
                };

            var allChallenges = uow.Challenges.GetAll();
            foreach (var mission in model.Missions)
                {
                foreach (var level in mission.Levels)
                    {
                    var challengesForLevel = allChallenges.Where(p => p.MissionTrack.MissionLevelId == level.Id);
                    var observationSpec = new EligibleObservationsForChallenges(challengesForLevel, requestingUser.UniqueId);
                    var eligibleObservations = uow.Observations.AllSatisfying(observationSpec);
                    level.OverallProgressPercent = gameEngine.ComputePercentComplete(challengesForLevel, eligibleObservations);
                    }
                }
            return View(model);
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
            // Compute the % complete for each level overall and its individual tracks
            foreach (var missionLevel in missionModel.Levels)
                {
                var challengeSpecification = new ChallengesInMissionLevel(missionLevel.Id);
                var challengesInLevel = uow.Challenges.AllSatisfying(challengeSpecification);
                missionLevel.Unlocked = gameEngine.IsLevelUnlockedForUser(missionLevel, requestingUser.UniqueId);
                // Only count one observation towards each challenge, for the purposes of computing progress.
                var eligibleObservationsForLevel = new EligibleObservationsForChallenges(challengesInLevel,
                    requestingUser.UniqueId);
                var observationsForLevel = uow.Observations.AllSatisfying(eligibleObservationsForLevel);
                missionLevel.OverallProgressPercent = gameEngine.ComputePercentComplete(challengesInLevel, observationsForLevel);
                // Compute individual track progress
                foreach (var track in missionLevel.Tracks)
                    {
                    var challengesInTrack = challengesInLevel.Where(p => p.MissionTrackId == track.Id).ToList();
                    var trackObservationsSpec = new EligibleObservationsForChallenges(challengesInTrack,
                        requestingUser.UniqueId);
                    var observationsForTrack = uow.Observations.AllSatisfying(trackObservationsSpec);
                    track.PercentComplete = gameEngine.ComputePercentComplete(challengesInTrack, observationsForTrack);
                    // Iterate through the challenges to determine whether or not each one is "complete"
                    foreach (var challenge in track.Challenges)
                        {
                        challenge.HasObservation = observationsForTrack.Any(p => p.ChallengeId == challenge.Id);
                        }
                    }
                }
            return View(missionModel);
            }

        // GET: Mission/ChallengeDetails/1
        public ActionResult ChallengeDetails(int id)
            {
            var challengeSpec = new SingleChallengeWithTrackAndCategory(id);
            var maybeChallenge = uow.Challenges.GetMaybe(challengeSpec);
            if (maybeChallenge.None)
                return HttpNotFound();
            var model = maybeChallenge.Single();
            return View(model);
            }
        }
    }