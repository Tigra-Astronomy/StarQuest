// This file is part of the MS.Gamification project
// 
// File: SubmitObservationViewModel.cs  Created: 2016-07-09@20:14
// Last modified: 2016-08-11@00:42

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MS.Gamification.Models;
using MS.Gamification.ViewModels.CustomValidation;

namespace MS.Gamification.ViewModels
    {
    public class SubmitObservationViewModel
        {
        public virtual Challenge Challenge { get; set; }

        [Required]
        [Display(Name = "Observation date and time")]
        public DateTime ObservationDateTimeLocal { get; set; }

        [Required]
        [Display(Name = "Equipment used")]
        public ObservingEquipment Equipment { get; set; }

        [Required]
        [Display(Name = "Observing site")]
        public string ObservingSite { get; set; }

        [Required]
        public AntoniadiScale Seeing { get; set; }

        [Required]
        public TransparencyLevel Transparency { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [ImageIdentifier]
        [Required]
        public string SubmittedImage { get; set; }

        public List<string> ValidationImages { get; set; }

        public IEnumerable<SelectListItem> EquipmentPicker { get; set; }

        public IEnumerable<SelectListItem> SeeingPicker { get; set; }

        public IEnumerable<SelectListItem> TransparencyPicker { get; set; }
        }
    }