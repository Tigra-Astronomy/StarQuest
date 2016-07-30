// This file is part of the MS.Gamification project
// 
// File: CumulativeScoreController.cs  Created: 2016-07-29@21:38
// Last modified: 2016-07-30@19:48

using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic.QuerySpecifications;
using MS.Gamification.Models;

namespace MS.Gamification.Controllers.Api
    {
    /// <summary>
    ///     Supplies historical data about a player's score over time.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class CumulativeScoreController : ApiController
        {
        private readonly IUnitOfWork uow;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CumulativeScoreController" /> class.
        /// </summary>
        /// <param name="uow">
        ///     A Unit of Work containing data repositories. This is typically supplied by a Dependency Injection
        ///     framework
        /// </param>
        public CumulativeScoreController(IUnitOfWork uow)
            {
            this.uow = uow;
            }

        /// <summary>
        ///     Gets the cumulative score for the specified player for each day on which the score changed.
        /// </summary>
        /// <param name="id">The player's unique user ID (usually a GUID).</param>
        /// <returns>IHttpActionResult.</returns>
        public IHttpActionResult Get(string id)
            {
            var spec = new SingleUserWithObservations(id);
            var maybeUser = uow.Users.GetMaybe(spec);
            if (maybeUser.None)
                return BadRequest("no such user");
            var user = maybeUser.Single();
            var groupQuery = from observation in user.Observations
                             where observation.Status == ModerationState.Approved
                             let dt = observation.ObservationDateTimeUtc
                             let date = dt.Date
                             group observation by date
                             into days
                             select new {Date = days.Key, Score = days.Sum(p => p.Challenge.Points)};
            var dailyScores = groupQuery.ToList();
            var dayCount = dailyScores.Count;
            var scores = new List<int>(dayCount);
            var dates = new List<string>(dayCount);
            var runnintTotal = 0;
            foreach (var day in dailyScores)
                {
                runnintTotal += day.Score;
                scores.Add(runnintTotal);
                dates.Add($"{day.Date:yyyy-mm-dd}");
                }
            var pointsJournal = new {dates, scores};
            return Ok(pointsJournal);
            }
        }
    }