// This file is part of the MS.Gamification project
// 
// File: SubmitObservationViewModel.cs  Created: 2016-04-24@16:52
// Last modified: 2016-05-04@21:53 by Fern

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TA.SoftwareLicensing.Models;

namespace MS.Gamification.Models
    {
    public class SubmitObservationViewModel
        {
        public virtual Challenge Challenge { get; set; }

        [Required]
        [Display(Name="Observation date and time")]
        public DateTime ObservationDateTimeLocal { get; set; }

        [Required]
        [Display (Name = "Equipment used")]
        public ObservingEquipment Equipment { get; set; }

        [Required]
        [Display (Name = "Observing site")]
        public string ObservingSite { get; set; }

        [Required]
        public AntoniadiScale Seeing { get; set; }

        [Required]
        public TransparencyLevel Transparency { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [FileNameWithoutPath]
        [Required]
        public string SubmittedImage { get; set; }

        public List<string> ValidationImages { get; set; }
        }
    }
