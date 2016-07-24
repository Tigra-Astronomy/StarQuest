// This file is part of the MS.Gamification project
// 
// File: BatchObservationViewModel.cs  Created: 2016-07-24@03:03
// Last modified: 2016-07-24@03:54

using System.Collections.Generic;

namespace MS.Gamification.ViewModels.UserAdministration
    {
    /// <summary>
    ///     Contains all of the details for an observation, plus a list of users.
    /// </summary>
    /// <seealso cref="ObservationDetailsViewModel" />
    public class BatchObservationViewModel : ObservationDetailsViewModel
        {
        public List<string> Users { get; set; }

        public int ChallengeId { get; set; }
        }
    }