﻿// This file is part of the MS.Gamification project
// 
// File: Mission.cs  Created: 2016-07-06@22:15
// Last modified: 2016-08-06@00:10

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using JetBrains.Annotations;
using MS.Gamification.DataAccess;
using MS.Gamification.Properties;
using MS.Gamification.ViewModels.CustomValidation;

namespace MS.Gamification.Models
    {
    public class Mission : IDomainEntity<int>
        {
        [Required]
        [MaxLength(128)]
        public string Title { get; set; }

        [DefaultValue("")]
        [NotNull]
        [AllowHtml]
        [XmlDocument("PreconditionSchema", typeof(Resources))]
        [DataType(DataType.MultilineText)]
        public string Precondition { get; set; } = string.Empty;

        public virtual List<MissionLevel> MissionLevels { get; set; }

        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        }
    }