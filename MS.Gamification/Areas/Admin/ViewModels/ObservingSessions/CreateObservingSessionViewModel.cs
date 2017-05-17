// This file is part of the MS.Gamification project
// 
// File: CreateObservingSessionViewModel.cs  Created: 2017-05-16@21:07
// Last modified: 2017-05-17@03:42

using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace MS.Gamification.Areas.Admin.ViewModels.ObservingSessions
    {
    public class CreateObservingSessionViewModel
        {
        public CreateObservingSessionViewModel()
            {
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

        [Display(Name = "When", Description = "Start date and time of the session")]
        public DateTime StartsAt { get; set; }

        [CanBeNull]
        public string Description { get; set; }

        public bool SendNotifications { get; set; }
        }
    }