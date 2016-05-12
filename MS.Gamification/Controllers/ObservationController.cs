// This file is part of the MS.Gamification project
// 
// File: ObservationController.cs  Created: 2016-04-22@21:48
// Last modified: 2016-05-09@01:52 by Fern

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MS.Gamification.BusinessLogic.QuerySpecifications;
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
            var validationImages = GetValidationImages(model.Challenge);
            model.ValidationImages = validationImages.ToList();


            var equipmentPicklist = PickListExtensions.FromEnum<ObservingEquipment>();
            ViewBag.Equipment = equipmentPicklist.ToSelectList();
            var seeingPicklist = PickListExtensions.FromEnum<AntoniadiScale>();
            ViewBag.Seeing = seeingPicklist.ToSelectList();
            var transparencyPicklist = PickListExtensions.FromEnum<TransparencyLevel>();
            ViewBag.Transparency = transparencyPicklist.ToSelectList();
            TempData[nameof(Challenge)] = maybeChallenge.Single();
            return View(model);
            }

        IEnumerable<Challenge> IncorrectImagesForChallenge(Challenge challenge)
            {
            var specification = new IncorrectValidationImagesForChallenge(challenge);
            return uow.ChallengesRepository.AllSatisfying(specification);
            }

        /// <summary>
        /// Builds a list of exactly 4 validation images, consisting of one correct image
        /// and 3 incorrect images, in a random order.The resultant collection is guaranteed
        /// to always have exactly 4 entries.
        /// </summary>
        /// <param name="challenge"></param>
        /// <returns></returns>
        IEnumerable<string> GetValidationImages(Challenge challenge)
            {
            var results = new Dictionary<int,string>();
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

            int randomKey = 0;
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
        /// Selects and returns a specified number of elements chosen randomly from a collection. If there are not
        /// enough elements in the source collection, a filler is used. The resulting collection is guaranteed to have
        /// exactly the requested number of elements.
        /// </summary>
        /// <typeparam name="T">The type of collection</typeparam>
        /// <param name="count">The number of elements requested.</param>
        /// <param name="source">The source collection to be randomly sampled.</param>
        /// <param name="filler">An object used as padding when there are not enough source elements to satisfy the
        /// request.</param>
        /// <returns></returns>
        IEnumerable<T> SelectRandomElementsFromList<T>(int count, IEnumerable<T> source, T filler)
            {
            var generator = new Random();
            var results = new List<T>(count);
            var usedElements = new List<int>(3);
            var sourceList = source.ToList();
            var sourceCount = sourceList.Count;
            int index = 0;
            for (int i = 0; i < count; i++)
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
