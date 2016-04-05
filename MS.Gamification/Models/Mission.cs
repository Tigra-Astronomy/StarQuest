// This file is part of the MS.Gamification project
// 
// File: Mission.cs  Created: 2016-04-05@22:45
// Last modified: 2016-04-05@23:34 by Fern

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MS.Gamification.DataAccess;

namespace MS.Gamification.Models
    {
    public class Mission : IDomainEntity<int>
        {
        [Required]
        public string Name { get; set; }
        public int Level { get; set; }
        public virtual List<MissionTrack> Tracks { get; set; }
        [Required]
        public string AwardTitle { get; set; }
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        }
    }
