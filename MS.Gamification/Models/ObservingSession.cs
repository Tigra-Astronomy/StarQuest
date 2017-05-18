// This file is part of the MS.Gamification project
// 
// File: ObservingSession.cs  Created: 2017-05-16@19:02
// Last modified: 2017-05-18@17:17

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using MS.Gamification.BusinessLogic.EventManagement;
using MS.Gamification.DataAccess;

namespace MS.Gamification.Models
    {
    /// <summary>
    ///     Represents a planned observing session where observations
    ///     will be automatically awarded to attendees.
    /// </summary>
    /// <seealso cref="IDomainEntity{TKey}" />
    public class ObservingSession : IDomainEntity<int>
        {
        public ObservingSession()
            {
            Attendees = new List<ApplicationUser>();
            Venue = string.Empty;
            Title = string.Empty;
            Description = string.Empty;
            StartsAt = DateTime.Now;
            }

        [Required]
        [NotNull]
        public string Title { get; set; }

        [Required]
        [NotNull]
        public string Venue { get; set; }

        public DateTime StartsAt { get; set; }

        [CanBeNull]
        public string Description { get; set; }

        public bool SendNotifications { get; set; }

        [NotNull]
        [ItemNotNull]
        public List<ApplicationUser> Attendees { get; set; }

        public ScheduleState ScheduleState { get; set; }

        public int Id { get; set; }
        }
    }