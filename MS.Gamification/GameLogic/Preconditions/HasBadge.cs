﻿// This file is part of the MS.Gamification project
// 
// File: HasBadge.cs  Created: 2016-07-20@10:54
// Last modified: 2016-07-21@01:41

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic.Preconditions
    {
    public class HasBadge : IPredicate<ApplicationUser>
        {
        private readonly int badgeId;

        public HasBadge(int badgeId)
            {
            this.badgeId = badgeId;
            }

        public bool Evaluate(ApplicationUser candidate)
            {
            return candidate.Badges.Any(p => p.Id == badgeId);
            }
        }
    }