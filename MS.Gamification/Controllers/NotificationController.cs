// This file is part of the MS.Gamification project
// 
// File: NotificationController.cs  Created: 2016-06-06@21:07
// Last modified: 2016-06-06@22:28

using System.Linq;
using System.Web.Mvc;
using MS.Gamification.BusinessLogic;
using MS.Gamification.BusinessLogic.QuerySpecifications;
using MS.Gamification.DataAccess;
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
            if (!User.IsInRole(AdminController.ModeratorRoleName))
                {
                return new EmptyResult();
                }
            var query = new ObservationsAwaitingModeration();
            query.FetchStrategy = new GenericFetchStrategy<Observation>();
            var results = uow.ObservationsRepository.AllSatisfying(query);
            return View(results.Count());
            }
        }
    }