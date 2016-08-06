// This file is part of the MS.Gamification project
// 
// File: GameRulesService.cs  Created: 2016-07-09@20:14
// Last modified: 2016-07-30@13:47

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic.Preconditions;
using MS.Gamification.GameLogic.QuerySpecifications;
using MS.Gamification.Models;
using NLog;

namespace MS.Gamification.GameLogic
    {
    public class GameRulesService : IGameEngineService
        {
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();
        private readonly IMapper mapper;
        private readonly IGameNotificationService notifier;
        private readonly IUnitOfWork unitOfWork;

        public GameRulesService(IUnitOfWork unitOfWork, IMapper mapper, IGameNotificationService notifier)
            {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.notifier = notifier;
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
                    maybeUser = unitOfWork.Users.GetMaybe(userId);
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
        ///     Evaluates whether the user is entitled to any new badges, as a result of submitting an observation.
        /// </summary>
        /// <param name="observation">The observation that has just been approved for the user.</param>
        public async Task EvaluateBadges(Observation observation)
            {
            var userId = observation.UserId;
            Log.Info($"Evaluating badges for user id={userId} name={observation.User.UserName}");
            try
                {
                var challenge = observation.Challenge;
                var track = challenge.MissionTrack;
                var badgeForTrack = track.Badge;
                var alreadyHasBadge = badgeForTrack.Users.Any(p => p.Id == userId);
                if (alreadyHasBadge)
                    return;
                var eligibleObservationsSpec = new EligibleObservationsForChallenges(track.Challenges, userId);
                var eligibleObservations = unitOfWork.Observations.AllSatisfying(eligibleObservationsSpec);
                var percentComplete = ComputePercentComplete(track.Challenges, eligibleObservations);
                if (percentComplete < 100)
                    return;
                AwardBadge(badgeForTrack.Id, userId);
                await notifier.BadgeAwarded(badgeForTrack, observation.User, track);
                }
            finally
                {
                Log.Info($"Completed evaluating badges for user id={userId} name={observation.User.UserName}");
                }
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

        /// <summary>
        /// Deletes the specified mission, if it is safe to do so.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        public Task DeleteMissionAsync(int id)
            {
            throw new InvalidOperationException("Deleting missions is not currently supported");
            }

        /// <summary>
        ///     Determines whether a level is unlocked for a user by evaluating the level preconditions against that user.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="user">The user.</param>
        /// <returns><c>true</c> if [is level unlocked for user] [the specified level]; otherwise, <c>false</c>.</returns>
        public bool IsLevelUnlockedForUser(IPreconditionXml level, string userId)
            {
            try
                {
                var preconditionXml = level.Precondition ?? string.Empty;
                var specification = new SingleUserWithBadges(userId);
                var maybeUser = unitOfWork.Users.GetMaybe(specification);
                if (maybeUser.None)
                    return false;
                if (string.IsNullOrWhiteSpace(preconditionXml))
                    return true; // No rules = unlocked
                var parser = new LevelPreconditionParser();
                var rules = parser.ParsePreconditionXml(preconditionXml);
                return rules.Evaluate(maybeUser.Single());
                }
            catch (Exception e)
                {
                Log.Error(e, $"Error while evaluating level access for user {userId}", level);
                return false;
                }
            }

        /// <summary>
        ///     Awards a badge to a user.
        /// </summary>
        /// <param name="badgeId">The badge identifier.</param>
        /// <param name="userId">The user identifier.</param>
        private void AwardBadge(int badgeId, string userId)
            {
            var maybeBadge = unitOfWork.Badges.GetMaybe(badgeId);
            if (maybeBadge.None)
                {
                Log.Warn($"Attempt to award badge {badgeId} to {userId} and the badge doesn't exist");
                return;
                }
            var maybeUser = unitOfWork.Users.GetMaybe(userId);
            if (maybeUser.None)
                {
                Log.Warn($"Attempt to award badge {badgeId} to user {userId} and the user doesn't exist");
                return;
                }
            var badge = maybeBadge.Single();
            if (badge.Users.Any(p => p.Id == userId))
                {
                Log.Warn($"Attempt to award badge {badgeId} to user {userId} but the user already has the badge");
                return;
                }
            badge.Users.Add(maybeUser.Single());
            unitOfWork.Commit();
            }
        }
    }