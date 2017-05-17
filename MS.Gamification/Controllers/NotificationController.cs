// This file is part of the MS.Gamification project
// 
// File: NotificationController.cs  Created: 2016-11-01@19:37
// Last modified: 2017-05-16@20:13

using System.Linq;
using System.Web.Mvc;
using MS.Gamification.BusinessLogic.Gamification;
using MS.Gamification.BusinessLogic.Gamification.QuerySpecifications;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;

namespace MS.Gamification.Controllers
    {
    public class NotificationController : Controller
        {
        private readonly IUnitOfWork uow;

        public NotificationController(IUnitOfWork uow)
            {
            this.uow = uow;
            }

        public ActionResult ModerationQueue()
            {
            if (!User.IsInRole(RoleNames.Moderator))
                {
                return new EmptyResult();
                }
            var query = new ObservationsAwaitingModeration();
            query.FetchStrategy = new GenericFetchStrategy<Observation>();
            var results = uow.Observations.AllSatisfying(query);
            var count = results.Count();
            if (count == 0)
                return new EmptyResult();
            return View(count);
            }
        }
    }