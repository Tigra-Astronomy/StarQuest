// This file is part of the MS.Gamification project
// 
// File: MissionTrack.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-27@19:56

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

        [ForeignKey("BadgeId")]
        public virtual Badge Badge { get; set; }

        public virtual int BadgeId { get; set; }

        public virtual int MissionLevelId { get; set; }

        public virtual MissionLevel MissionLevel { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        }
    }