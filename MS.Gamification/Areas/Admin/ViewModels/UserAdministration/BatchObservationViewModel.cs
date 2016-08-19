// This file is part of the MS.Gamification project
// 
// File: BatchObservationViewModel.cs  Created: 2016-08-19@04:17
// Last modified: 2016-08-19@23:36

using System.Collections.Generic;
using System.Web.Mvc;
using MS.Gamification.ViewModels;

namespace MS.Gamification.Areas.Admin.ViewModels.UserAdministration
    {
    /// <summary>
    ///     Contains all of the details for an observation, plus a list of users.
    /// </summary>
    /// <seealso cref="ObservationDetailsViewModel" />
    public class BatchObservationViewModel : ObservationDetailsViewModel
        {
        public List<string> Users { get; set; }

        public int ChallengeId { get; set; }

        public IEnumerable<SelectListItem> ChallengePicker { get; set; }

        public IEnumerable<SelectListItem> SeeingPicker { get; set; }

        public IEnumerable<SelectListItem> TransparencyPicker { get; set; }

        public IEnumerable<SelectListItem> EquipmentPicker { get; set; }
        }
    }