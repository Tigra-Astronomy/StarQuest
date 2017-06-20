// This file is part of the MS.Gamification project
// 
// File: ObservingSessionIndexViewModel.cs  Created: 2017-06-20@17:34
// Last modified: 2017-06-20@17:39

using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using MS.Gamification.BusinessLogic.EventManagement;

namespace MS.Gamification.Areas.Admin.ViewModels.ObservingSessions
    {
    public class ObservingSessionIndexViewModel
        {
        public ObservingSessionIndexViewModel()
            {
            Title = string.Empty;
            }

        public int Id { get; set; }

        [Display(Name = "Status")]
        public ScheduleState ScheduleState { get; set; }

        [NotNull]
        public string Title { get; set; }

        [CanBeNull]
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
            $"{StartsAt} {Title} at {Venue}, {ScheduleState}, {nameof(RemindOneWeekBefore)}: {RemindOneWeekBefore}, {nameof(RemindOneDayBefore)}: {RemindOneDayBefore}";
        }
    }