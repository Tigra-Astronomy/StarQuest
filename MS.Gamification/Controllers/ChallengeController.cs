using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MS.Gamification.DataAccess;
using MS.Gamification.DataAccess.EntityFramework6;
using MS.Gamification.Models;

namespace MS.Gamification.Controllers
{
    public class ChallengeController : Controller
    {
        readonly IUnitOfWork uow;

        public ChallengeController(IUnitOfWork uow)
            {
            this.uow = uow;
            }


        // GET: Challenge
        public ActionResult Index()
            {
            var model = uow.ChallengesRepository.GetAll();
            return View(model);
        }

        public ActionResult Create()
            {
            return View();
            }

        [HttpPost]
        public ActionResult Create(Challenge model)
            {
            uow.ChallengesRepository.Add(model);
            uow.Commit();
            return RedirectToAction("Index");
            }
    }
}