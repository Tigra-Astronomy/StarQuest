// This file is part of the MS.Gamification project
// 
// File: ModerationController.cs  Created: 2016-05-19@00:56
// Last modified: 2016-05-19@01:46

using System.Web.Mvc;
using MS.Gamification.BusinessLogic.QuerySpecifications;
using MS.Gamification.DataAccess;

namespace MS.Gamification.Controllers
    {
    [Authorize(Roles = "Moderator")]
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
            var model = uow.ObservationsRepository.AllSatisfying(query);
            return View(model);
            }
        }
    }