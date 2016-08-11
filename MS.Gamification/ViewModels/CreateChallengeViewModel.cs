// This file is part of the MS.Gamification project
// 
// File: CreateChallengeViewModel.cs  Created: 2016-07-09@20:19
// Last modified: 2016-08-11@00:53

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MS.Gamification.ViewModels
    {
    public class CreateChallengeViewModel
        {
        //ToDo: Eventually this must be supplied
        //[Required]
        //[FileNameWithoutPath]
        //[MaxLength(255)]
        //public string ValidationImage { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int Points { get; set; }

        public string Location { get; set; }

        public string BookSection { get; set; }

        [Display(Name = "Observation Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Mission Track")]
        public int MissionTrackId { get; set; }

        public string ValidationImage { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> CategoryPicker { get; set; }

        public IEnumerable<SelectListItem> TrackPicker { get; set; }
        }
    }