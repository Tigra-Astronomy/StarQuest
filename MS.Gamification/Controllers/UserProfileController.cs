// This file is part of the MS.Gamification project
// 
// File: UserProfileController.cs  Created: 2016-07-29@15:35
// Last modified: 2016-08-20@00:01

using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using MS.Gamification.BusinessLogic.Gamification;
using MS.Gamification.BusinessLogic.Gamification.QuerySpecifications;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using MS.Gamification.ViewModels.Mission;
using MS.Gamification.ViewModels.UserProfile;

namespace MS.Gamification.Controllers
    {
    public class UserProfileController : RequiresAuthorization
        {
        private readonly IGameEngineService gameEngine;
        private readonly IMapper mapper;
        private readonly IUnitOfWork uow;
        private readonly ICurrentUser user;

        public UserProfileController(ICurrentUser user, IUnitOfWork uow, IMapper mapper, IGameEngineService gameEngine)
            {
            this.user = user;
            this.uow = uow;
            this.mapper = mapper;
            this.gameEngine = gameEngine;
            }

        // GET: UserProfile
        public ActionResult Index(int showBadges = 4, int showObservations = 10)
            {
            var specification = new SingleUserWithProfileInformation(user.UniqueId);
            var maybeUser = uow.Users.GetMaybe(specification);
            if (maybeUser.None)
                return HttpNotFound("User not found in the database");
            var appUser = maybeUser.Single();
            var badges = appUser.Badges.Take(showBadges).Select(p => p.ImageIdentifier);
            var missionSpec = new MissionProgressSummary();
            var missions = uow.Missions.AllSatisfying(missionSpec);
            var missionViewModel = missions.Select(p => mapper.Map<Mission, MissionProgressViewModel>(p)).ToList();
            var model = new UserProfileViewModel
                {
                UserId = user.UniqueId,
                UserName = user.DisplayName,
                EmailAddress = user.LoginName,
                Titles = Enumerable.Empty<string>(), //ToDo: coming soon...
                Badges = badges,
                Observations = appUser.Observations
                    .Take(showObservations)
                    .Select(p => new ObservationSummaryViewModel
                        {
                        DateTimeUtc = p.ObservationDateTimeUtc,
                        ChallengeTitle = p.Challenge.Name
                        }),
                Missions = missionViewModel
                };

            var allChallenges = uow.Challenges.GetAll();
            foreach (var mission in model.Missions)
                {
                foreach (var level in mission.Levels)
                    {
                    var challengesForLevel = allChallenges.Where(p => p.MissionTrack.MissionLevelId == level.Id);
                    var observationSpec = new EligibleObservationsForChallenges(challengesForLevel, user.UniqueId);
                    var eligibleObservations = uow.Observations.AllSatisfying(observationSpec);
                    level.OverallProgressPercent = gameEngine.ComputePercentComplete(challengesForLevel, eligibleObservations);
                    }
                }
            return View(model);
            }
        }
    }