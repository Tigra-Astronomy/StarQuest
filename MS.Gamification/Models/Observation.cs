// This file is part of the MS.Gamification project
// 
// File: Observation.cs  Created: 2016-05-10@22:29
// Last modified: 2016-07-14@01:02

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MS.Gamification.DataAccess;

namespace MS.Gamification.Models
    {
    public class Observation : IDomainEntity<int>
        {
        public int ChallengeId { get; set; }

        public virtual Challenge Challenge { get; set; }

        [Display(Name = "Date and Time")]
        public DateTime ObservationDateTimeUtc { get; set; }

        [Display(Name = "Equipment")]
        public ObservingEquipment Equipment { get; set; }

        [Display(Name = "Observing Site")]
        public string ObservingSite { get; set; }

        public AntoniadiScale Seeing { get; set; }

        public TransparencyLevel Transparency { get; set; }

        public ApplicationUser User { get; set; }

        public string UserId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [FileNameWithoutPath]
        public string ExpectedImage { get; set; }

        [FileNameWithoutPath]
        public string SubmittedImage { get; set; }

        public ModerationState Status { get; set; }

        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        }
    }