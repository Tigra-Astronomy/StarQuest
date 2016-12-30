// This file is part of the MS.Gamification project
// 
// File: GameNotificationService.cs  Created: 2016-11-01@19:37
// Last modified: 2016-12-13@03:32

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using MS.Gamification.EmailTemplates;
using MS.Gamification.Models;
using MS.Gamification.ViewModels.Moderation;
using NLog;
using RazorEngine.Templating;

namespace MS.Gamification.GameLogic
    {
    /// <summary>
    ///     Notifies users of game events by email.
    /// </summary>
    /// <seealso cref="MS.Gamification.GameLogic.IGameNotificationService" />
    internal class GameNotificationService : IGameNotificationService
        {
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();
        private readonly IRazorEngineService razor;
        private readonly UrlHelper url;
        private readonly ApplicationUserManager userManager;


        public GameNotificationService(IRazorEngineService razor, ApplicationUserManager userManager, UrlHelper url)
            {
            this.razor = razor;
            this.userManager = userManager;
            this.url = url;
            }

        public string HomePage
            {
            get
                {
                var requestUrl = url.RequestContext.HttpContext.Request.Url;
                var fqUrl = url.Action("Index", "Home", new {area = string.Empty}, requestUrl.Scheme);
                //var fqUrl = url.RouteUrl("default", null, requestUrl.Scheme, requestUrl.Authority);
                return fqUrl;
                }
            }

        /// <summary>
        ///     Notifies the user that an observation they submitted has been approved by a moderator.
        /// </summary>
        /// <param name="observation">The observation that has been approved.</param>
        /// <returns>An awaitable Task.</returns>
        public async Task ObservationApproved(Observation observation)
            {
            Log.Info($"Notifying user {observation.UserId} of observation approval for observation ID {observation.Id}");
            var model = new ModerationEmailModel
                {
                ChallengeName = observation.Challenge.Name,
                Points = observation.Challenge.Points,
                InformationUrl = HomePage,
                Recipient = observation.User.Email
                };
            var emailBody = razor.RunCompile("ObservationApproved.cshtml", typeof(ModerationEmailModel), model);
            await userManager.SendEmailAsync(observation.UserId, "Observation approved", emailBody);
            Log.Info($"Successfully notified user {observation.UserId} of observation approval");
            }

        /// <summary>
        ///     Notifies the user that they have been awarded a badge.
        /// </summary>
        /// <param name="badge">The badge that was awarded.</param>
        /// <param name="user">The recipient user.</param>
        /// <param name="track">The track that was completed resulting in the award.</param>
        public async Task BadgeAwarded(Badge badge, ApplicationUser user, MissionTrack track)
            {
            Log.Info($"Notifying user {user.Id} <{user.UserName}> of awarded badge id={badge.Id} name={badge.Name}");
            var model = new BadgeAwardedEmailModel
                {
                Recipient = user.Email,
                MissionTitle = track.MissionLevel.Mission.Title,
                InformationUrl = HomePage,
                BadgeName = badge.Name,
                LevelAwardTitle = track.MissionLevel.AwardTitle,
                TrackName = track.Name
                };
            var emailBody = razor.RunCompile("BadgeAwarded.cshtml", typeof(BadgeAwardedEmailModel), model);
            await userManager.SendEmailAsync(user.Id, "Badge Awarded", emailBody);
            Log.Info($"Successfully notified user {user.Id} <{user.UserName}> of awarded badge id={badge.Id} name={badge.Name}");
            }

        /// <summary>
        ///     Notifies a user about pending observations.
        /// </summary>
        /// <param name="user">The user to be notified.</param>
        /// <param name="observations">The list of pending observations.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task PendingObservationSummary(ApplicationUser user, IEnumerable<ModerationQueueItem> observations)
            {
            throw new NotImplementedException();
            }
        }
    }