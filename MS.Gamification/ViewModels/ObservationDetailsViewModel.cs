﻿// This file is part of the MS.Gamification project
// 
// File: ObservationDetailsViewModel.cs  Created: 2016-07-13@23:59
// Last modified: 2016-07-14@00:43

using System;
using System.ComponentModel.DataAnnotations;
using MS.Gamification.Models;

namespace MS.Gamification.ViewModels
    {
    public class ObservationDetailsViewModel
        {
        public string ChallengeName { get; set; }

        [Display(Name = "Observation date (UTC)")]
        public DateTime ObservationDateTimeUtc { get; set; }

        [Display(Name = "Equipment used")]
        public ObservingEquipment Equipment { get; set; }

        [Display(Name = "Observing site")]
        public string ObservingSite { get; set; }

        public AntoniadiScale Seeing { get; set; }

        public TransparencyLevel Transparency { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [FileNameWithoutPath]
        public string SubmittedImage { get; set; }

        public string UserName { get; set; }
        }
    }