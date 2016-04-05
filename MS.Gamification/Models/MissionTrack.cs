// This file is part of the MS.Gamification project
// 
// File: MissionTrack.cs  Created: 2016-04-05@23:10
// Last modified: 2016-04-05@23:11 by Fern

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MS.Gamification.DataAccess;

namespace MS.Gamification.Models
    {
    public class MissionTrack : IDomainEntity<int>
        {
        [Required]
        public string Name { get; set; }
        public int Number { get; set; }
        public virtual List<Challenge> Challenges { get; set; }
        [Required]
        public string AwardTitle { get; set; }
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        }
    }
