// This file is part of the MS.Gamification project
// 
// File: ChallengeController.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-13@23:33

using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;

namespace MS.Gamification.Controllers
    {
    public class ChallengeController : AdminController
        {
        private readonly IMapper mapper;
        private readonly IUnitOfWork uow;

        public ChallengeController(IUnitOfWork uow, IMapper mapper)
            {
            this.uow = uow;
            this.mapper = mapper;
            }

        // GET: Challenge
        public ActionResult Index()
            {
            var model = uow.Challenges.GetAll();
            return View(model);
            }

        public ActionResult Create()
            {
            PopulateViewDataForCreateChallenge();
            return View();
            }

        public ActionResult Details(int id)
            {
            var model = new Challenge();
            return View(model);
            }

        private void PopulateViewDataForCreateChallenge()
            {
            var pickListItems = uow.CategoriesRepository.PickList;
            var tracks = uow.MissionTracks.PickList;
            ViewBag.Categories = pickListItems.ToSelectList();
            ViewBag.Tracks = tracks.ToSelectList();
            }

        [HttpPost]
        public ActionResult Create(CreateChallengeViewModel model)
            {
            if (!ModelState.IsValid)
                {
                PopulateViewDataForCreateChallenge();
                return View(model);
                }
            var challenge = mapper.Map<CreateChallengeViewModel, Challenge>(model);
            uow.Challenges.Add(challenge);
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
            var maybeChallenge = uow.Challenges.GetMaybe(id);
            if (maybeChallenge.None)
                return HttpNotFound();
            uow.Challenges.Remove(maybeChallenge.Single());
            uow.Commit();
            return RedirectToAction("Index");
            }

        public ActionResult Delete(int id)
            {
            var maybeChallenge = uow.Challenges.GetMaybe(id);
            if (maybeChallenge.None)
                return HttpNotFound();
            return View(maybeChallenge.Single());
            }

        public ActionResult Edit(int id)
            {
            var maybeChallenge = uow.Challenges.GetMaybe(id);
            if (maybeChallenge.None)
                return HttpNotFound();

            var pickListItems = uow.CategoriesRepository.PickList;
            ViewBag.Categories = pickListItems.ToSelectList();
            return View(maybeChallenge.Single());
            }

        [HttpPost]
        public ActionResult Edit(Challenge model)
            {
            if (!ModelState.IsValid)
                return View(model);
            var id = model.Id;
            var maybeChallenge = uow.Challenges.GetMaybe(id);
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