// This file is part of the MS.Gamification project
// 
// File: GameNotificationService.cs  Created: 2016-11-01@19:37
// Last modified: 2016-12-31@13:36

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MS.Gamification.EmailTemplates;
using MS.Gamification.Models;
using MS.Gamification.ViewModels.Moderation;
using NLog;
using RazorEngine.Templating;

namespace MS.Gamification.BusinessLogic.Gamification
    {
    /// <summary>
    ///     Notifies users of game events by email.
    /// </summary>
    /// <seealso cref="IGameNotificationService" />
    internal class GameNotificationService : IGameNotificationService
        {
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();
        private readonly IRazorEngineService razor;
        private readonly UrlHelper url;
        private readonly IMapper mapper;
        private readonly ApplicationUserManager userManager;


        public GameNotificationService(IRazorEngineService razor, ApplicationUserManager userManager, UrlHelper url, IMapper mapper)
            {
            this.razor = razor;
            this.userManager = userManager;
            this.url = url;
            this.mapper = mapper;
            }

        public string HomePage
            {
            get
                {
                try
                    {
                    if (url == null) return string.Empty;
                    var requestUrl = url.RequestContext.HttpContext.Request.Url;
                    var fqUrl = url.Action("Index", "Home", new {area = string.Empty}, requestUrl.Scheme);
                    //var fqUrl = url.RouteUrl("default", null, requestUrl.Scheme, requestUrl.Authority);
                    return fqUrl;
                    }
                catch (Exception e)
                    {
                    Log.Warn("Unable to determine web application home page address");
                    return string.Empty;
                    }
                }
            }

        /// <summary>
        ///     Notifies the user that an observation they submitted has been approved by a moderator.
        /// </summary>
        /// <param name="observation">The observation that has been approved.</param>
        /// <returns>An awaitable Task.</returns>
        public async Task ObservationApprovedAsync(Observation observation)
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
        public async Task BadgeAwardedAsync(Badge badge, ApplicationUser user, MissionTrack track)
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
        public async Task PendingObservationSummaryAsync(ApplicationUser user, IEnumerable<ModerationQueueItem> observations)
            {
            Log.Info($"Notifying user {user.Id} <{user.UserName}> of pending moderation requests");
            var pendingObservations = observations as IList<ModerationQueueItem> ?? observations.ToList();
            var model = new PendingObservationsEmailModel
                {
                Recipient = user.Email,
                InformationUrl = "http://starquest.monktonstargazers.org",
                PendingObservations = pendingObservations
                };
            var emailBody = razor.RunCompile("PendingObservations.cshtml", typeof(PendingObservationsEmailModel), model);
            await userManager.SendEmailAsync(user.Id, "Observations Pending Moderator Review", emailBody);
            Log.Info(
                $"Successfully notified user {user.Id} <{user.UserName}> of {pendingObservations.Count} pending observations");
            }

        public async Task NotifyUsersAsync<TModel>(TModel model, string subject, IEnumerable<string> userIds) where TModel : EmailModelBase
            {
            var razorViewName = RazorViewNameByConvention(model);
            
            foreach (var userId in userIds)
                {
                var emailModel = mapper.Map<TModel>(model);
                emailModel.Recipient = await userManager.GetEmailAsync(userId).ConfigureAwait(false);
                emailModel.InformationUrl = HomePage;
                var emailBody = razor.RunCompile(razorViewName, model.GetType(), model);
                await userManager.SendEmailAsync(userId, subject, emailBody);

            }
        }

        private string RazorViewNameByConvention(EmailModelBase model)
            {
            var modelType = model.GetType();
            var modelName = modelType.Name;
            string viewName;
            var tail = "EmailModel";
            var tailLength = tail.Length;
            if (modelName.EndsWith(tail, StringComparison.InvariantCultureIgnoreCase))
                {
                viewName = modelName.RemoveTail(tailLength);
                }
            else
                {
                viewName = modelName;
                }
            var razorPage = viewName + ".cshtml";
            return razorPage;
            }
        }
    }