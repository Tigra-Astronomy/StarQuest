﻿// This file is part of the MS.Gamification project
// 
// File: JoinedBefore.cs  Created: 2016-07-20@13:06
// Last modified: 2016-07-20@13:19

using System;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic.Preconditions
    {
    public class JoinedBefore : IPredicate<ApplicationUser>
        {
        private readonly DateTime deadline;

        public JoinedBefore(DateTime deadline)
            {
            this.deadline = deadline;
            }

        public bool Evaluate(ApplicationUser candidate)
            {
            return candidate.Provisioned < deadline;
            }
        }
    }