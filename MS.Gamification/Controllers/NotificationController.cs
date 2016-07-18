// This file is part of the MS.Gamification project
// 
// File: NotificationController.cs  Created: 2016-06-06@21:07
// Last modified: 2016-06-06@22:28

using System.Linq;
using System.Web.Mvc;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic;
using MS.Gamification.GameLogic.QuerySpecifications;
using MS.Gamification.Models;

namespace MS.Gamification.Controllers
    {
    public class NotificationController : Controller
        {
        readonly IUnitOfWork uow;

        public NotificationController(IUnitOfWork uow)
            {
            this.uow = uow;
            }

        public ActionResult ModerationQueue()
            {
            if (!User.IsInRole(RequiresAdministratorRights.ModeratorRoleName))
                {
                return new EmptyResult();
                }
            var query = new ObservationsAwaitingModeration();
            query.FetchStrategy = new GenericFetchStrategy<Observation>();
            var results = uow.Observations.AllSatisfying(query);
            return View(results.Count());
            }
        }
    }