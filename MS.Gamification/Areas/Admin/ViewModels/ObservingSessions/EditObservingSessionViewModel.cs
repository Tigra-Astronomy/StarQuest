// This file is part of the MS.Gamification project
// 
// File: EditObservingSessionViewModel.cs  Created: 2017-05-17@02:24
// Last modified: 2017-05-31@13:01

using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace MS.Gamification.Areas.Admin.ViewModels.ObservingSessions
    {
    public class EditObservingSessionViewModel
        {
        public int Id { get; set; }

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

        [Display(Name = "Remind one week before")]
        public bool RemindOneWeekBefore { get; set; } = false;

        [Display(Name = "Remind one day before")]
        public bool RemindOneDayBefore { get; set; } = false;

        public override string ToString() =>
            $"{StartsAt} {Title} at {Venue}, {nameof(RemindOneWeekBefore)}: {RemindOneWeekBefore}, {nameof(RemindOneDayBefore)}: {RemindOneDayBefore}";
        }
    }