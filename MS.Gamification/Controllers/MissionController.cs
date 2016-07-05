// This file is part of the MS.Gamification project
// 
// File: MissionController.cs  Created: 2016-07-01@00:05
// Last modified: 2016-07-05@00:40

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MS.Gamification.BusinessLogic.QuerySpecifications;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;

namespace MS.Gamification.Controllers
    {
    public class MissionController : UserController
        {
        private readonly ICurrentUser requestingUser;
        private readonly IUnitOfWork uow;

        public MissionController(IUnitOfWork uow, ICurrentUser user)
            {
            this.uow = uow;
            requestingUser = user;
            }

        // GET Mission/Level/1
        public ActionResult Level(int id)
            {
            // Validate the parameters and fetch the Mission from the database
            var query = new MissionLevelProgress(id);
            var maybeMission = uow.Missions.GetMaybe(query);
            if (maybeMission.None)
                return HttpNotFound("The specified Mission ID was not found");
            var mission = maybeMission.Single();
            var model = new MissionProgressViewModel();
            model.Level = mission.Level;

            model.Tracks = new List<MissionTrack>(mission.Tracks);
            // Get the logged in requestingUser and fetch the requestingUser's Observation log from the database
            //var observationSpecification = new ObservationsForUserMission(requestingUser.UniqueId, mission.Id);
            //var userObservations = uow.Observations.AllSatisfying(observationSpecification);
            var challengeSpecification = new ChallengesInMission(mission.Id);
            var challengesInMission = uow.Challenges.AllSatisfying(challengeSpecification);
            // Only count one observation towards each challenge, for the purposes of computing progress.
            var eligibleObservationsForMission = new EligibleObservationsForChallenges(challengesInMission,
                requestingUser.UniqueId);
            var observationsForMission = uow.Observations.AllSatisfying(eligibleObservationsForMission);
            var missionTotalChallenges = 0;
            mission.Tracks.ForEach(p => missionTotalChallenges += p.Challenges.Count);
            model.OverallProgressPercent = ComputePercentComplete(challengesInMission, observationsForMission);
            // Compute individual track progress
            var trackPercentComplete = new List<int>(model.Tracks.Count());
            foreach (var track in model.Tracks)
                {
                var challengesInTrack = challengesInMission.Where(p => p.MissionTrackId == track.Id).ToList();
                var eligibleObservationsForTrack = new EligibleObservationsForChallenges(challengesInTrack,
                    requestingUser.UniqueId);
                var observations = uow.Observations.AllSatisfying(eligibleObservationsForTrack);
                trackPercentComplete.Add(ComputePercentComplete(challengesInTrack, observations));
                }
            model.TrackProgress = trackPercentComplete;
            return View(model);
            }

        /// <summary>
        ///     Computes the percent complete for a set of challenges, given a set of eligible observations. The
        ///     computation is based on the number of points gained, rather than just a simple count.
        /// </summary>
        /// <param name="challenges">The set of challenges that represents 100% progress.</param>
        /// <param name="eligibleObservations">The eligible observations for the set of challenges.</param>
        /// <returns>The computed percentage, as an integer, guaranteed to be between 0% and 100% inclusive.</returns>
        /// <remarks>
        ///     It is assumed that the set of observations has already been filtered for eligibility, e.g. by calling
        ///     <see cref="EligibleObservations" />.
        /// </remarks>
        private int ComputePercentComplete(IEnumerable<Challenge> challenges, IEnumerable<Observation> eligibleObservations)
            {
            var pointsAvailable = challenges.Select(p => p.Points).Sum();
            if (pointsAvailable < 1) return 0; // Avoid divide-by-zero error
            var pointsAwarded = eligibleObservations.Select(p => p.Challenge.Points).Sum();
            var percentComplete = pointsAwarded * 100 / pointsAvailable;
            return Math.Min(percentComplete, 100);
            }
        }
    }