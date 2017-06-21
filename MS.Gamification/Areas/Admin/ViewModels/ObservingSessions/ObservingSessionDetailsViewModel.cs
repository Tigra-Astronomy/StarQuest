// This file is part of the MS.Gamification project
// 
// File: ObservingSessionDetailsViewModel.cs  Created: 2017-06-21@19:31
// Last modified: 2017-06-21@20:30

using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using MS.Gamification.BusinessLogic.EventManagement;

namespace MS.Gamification.Areas.Admin.ViewModels.ObservingSessions
    {
    public class ObservingSessionDetailsViewModel
        {
        public ObservingSessionDetailsViewModel()
            {
            Title = string.Empty;
            Venue = string.Empty;
            }

        public int Id { get; set; }

        [Display(Name = "Schedule State")]
        public ScheduleState ScheduleState { get; set; }

        [NotNull]
        public string Title { get; set; }

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

        public string Reminders
            {
            get
                {
                if (RemindOneWeekBefore && RemindOneDayBefore)
                    return "A week before and the day before";
                if (RemindOneWeekBefore)
                    return "A week before";
                if (RemindOneDayBefore)
                    return "The day before";
                return "No reminder";
                }
            }

        public override string ToString() =>
            $"{StartsAt} {Title} at {Venue}, {nameof(RemindOneWeekBefore)}: {RemindOneWeekBefore}, {nameof(RemindOneDayBefore)}: {RemindOneDayBefore}";
        }
    }