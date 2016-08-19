// This file is part of the MS.Gamification project
// 
// File: ChallengesController.cs  Created: 2016-08-19@00:11
// Last modified: 2016-08-19@03:35

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic;
using MS.Gamification.GameLogic.QuerySpecifications;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;
using Ninject;

namespace MS.Gamification.Areas.Admin.Controllers
    {
    public class ChallengesController : RequiresAdministratorRights
        {
        private readonly IMapper mapper;
        private readonly IImageStore store;
        private readonly IUnitOfWork uow;

        public ChallengesController(IUnitOfWork uow, IMapper mapper, [Named("ValidationImageStore")] IImageStore store)
            {
            this.uow = uow;
            this.mapper = mapper;
            this.store = store;
            }

        // GET: Challenge
        public ActionResult Index()
            {
            var model = uow.Challenges.GetAll();
            return View(model);
            }

        public ActionResult Create()
            {
            var model = PopulateViewDataForCreateChallenge();
            return View(model);
            }

        public ActionResult Details(int id)
            {
            var challengeSpec = new SingleChallengeWithTrackAndCategory(id);
            var maybeChallenge = uow.Challenges.GetMaybe(challengeSpec);
            if (maybeChallenge.None)
                return HttpNotFound();
            var model = maybeChallenge.Single();
            return View(model);
            }

        private CreateChallengeViewModel PopulateViewDataForCreateChallenge(CreateChallengeViewModel viewModel = null)
            {
            var model = viewModel ?? new CreateChallengeViewModel();
            model.CategoryPicker = uow.CategoriesRepository.PickList.ToSelectList();
            model.TrackPicker = uow.MissionTracks.PickList.ToSelectList();
            var imagesInStore = store.EnumerateImages();
            var imagePickList = imagesInStore.Select(p => new PickListItem<string> {Id = p, DisplayName = p});
            model.ValidationImagePicker = imagePickList.ToSelectList();
            return model;
            }

        [HttpPost]
        public ActionResult Create(CreateChallengeViewModel model)
            {
            if (!ModelState.IsValid)
                {
                return View(PopulateViewDataForCreateChallenge(model));
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
            var challengeSpec = new SingleChallengeWithTrackAndCategory(id);
            var maybeChallenge = uow.Challenges.GetMaybe(challengeSpec);
            if (maybeChallenge.None)
                return HttpNotFound();
            var challenge = maybeChallenge.Single();
            var model = mapper.Map<Challenge, CreateChallengeViewModel>(challenge);
            model = PopulateViewDataForCreateChallenge(model);
            return View(model);
            }

        [HttpPost]
        public ActionResult Edit(CreateChallengeViewModel model)
            {
            if (!ModelState.IsValid)
                return View(model);
            var id = model.Id;
            var maybeChallenge = uow.Challenges.GetMaybe(id);
            var original = maybeChallenge.Single();
            mapper.Map(model, original);
            uow.Commit();
            return RedirectToAction("Index");
            }

        [HttpPost]
        public ActionResult Upload()
            {
            var fileNames = (IEnumerable<string>) Request.Files.AllKeys;
            if (fileNames.Count() > 1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Multiple files not allowed");
            var index = fileNames.Single();
            var postedFile = Request.Files[index];
            var postedFileName = postedFile.FileName;
            var identifier = postedFileName.ToImageIdentifier();
            try
                {
                store.Save(postedFile.InputStream, identifier);
                return Json(new {imageIdentifier = identifier});
                }
            catch (Exception e)
                {
                return Json(new {error = e.Message});
                }
            }
        }
    }