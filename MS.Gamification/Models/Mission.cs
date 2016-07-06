// This file is part of the MS.Gamification project
// 
// File: Mission.cs  Created: 2016-07-06@22:09
// Last modified: 2016-07-06@22:15

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MS.Gamification.DataAccess;

namespace MS.Gamification.Models
    {
    public class Mission : IDomainEntity<int>
        {
        [Required][Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required][MaxLength(128)]
        public string Title { get; set; }

        public virtual List<MissionLevel> MissionLevels { get; set; }
        }
    }