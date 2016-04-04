// This file is part of the MS.Gamification project
// 
// File: ChallengeController.cs  Created: 2016-04-01@23:54
// Last modified: 2016-04-04@02:06 by Fern

using System.Linq;
using System.Web.Mvc;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;

namespace MS.Gamification.Controllers
    {
    public class ChallengeController : AdminController
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
            var pickListItems = uow.CategoriesRepository.PickList;
            ViewBag.Categories = pickListItems.ToSelectList();
            return View();
            }

        [HttpPost]
        public ActionResult Create(Challenge model)
            {
            if (!ModelState.IsValid)
                return View(model);

            uow.ChallengesRepository.Add(model);
            uow.Commit();
            return RedirectToAction("Index");
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
            {
            //add code to show details of item to be removed and confirm deletion button
            // select item from viewall screen
            //show details screen = get request to (new) details method
            //confirm delete - post request to delete method
            var maybeChallenge = uow.ChallengesRepository.GetMaybe(id);
            if (maybeChallenge.None)
                return HttpNotFound();
            uow.ChallengesRepository.Remove(maybeChallenge.Single());
            uow.Commit();
            return RedirectToAction("Index");
            }

        public ActionResult Delete(int id)
            {
            var maybeChallenge = uow.ChallengesRepository.GetMaybe(id);
            if (maybeChallenge.None)
                return HttpNotFound();
            return View(maybeChallenge.Single());
            }

        public ActionResult Edit(int id)
            {
            var maybeChallenge = uow.ChallengesRepository.GetMaybe(id);
            if (maybeChallenge.None)
                return HttpNotFound();

            return View(maybeChallenge.Single());
            }

        [HttpPost]
        public ActionResult Edit(Challenge model)
            {
            if (!ModelState.IsValid)
                return View(model);
            var id = model.Id;
            var maybeChallenge = uow.ChallengesRepository.GetMaybe(id);
            var original = maybeChallenge.Single();
            original.BookSection = model.BookSection;
            original.Category = model.Category;
            original.Location = model.Location;
            original.Name = model.Name;
            original.Points = model.Points;
            uow.Commit();
            return RedirectToAction("Index");
            }
        }
    }
