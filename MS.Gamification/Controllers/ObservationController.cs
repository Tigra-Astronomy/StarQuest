// This file is part of the MS.Gamification project
// 
// File: ObservationController.cs  Created: 2016-04-22@21:48
// Last modified: 2016-04-24@18:15 by Fern

using System;
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
                ObservationDateTimeUtc = DateTime.UtcNow,
                Seeing = AntoniadiScale.Unknown,
                Transparency = TransparencyLevel.Unknown
                };
            return View(model);
            }

        //public ActionResult SubmitObservation(FormCollection collection)
        //[HttpPost]

        // POST: Observation/Create
        //    {
        //    }
        }
    }
