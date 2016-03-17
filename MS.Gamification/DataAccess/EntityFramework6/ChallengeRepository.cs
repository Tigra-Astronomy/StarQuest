// This file is part of the MS.Gamification project
// 
// File: ChallengeRepository.cs  Created: 2016-03-17@01:33
// Last modified: 2016-03-17@02:54 by Fern

using MS.Gamification.Models;

namespace MS.Gamification.DataAccess.EntityFramework6
    {
    public class ChallengeRepository : Repository<Challenge>
        {
        public ChallengeRepository(ApplicationDbContext dbContext) : base(dbContext) {}
        }
    }
