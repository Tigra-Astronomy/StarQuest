// This file is part of the MS.Gamification project
// 
// File: MissionLevel.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-06@23:10

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MS.Gamification.DataAccess;

namespace MS.Gamification.Models
    {
    public class MissionLevel : IDomainEntity<int>
        {
        [Required]
        public string Name { get; set; }

        public int Level { get; set; }

        public virtual List<MissionTrack> Tracks { get; set; }

        [Required]
        public string AwardTitle { get; set; }

        public int MissionId { get; set; }

        public virtual Mission Mission { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        }
    }