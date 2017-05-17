// This file is part of the MS.Gamification project
// 
// File: GameRulesService.cs  Created: 2016-07-09@20:14
// Last modified: 2016-08-20@02:33

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MS.Gamification.Areas.Admin.ViewModels.MissionLevels;
using MS.Gamification.Areas.Admin.ViewModels.MissionTracks;
using MS.Gamification.BusinessLogic.Gamification.Preconditions;
using MS.Gamification.BusinessLogic.Gamification.QuerySpecifications;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using NLog;

namespace MS.Gamification.BusinessLogic.Gamification
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
        ///     <see cref="EligibleObservationsForChallenges" />.
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
        ///     Deletes the specified mission, if it is safe to do so.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the mission could not be deleted.</exception>
        public async Task DeleteMissionAsync(int id)
            {
            Log.Info($"Attempting to delete mission id={id}");
            var associatedObservationsSpec = new MissionHasAssociatedObservations(id);
            var maybeObservations = unitOfWork.Observations.GetMaybe(associatedObservationsSpec);
            if (maybeObservations.Any())
                {
                Log.Warn($"Delete mission id={id} PREVENTED because of associated observations");
                throw new InvalidOperationException("Delete prevented: there are observations associated with this mission");
                }
            var maybeMission = unitOfWork.Missions.GetMaybe(id);
            if (maybeMission.None)
                {
                Log.Error($"Delete mission id={id} FAILED because the mission was not found in the database");
                throw new InvalidOperationException("Unable to locate the mission in the database");
                }
            var mission = maybeMission.Single();
            unitOfWork.Missions.Remove(mission);
            await unitOfWork.CommitAsync();
            Log.Info($"Delete mission id={mission.Id} title={mission.Title} SUCCEEDED");
            }

        /// <summary>
        ///     Deletes the specified mission level, if it is safe to do so.
        /// </summary>
        /// <param name="levelId">The ID of the level to be deleted.</param>
        /// <exception cref="InvalidOperationException">Thrown is the level was not deleted for any reason.</exception>
        public async Task DeleteLevelAsync(int levelId)
            {
            Log.Info($"Deleting level id={levelId}");
            var maybeLevel = unitOfWork.MissionLevels.GetMaybe(levelId);
            if (maybeLevel.None)
                {
                Log.Error($"Delete failed because level id {levelId} was not found in the database");
                throw new ArgumentException("Level not found");
                }
            var level = maybeLevel.Single();
            if (LevelHasAssociatedObservations(levelId))
                {
                Log.Warn($"Delete blocked because level {levelId} has associated observations");
                throw new InvalidOperationException("Cannot delete a level that has observations associated with it");
                }
            unitOfWork.MissionLevels.Remove(level);
            await unitOfWork.CommitAsync();
            Log.Info($"Successfully deleted level id={levelId}");
            }

        /// <summary>
        ///     Creates the specified level according to game rules.
        ///     Throws an exception if the level was invalid or could not be created.
        /// </summary>
        /// <param name="newLevel">The new level.</param>
        public async Task CreateLevelAsync(MissionLevelViewModel newLevel)
            {
            Log.Info($"Creating new level id={newLevel.Id} name={newLevel.Name} in mission {newLevel.MissionId}");
            var specification = new LevelExistsInMission(newLevel.Level, newLevel.MissionId);
            var maybeHasLevel = unitOfWork.MissionLevels.GetMaybe(specification);
            if (maybeHasLevel.Any())
                {
                Log.Warn($"Create failed because mission {newLevel.MissionId} already has a level {newLevel.Level}");
                throw new InvalidOperationException($"There is already a level number {newLevel.Level} in the mission");
                }
            var levelToAdd = mapper.Map<MissionLevelViewModel, MissionLevel>(newLevel);
            unitOfWork.MissionLevels.Add(levelToAdd);
            await unitOfWork.CommitAsync();
            Log.Info($"Successfully created level id={newLevel.Id} name={newLevel.Name} in mission {newLevel.MissionId}");
            }

        /// <summary>
        ///     Updates the level in the database with the supplied values, provided
        ///     that no game rules are violated.
        /// </summary>
        /// <param name="updatedLevel">The updated level (which must include the ID).</param>
        /// <returns>Task.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the level update failed for any reason.</exception>
        public async Task UpdateLevelAsync(MissionLevelViewModel updatedLevel)
            {
            Log.Info($"Updating level id={updatedLevel.Id} name={updatedLevel.Name} mission={updatedLevel.MissionId}");
            var maybeLevel = unitOfWork.MissionLevels.GetMaybe(updatedLevel.Id);
            if (maybeLevel.None)
                {
                Log.Error($"Update failed because level {updatedLevel.Id} was not found in the database");
                throw new InvalidOperationException("The mission to be updated was not found in the database");
                }
            var dbLevel = maybeLevel.Single();
            var maybeMission = unitOfWork.Missions.GetMaybe(updatedLevel.MissionId);
            if (maybeMission.None)
                {
                Log.Error($"Update failed because mission {updatedLevel.MissionId} was not found in the database");
                throw new InvalidOperationException("The target mission does not exist in the database");
                }
            var targetMission = maybeMission.Single();
            if (targetMission.MissionLevels.Any(p => p.Level == updatedLevel.Level && p.Id != updatedLevel.Id))
                {
                Log.Warn($"Update blocked because the target mission {targetMission.Id} already had level {updatedLevel.Level}");
                throw new InvalidOperationException($"The target mission already has a level {updatedLevel.Level}");
                }
            if (LevelHasAssociatedObservations(updatedLevel.Id))
                {
                Log.Warn($"Update blocked because the level has associated observations");
                throw new InvalidOperationException("Cannot move a level that has observations associated with it");
                }
            mapper.Map(updatedLevel, dbLevel);
            await unitOfWork.CommitAsync();
            Log.Info($"Successfully updated level id={dbLevel.Id}");
            }

        /// <exception cref="InvalidOperationException">Thrown if the track was not created for any reason.</exception>
        public async Task CreateTrackAsync(MissionTrack newTrack)
            {
            Log.Info(
                $"Creating new Mission Track id={newTrack.Id} Name={newTrack.Name} in mission {newTrack.MissionLevelId} with badge id={newTrack.BadgeId}");
            var maybeBadge = unitOfWork.Badges.GetMaybe(newTrack.BadgeId);
            if (maybeBadge.None)
                {
                Log.Error($"Create blocked because the target badge id={newTrack.BadgeId} does not exist");
                throw new InvalidOperationException("The associated Mission Level could not be found");
                }
            var targetLevel = unitOfWork.MissionLevels.GetMaybe(newTrack.MissionLevelId);
            if (targetLevel.None)
                {
                Log.Error($"Create blocked because the target level id={newTrack.MissionLevelId} does not exist");
                throw new InvalidOperationException("The associated Mission Level could not be found");
                }
            var missionTracks = unitOfWork.MissionTracks.GetAll();
            if (missionTracks.Any(p => p.MissionLevelId == newTrack.MissionLevelId && p.Number == newTrack.Number))
                {
                Log.Warn("Create blocked because the track already exists");
                throw new InvalidOperationException("That track number already exists");
                }
            unitOfWork.MissionTracks.Add(newTrack);
            await unitOfWork.CommitAsync();
            Log.Info($"Successfully added track id={newTrack.Id}");
            }

        /// <summary>
        ///     Deletes the track provided game rules allow it.
        /// </summary>
        /// <param name="id">The track ID.</param>
        /// <exception cref="InvalidOperationException">Thrown if the track could not be deleted for any reason.</exception>
        public async Task DeleteTrackAsync(int id)
            {
            Log.Info($"Deleting mission track id={id}");
            var maybeTrack = unitOfWork.MissionTracks.GetMaybe(id);
            if (maybeTrack.None)
                {
                Log.Error($"Delete failed, track id={id} was not found in the database");
                throw new InvalidOperationException("Track not found in the database");
                }
            var observationSpec = new TrackHasAssociatedObservations(id);
            var maybeHasObservations = unitOfWork.Observations.GetMaybe(observationSpec);
            if (maybeHasObservations.Any())
                {
                Log.Warn($"Delete of track id={id} blocked because there are associated observations");
                throw new InvalidOperationException("Cannot delete a track that has observations associated with it");
                }
            unitOfWork.MissionTracks.Remove(maybeTrack.Single());
            await unitOfWork.CommitAsync();
            Log.Info($"Successfully deleted track id={id}");
            }

        /// <summary>
        ///     Updates a mission track from values in the submitted model, provided that game rules allow it.
        /// </summary>
        /// <param name="model">The model containing new values.</param>
        /// <returns>Task.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the track was not updated for any reason.</exception>
        public async Task UpdateTrackAsync(MissionTrackViewModel model)
            {
            Log.Info($"Updating track {model}");
            var maybeTrack = unitOfWork.MissionTracks.GetMaybe(model.Id);
            if (maybeTrack.None)
                {
                Log.Error($"The target track id={model.Id} was not found in the database");
                throw new InvalidOperationException("The track was not found in the database");
                }
            var targetLevel = model.MissionLevelId;
            var maybeLevel = unitOfWork.MissionLevels.GetMaybe(targetLevel);
            if (maybeLevel.None)
                {
                Log.Error($"Update blocked because the target level id={targetLevel} was not found");
                throw new InvalidOperationException("Can't move the track to a non-existent level");
                }
            var dbLevel = maybeLevel.Single();
            if (dbLevel.Tracks.Any(p => p.Number == model.Number && p.Id != model.Id))
                {
                Log.Warn($"Update blocked because destination level id={targetLevel} already has a track number={model.Number}");
                throw new InvalidOperationException("The destination track already has that level number");
                }
            var dbTrack = maybeTrack.Single();

            mapper.Map(model, dbTrack);
            await unitOfWork.CommitAsync();
            }

        /// <summary>
        ///     Determines whether a level is unlocked for a user by evaluating the level preconditions against that user.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="userId">The user.</param>
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

        private bool LevelHasAssociatedObservations(int levelId)
            {
            var observationSpec = new LevelHasAssociatedObservations(levelId);
            var maybeHasObservations = unitOfWork.Observations.GetMaybe(observationSpec);
            return maybeHasObservations.Any();
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