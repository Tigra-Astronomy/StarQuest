// This file is part of the MS.Gamification project
// 
// File: UserProfileController.cs  Created: 2016-07-29@15:35
// Last modified: 2016-07-29@17:03

using System.Linq;
using System.Web.Mvc;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic.QuerySpecifications;
using MS.Gamification.ViewModels.UserProfile;

namespace MS.Gamification.Controllers
    {
    public class UserProfileController : RequiresAuthorization
        {
        private readonly IUnitOfWork uow;
        private readonly ICurrentUser user;

        public UserProfileController(ICurrentUser user, IUnitOfWork uow)
            {
            this.user = user;
            this.uow = uow;
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
            var model = new UserProfileViewModel
                {
                UserName = user.DisplayName,
                EmailAddress = user.LoginName,
                Titles = new[] {"Big Cheese", "Top Cat", "Legendary Monkton Stargazer", "Teacher's Pet"},
                Badges = badges,
                Observations = appUser.Observations
                    .Take(showObservations)
                    .Select(p => new ObservationSummaryViewModel
                        {
                        DateTimeUtc = p.ObservationDateTimeUtc,
                        ChallengeTitle = p.Challenge.Name
                        })
                };
            return View(model);
            }
        }
    }