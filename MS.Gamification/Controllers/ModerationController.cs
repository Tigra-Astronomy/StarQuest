// This file is part of the MS.Gamification project
// 
// File: ModerationController.cs  Created: 2016-05-19@01:49
// Last modified: 2016-05-26@03:43

using System.Linq;
using System.Web.Mvc;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic.QuerySpecifications;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;

namespace MS.Gamification.Controllers
    {
    [Authorize(Roles = "Moderator,Administrator")]
    public class ModerationController : RequiresAuthorization
        {
        readonly IUnitOfWork uow;

        public ModerationController(IUnitOfWork uow)
            {
            this.uow = uow;
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
            var query = new SingleObservationWithNavigationProperties(id.Value);
            var maybeObservation = uow.Observations.GetMaybe(query);
            if (maybeObservation.None)
                {
                return new HttpNotFoundResult();
                }
            return View(maybeObservation.Single());
            }

        public ActionResult Approve(int id)
            {
            var maybeObservation = uow.Observations.GetMaybe(id);
            if (maybeObservation.None)
                return new HttpNotFoundResult("Could not retrieve the specified observation");
            var observation = maybeObservation.Single();
            observation.Status = ModerationState.Approved;
            uow.Commit();
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