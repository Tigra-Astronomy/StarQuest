// This file is part of the MS.Gamification project
// 
// File: ObservationController.cs  Created: 2016-04-22@21:48
// Last modified: 2016-05-09@01:52 by Fern

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;

namespace MS.Gamification.Controllers
    {
    public class ObservationController : UserController
        {
        readonly IUnitOfWork uow;

        public ObservationController(IUnitOfWork uow)
            {
            this.uow = uow;
            }

        // GET: Observation/Create
        public ActionResult SubmitObservation(int? id)
            {
            if (id == null)
                return new HttpNotFoundResult("Challenge ID must be specified");
            var maybeChallenge = uow.ChallengesRepository.GetMaybe(id.Value);
            if (maybeChallenge.None)
                return HttpNotFound();
            var model = new SubmitObservationViewModel
                {
                Challenge = maybeChallenge.Single(),
                ObservationDateTimeLocal = DateTime.UtcNow,
                Seeing = AntoniadiScale.Unknown,
                Transparency = TransparencyLevel.Unknown
                };

            // Set validation images
            // ToDo - randomly choose 3 incorrect images and obtain the 1 correct image from the challenge data
            // For now, just use the NoImage.png placeholder
            var validationImages = new List<string>
                {
                Challenge.NoImagePlaceholder, Challenge.NoImagePlaceholder, Challenge.NoImagePlaceholder,
                Challenge.NoImagePlaceholder
                };
            model.ValidationImages = validationImages;


            var equipmentPicklist = PickListExtensions.FromEnum<ObservingEquipment>();
            ViewBag.Equipment = equipmentPicklist.ToSelectList();
            var seeingPicklist = PickListExtensions.FromEnum<AntoniadiScale>();
            ViewBag.Seeing = seeingPicklist.ToSelectList();
            var transparencyPicklist = PickListExtensions.FromEnum<TransparencyLevel>();
            ViewBag.Transparency = transparencyPicklist.ToSelectList();
            TempData[nameof(Challenge)] = maybeChallenge.Single();
            return View(model);
            }

        [HttpPost]
        public ActionResult SubmitObservation(SubmitObservationViewModel model)
            {
            var postedChallenge = TempData[nameof(Challenge)] as Challenge;
            var maybeChallenge = uow.ChallengesRepository.GetMaybe(postedChallenge.Id);
            var challenge = maybeChallenge.Single();
            var observation = new Observation
                {
                ChallengeId = challenge.Id,
                Challenge = challenge,
                Equipment = model.Equipment,
                Notes = model.Notes,
                ObservationDateTimeUtc = model.ObservationDateTimeLocal.ToUniversalTime(),
                ObservingSite = model.ObservingSite,
                Seeing = model.Seeing,
                Status = ModerationState.AwaitingModeration,
                SubmittedImage = Challenge.NoImagePlaceholder,  // ToDo - use the actual submitted image
                ExpectedImage = Challenge.NoImagePlaceholder,   // ToDo - use the image specified by the challenge
                Transparency = model.Transparency
                };
            uow.ObservationsRepository.Add(observation);
            uow.Commit();
            // ToDo: should redirect to a confirmation screen rather than the home page
            return RedirectToRoute(new {Controller = "Home", Action = "Index"});
            }
        }
    }
