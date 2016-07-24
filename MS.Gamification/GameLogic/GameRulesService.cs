// This file is part of the MS.Gamification project
// 
// File: GameRulesService.cs  Created: 2016-07-09@20:14
// Last modified: 2016-07-24@12:34

using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic.QuerySpecifications;
using MS.Gamification.Models;
using NLog;

namespace MS.Gamification.GameLogic
    {
    public class GameRulesService : IGameEngineService
        {
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public GameRulesService(IUnitOfWork unitOfWork, IMapper mapper)
            {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
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
        public int ComputePercentComplete(IEnumerable<Challenge> challenges, IEnumerable<Observation> eligibleObservations)
            {
            var pointsAvailable = challenges.Select(p => p.Points).Sum();
            if (pointsAvailable < 1) return 0; // Avoid divide-by-zero error
            var pointsAwarded = eligibleObservations.Select(p => p.Challenge.Points).Sum();
            var percentComplete = pointsAwarded * 100 / pointsAvailable;
            return Math.Min(percentComplete, 100);
            }

        /// <summary>
        ///     Creates observations in bulk, for the specified list of users.
        /// </summary>
        /// <param name="observation">The observation template.</param>
        /// <param name="userIds">A list of user IDs.</param>
        public BatchCreateObservationsResult BatchCreateObservations(Observation observation, IEnumerable<string> userIds)
            {
            Log.Info($"Batch creating observations for {userIds.Count()} users, Challenge ID {observation.ChallengeId}");
            var resultSummary = new BatchCreateObservationsResult();
            var maybeChallenge = unitOfWork.Challenges.GetMaybe(observation.ChallengeId);
            if (maybeChallenge.None)
                {
                resultSummary.Failed = userIds.Count();
                resultSummary.Succeeded = 0;
                resultSummary.Errors["General"] = "Unable to locate the challenge in the database";
                return resultSummary;
                }
            var maybeUser = Maybe<ApplicationUser>.Empty;
            foreach (var userId in userIds)
                {
                try
                    {
                    maybeUser = unitOfWork.UsersRepository.GetMaybe(userId);
                    if (maybeUser.None)
                        {
                        ++resultSummary.Failed;
                        resultSummary.Errors[userId] = $"User not found in the database";
                        continue;
                        }
                    var specification = new ObservationsForChallenge(userId, observation.ChallengeId);
                    var userObservations = unitOfWork.Observations.AllSatisfying(specification);
                    if (userObservations.Any(p => p.ObservationDateTimeUtc.Date == observation.ObservationDateTimeUtc.Date))
                        {
                        ++resultSummary.Failed;
                        resultSummary.Errors[maybeUser.Single().UserName] = "User already has that observation on that date.";
                        continue;
                        }
                    var observationToAdd = mapper.Map<Observation, Observation>(observation);
                    observationToAdd.UserId = userId;
                    observationToAdd.ChallengeId = observation.ChallengeId;
                    observationToAdd.SubmittedImage = maybeChallenge.Single().ValidationImage;
                    observationToAdd.Status = ModerationState.Approved;
                    unitOfWork.Observations.Add(observationToAdd);
                    unitOfWork.Commit();
                    ++resultSummary.Succeeded;
                    }
                catch (Exception ex)
                    {
                    ++resultSummary.Failed;
                    resultSummary.Errors[maybeUser.Single().UserName] = $"Error: {ex.Message}";
                    }
                }

            return resultSummary;
            }

        /// <summary>
        ///     Determines whether the supplied set of observations are sufficient to complete the given level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="observations">The observations.</param>
        /// <returns><c>true</c> if [is level complete] [the specified level]; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool IsLevelComplete(MissionLevel level, IEnumerable<Observation> observations)
            {
            var eligibleObservations = observations as IList<Observation> ?? observations.ToList();
            foreach (var track in level.Tracks)
                {
                var percentComplete = ComputePercentComplete(track.Challenges, eligibleObservations);
                if (percentComplete == 100) return true;
                }
            return false;
            }
        }
    }