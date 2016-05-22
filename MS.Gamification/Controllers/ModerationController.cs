// This file is part of the MS.Gamification project
// 
// File: ModerationController.cs  Created: 2016-05-19@01:49
// Last modified: 2016-05-22@16:55

using System;
using System.Linq;
using System.Web.Mvc;
using MS.Gamification.BusinessLogic.QuerySpecifications;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;

namespace MS.Gamification.Controllers
    {
    [Authorize(Roles = "Moderator,Administrator")]
    public class ModerationController : UserController
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
            var queue = uow.ObservationsRepository.AllSatisfying(query);
            var model = queue.Select(q => new ModerationQueueItem
                {
                ObservationId = q.Id,
                DateTime = q.ObservationDateTimeUtc,
                ChallengeName = q.Challenge.Name,
                UserName = q.User.UserName
                });
            return View(model);
            }

        public ActionResult Details(int? Id)
            {
            throw new NotImplementedException();
            }
        }
    }