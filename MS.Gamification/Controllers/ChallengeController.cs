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

        [HttpPost][ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            //add code to show details of item to be removed and confirm deletion button
            // select item from viewall screen
            //show details screen = get request to (new) details method
            //confirm delete - post request to delete method
            var maybeChallenge = uow.ChallengesRepository.GetMaybe(id);
            if (maybeChallenge.None)
                {
                return HttpNotFound();
                }
            uow.ChallengesRepository.Remove(maybeChallenge.Single());
            uow.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
            {
            var maybeChallenge = uow.ChallengesRepository.GetMaybe(id);
            if (maybeChallenge.None)
            {
                return HttpNotFound();
            }
            return View(maybeChallenge.Single());


            }
    }
}