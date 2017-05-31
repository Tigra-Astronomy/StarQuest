// This file is part of the MS.Gamification project
// 
// File: CreateObservingSessionViewModel.cs  Created: 2017-05-16@21:07
// Last modified: 2017-05-31@13:01

using System;
using System.ComponentModel.DataAnnotations;

namespace MS.Gamification.Areas.Admin.ViewModels.ObservingSessions
    {
    public class CreateObservingSessionViewModel : EditObservingSessionViewModel
        {
        public CreateObservingSessionViewModel()
            {
            Venue = string.Empty;
            Title = string.Empty;
            Description = string.Empty;
            StartsAt = DateTime.Now;
            }


        [Display(Name = "Immediate announcement")]
        public bool SendAnnouncement { get; set; } = false;

        public override string ToString() =>
            $"{StartsAt} {Title} at {Venue}, {nameof(SendAnnouncement)}: {SendAnnouncement}, {nameof(RemindOneWeekBefore)}: {RemindOneWeekBefore}, {nameof(RemindOneDayBefore)}: {RemindOneDayBefore}";
        }
    }