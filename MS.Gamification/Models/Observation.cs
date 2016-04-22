// This file is part of the MS.Gamification project
// 
// File: Observation.cs  Created: 2016-04-21@23:39
// Last modified: 2016-04-22@01:12 by Fern

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MS.Gamification.DataAccess;
using TA.SoftwareLicensing.Models;

namespace MS.Gamification.Models
    {
    public class Observation : IDomainEntity<int>
        {
        public int ChallengeId { get; set; }
        public virtual Challenge Challenge { get; set; }
        public DateTime ObservationDateTimeUtc { get; set; }
        public ObservingEquipment Equipment { get; set; }
        public string ObservingSite { get; set; }
        public AntoniadiScale Seeing { get; set; }
        public TransparencyLevel Transparency { get; set; }
        public string Notes { get; set; }

        [FileNameWithoutPath]
        public string ExpectedImage { get; set; }

        [FileNameWithoutPath]
        public string SubmittedImage { get; set; }

        public ApprovalWorkflowState Status { get; set; }

        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        }
    }
