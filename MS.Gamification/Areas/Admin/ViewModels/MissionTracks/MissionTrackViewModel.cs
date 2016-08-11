// This file is part of the MS.Gamification project
// 
// File: MissionTrackViewModel.cs  Created: 2016-08-11@00:02
// Last modified: 2016-08-11@00:06

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MS.Gamification.Models;

namespace MS.Gamification.Areas.Admin.ViewModels.MissionTracks
    {
    public class MissionTrackViewModel
        {
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Track number determines the order in which tracks are displayed.
        /// </summary>
        /// <value>The number.</value>
        public int Number { get; set; }


        [Required]
        public string AwardTitle { get; set; }

        public virtual Badge Badge { get; set; }

        public virtual int BadgeId { get; set; }

        public virtual int MissionLevelId { get; set; }

        public virtual MissionLevel MissionLevel { get; set; }

        public int Id { get; set; }

        public IEnumerable<SelectListItem> BadgePicker { get; set; }

        public IEnumerable<SelectListItem> LevelPicker { get; set; }
        }
    }