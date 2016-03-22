// This file is part of the MS.Gamification project
// 
// File: ChallengeRepository.cs  Created: 2016-03-18@20:18
// Last modified: 2016-03-21@22:50 by Fern

using MS.Gamification.Models;

namespace MS.Gamification.DataAccess.EntityFramework6
    {
    /// <summary>
    ///   Stores the challenges.
    /// </summary>
    /// <seealso cref="EntityFramework6.Repository{Challenge, int}" />
    public class ChallengeRepository : Repository<Challenge, int>
        {
        /// <summary>
        ///   Initializes a new instance of the <see cref="ChallengeRepository" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ChallengeRepository(ApplicationDbContext dbContext) : base(dbContext) {}
        }
    }
