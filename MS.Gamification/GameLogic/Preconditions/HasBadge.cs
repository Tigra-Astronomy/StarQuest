// This file is part of the MS.Gamification project
// 
// File: HasBadge.cs  Created: 2016-07-20@10:54
// Last modified: 2016-07-20@11:29

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic.Preconditions
    {
    public class HasBadge : IPredicate<ApplicationUser>
        {
        private readonly Badge badge;

        public HasBadge(Badge badge)
            {
            this.badge = badge;
            }

        public bool Evaluate(ApplicationUser candidate)
            {
            return candidate.Badges.Any(p => p.Id == badge.Id);
            }
        }
    }