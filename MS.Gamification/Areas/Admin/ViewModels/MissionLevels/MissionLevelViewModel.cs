// This file is part of the MS.Gamification project
// 
// File: MissionLevelViewModel.cs  Created: 2016-08-20@02:18
// Last modified: 2016-08-20@02:23

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using JetBrains.Annotations;
using MS.Gamification.Properties;
using MS.Gamification.ViewModels.CustomValidation;

namespace MS.Gamification.Areas.Admin.ViewModels.MissionLevels
    {
    public class MissionLevelViewModel
        {
        [Required]
        public string Name { get; set; }

        public int Level { get; set; }

        [Required]
        public string AwardTitle { get; set; }

        [DefaultValue("")]
        [NotNull]
        [AllowHtml]
        [XmlDocument("PreconditionSchema", typeof(Resources))]
        [DataType(DataType.MultilineText)]
        public string Precondition { get; set; } = string.Empty;

        public int Id { get; set; }

        public int MissionId { get; set; }

        public IEnumerable<SelectListItem> MissionPicker { get; set; }
        }
    }