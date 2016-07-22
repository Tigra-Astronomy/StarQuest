// This file is part of the MS.Gamification project
// 
// File: MissionLevel.cs  Created: 2016-07-09@20:14
// Last modified: 2016-07-22@01:35

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using MS.Gamification.DataAccess;
using MS.Gamification.Properties;
using MS.Gamification.ViewModels.CustomValidation;

namespace MS.Gamification.Models
    {
    public class MissionLevel : IDomainEntity<int>
        {
        [Required]
        public string Name { get; set; }

        public int Level { get; set; }

        [Required]
        public string AwardTitle { get; set; }

        [DefaultValue("")]
        [NotNull]
        [XmlDocument("PreconditionSchema", typeof(Resources))]
        public string Precondition { get; set; } = string.Empty;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        #region Navigation
        public virtual List<MissionTrack> Tracks { get; set; }

        public int MissionId { get; set; }

        public virtual Mission Mission { get; set; }
        #endregion Navigation
        }
    }