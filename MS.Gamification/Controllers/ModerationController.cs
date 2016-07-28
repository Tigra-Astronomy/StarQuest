// This file is part of the MS.Gamification project
// 
// File: ModerationController.cs  Created: 2016-05-26@03:51
// Last modified: 2016-07-28@10:52

using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic;
using MS.Gamification.GameLogic.QuerySpecifications;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;
using NLog;

namespace MS.Gamification.Controllers
    {
    [Authorize(Roles = "Moderator,Administrator")]
    public class ModerationController : RequiresAuthorization
        {
        private static ILogger Log = LogManager.GetCurrentClassLogger();
        private readonly IGameEngineService gameEngine;
        private readonly IGameNotificationService notifier;
        private readonly IUnitOfWork uow;

        public ModerationController(IUnitOfWork uow, IGameEngineService gameEngine, IGameNotificationService notifier)
            {
            this.uow = uow;
            this.gameEngine = gameEngine;
            this.notifier = notifier;
            }

        // GET /Moderation
        public ActionResult Index()
            {
            var query = new ObservationsAwaitingModeration();
            var queue = uow.Observations.AllSatisfying(query);
            var model = queue.Select(q => new ModerationQueueItem
                {
                ObservationId = q.Id,
                DateTime = q.ObservationDateTimeUtc,
                ChallengeName = q.Challenge.Name,
                UserName = q.User.UserName
                });
            return View(model);
            }

        public ActionResult Details(int? id)
            {
            if (!id.HasValue) return new HttpStatusCodeResult(400, "No observation ID specified");
            var query = new SingleObservationWithChallengeAndUser(id.Value);
            var maybeObservation = uow.Observations.GetMaybe(query);
            if (maybeObservation.None)
                {
                return new HttpNotFoundResult();
                }
            return View(maybeObservation.Single());
            }

        public async Task<ActionResult> Approve(int id)
            {
            var observationSpec = new SingleObservationWithChallengeAndUser(id);
            var maybeObservation = uow.Observations.GetMaybe(observationSpec);
            if (maybeObservation.None)
                return new HttpNotFoundResult("Could not retrieve the specified observation");
            var observation = maybeObservation.Single();
            observation.Status = ModerationState.Approved;
            uow.Commit();
            await notifier.ObservationApproved(observation);
            await gameEngine.EvaluateBadges(observation);
            return RedirectToAction("Index");
            }

        public ActionResult Reject(int id)
            {
            var maybeObservation = uow.Observations.GetMaybe(id);
            if (maybeObservation.None)
                return new HttpNotFoundResult("Could not retrieve the specified observation");
            var observation = maybeObservation.Single();
            observation.Status = ModerationState.Rejected;
            uow.Commit();
            return RedirectToAction("Index");
            }
        }
    }