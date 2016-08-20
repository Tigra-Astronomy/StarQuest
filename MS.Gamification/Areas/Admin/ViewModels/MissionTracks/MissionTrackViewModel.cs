// This file is part of the MS.Gamification project
// 
// File: MissionTrackViewModel.cs  Created: 2016-08-19@04:17
// Last modified: 2016-08-20@01:54

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MS.Gamification.Areas.Admin.ViewModels.MissionTracks
    {
    public class MissionTrackViewModel
        {
        [Required]
        [Display(Name = "Track Name")]
        public string Name { get; set; }

        /// <summary>
        ///     Track number determines the order in which tracks are displayed.
        /// </summary>
        /// <value>The number.</value>
        [Display(Name = "Track Number")]
        public int Number { get; set; }


        [Required]
        [Display(Name = "Award Title")]
        public string AwardTitle { get; set; }

        [Display(Name = "Badge ID")]
        public virtual int BadgeId { get; set; }

        [Display(Name = "Level ID")]
        public virtual int MissionLevelId { get; set; }

        public int Id { get; set; }

        public IEnumerable<SelectListItem> BadgePicker { get; set; }

        public IEnumerable<SelectListItem> LevelPicker { get; set; }

        public override string ToString()
            {
            return
                $"Id: {Id}, Name: {Name}, Number: {Number}, AwardTitle: {AwardTitle}, BadgeId: {BadgeId}, MissionLevelId: {MissionLevelId}";
            }
        }
    }