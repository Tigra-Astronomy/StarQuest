// This file is part of the MS.Gamification project
// 
// File: CreateChallengeViewModel.cs  Created: 2016-07-09@20:19
// Last modified: 2016-07-09@23:53

using System.ComponentModel.DataAnnotations;

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
        }
    }