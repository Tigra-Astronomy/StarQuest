// This file is part of the MS.Gamification project
// 
// File: ObservationController.cs  Created: 2016-08-20@23:12
// Last modified: 2016-11-01@19:22

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic.QuerySpecifications;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;

namespace MS.Gamification.Controllers
    {
    public class ObservationController : RequiresAuthorization
        {
        private readonly IMapper mapper;
        private readonly IUnitOfWork uow;
        private readonly ICurrentUser webUser;

        public ObservationController(IUnitOfWork uow, IMapper mapper, ICurrentUser webUser)
            {
            this.uow = uow;
            this.mapper = mapper;
            this.webUser = webUser;
            }

        // GET: Observation/Create
        public ActionResult SubmitObservation(int? id)
            {
            if (id == null)
                return new HttpNotFoundResult("Challenge ID must be specified");
            var maybeChallenge = uow.Challenges.GetMaybe(id.Value);
            if (maybeChallenge.None)
                return HttpNotFound();
            var challenge = maybeChallenge.Single();
            var model = new SubmitObservationViewModel
                {
                ChallengeId = challenge.Id,
                ChallengeName = challenge.Name,
                ObservationDateTimeLocal = DateTime.UtcNow,
                Seeing = AntoniadiScale.Unknown,
                Transparency = TransparencyLevel.Unknown
                };

            // Set validation images
            // ToDo - randomly choose 3 incorrect images and obtain the 1 correct image from the challenge data
            // For now, just use the NoImage.png placeholder
            var validationImages = GetValidationImages(challenge);
            model.ValidationImages = validationImages.ToList();


            model.EquipmentPicker = PickListExtensions.FromEnum<ObservingEquipment>().ToSelectList();
            model.SeeingPicker = PickListExtensions.FromEnum<AntoniadiScale>().ToSelectList();
            model.TransparencyPicker = PickListExtensions.FromEnum<TransparencyLevel>().ToSelectList();
            return View(model);
            }

        private IEnumerable<Challenge> IncorrectImagesForChallenge(Challenge challenge)
            {
            var specification = new IncorrectValidationImagesForChallenge(challenge);
            return uow.Challenges.AllSatisfying(specification);
            }

        /// <summary>
        ///     Builds a list of exactly 4 validation images, consisting of one correct image and 3 incorrect images, in
        ///     a random order.The resultant collection is guaranteed to always have exactly 4 entries.
        /// </summary>
        /// <param name="challenge"></param>
        /// <returns></returns>
        private IEnumerable<string> GetValidationImages(Challenge challenge)
            {
            var results = new Dictionary<int, string>();
            var generator = new Random();
            // Add the validation image from the specified challenge, or a placeholder if none is specified
            results.Add(generator.Next(int.MaxValue),
                string.IsNullOrWhiteSpace(challenge.ValidationImage)
                    ? Challenge.NoImagePlaceholder
                    : challenge.ValidationImage);

            /*
             * Get all the possible incorrect images and whittle that down to a list of 3 chosen at random.
             * If there are less than 3 available, then placeholders will be used.
             * The result (trio) is guaranteed to have 3 elements.
             */
            var incorrect = IncorrectImagesForChallenge(challenge).Select(item => item.ValidationImage).ToList();
            var trio = SelectRandomElementsFromList(3, incorrect, Challenge.NoImagePlaceholder);

            var randomKey = 0;
            foreach (var image in trio)
                {
                do
                    {
                    randomKey = generator.Next(int.MaxValue);
                    } while (results.ContainsKey(randomKey));
                results.Add(randomKey, image);
                }
            /*
             * Now we have a dictionary with exactly 4 entries, each with a random unique key.
             * Sorting the keys essentially scrambles the order. Then we return only the values.
             */
            return results.OrderBy(p => p.Key).Select(q => q.Value).AsEnumerable();
            }

        /// <summary>
        ///     Selects and returns a specified number of elements chosen randomly from a collection. If there are not
        ///     enough elements in the source collection, a filler is used. The resulting collection is guaranteed to have
        ///     exactly the requested number of elements.
        /// </summary>
        /// <typeparam name="T">The type of collection</typeparam>
        /// <param name="count">The number of elements requested.</param>
        /// <param name="source">The source collection to be randomly sampled.</param>
        /// <param name="filler">
        ///     An object used as padding when there are not enough source elements to satisfy the
        ///     request.
        /// </param>
        /// <returns></returns>
        private IEnumerable<T> SelectRandomElementsFromList<T>(int count, IEnumerable<T> source, T filler)
            {
            var generator = new Random();
            var results = new List<T>(count);
            var usedElements = new List<int>(3);
            var sourceList = source.ToList();
            var sourceCount = sourceList.Count;
            var index = 0;
            for (var i = 0; i < count; i++)
                {
                if (i >= sourceCount)
                    break;
                do
                    {
                    index = generator.Next(sourceCount);
                    } while (usedElements.Contains(index));
                usedElements.Add(index);
                results.Add(sourceList[index]);
                }

            var resultCount = results.Count;
            var fillerCount = count - resultCount;
            while (fillerCount > 0)
                {
                results.Add(filler);
                --fillerCount;
                }
            return results;
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitObservation(SubmitObservationViewModel model)
            {
            var observation = mapper.Map<SubmitObservationViewModel, Observation>(model);
            observation.UserId = webUser.UniqueId;
            var maybeChallenge = uow.Challenges.GetMaybe(model.ChallengeId);
            if (maybeChallenge.None)
                throw new ArgumentException("Invalid challenge ID specified");
            observation.ExpectedImage = maybeChallenge.Single().ValidationImage;
            observation.Status = ModerationState.AwaitingModeration;
            uow.Observations.Add(observation);
            uow.Commit();
            // ToDo: should redirect to a confirmation screen rather than the home page
            return RedirectToRoute(new {Controller = "Home", Action = "Index"});
            }

        public ActionResult Index()
            {
            var userId = User.Identity.GetUserId();
            var specification = new SingleUserWithObservations(userId);
            var maybeUser = uow.Users.GetMaybe(specification);
            if (maybeUser.None) return new HttpStatusCodeResult(500, "Unable to retrieve webUser details. Sorry!");
            return View(maybeUser.Single().Observations);
            }

        public ActionResult Details(int id)
            {
            var specification = new SingleObservationWithChallengeAndUser(id);
            var maybeObservation = uow.Observations.GetMaybe(specification);
            if (maybeObservation.None)
                return HttpNotFound("There is no observation with the specified ID");
            var model = mapper.Map<Observation, ObservationDetailsViewModel>(maybeObservation.Single());
            return View(model);
            }
        }
    }