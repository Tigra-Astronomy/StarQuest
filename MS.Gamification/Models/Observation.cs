// This file is part of the MS.Gamification project
// 
// File: Observation.cs  Created: 2016-05-10@22:29
// Last modified: 2016-07-28@10:52

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MS.Gamification.DataAccess;
using MS.Gamification.ViewModels.CustomValidation;

namespace MS.Gamification.Models
    {
    public class Observation : IDomainEntity<int>
        {
        public virtual Challenge Challenge { get; set; }

        [Display(Name = "Date and Time")]
        public DateTime ObservationDateTimeUtc { get; set; }

        [Display(Name = "Equipment")]
        public ObservingEquipment Equipment { get; set; }

        [Display(Name = "Observing Site")]
        public string ObservingSite { get; set; }

        public AntoniadiScale Seeing { get; set; }

        public TransparencyLevel Transparency { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [ImageIdentifier]
        public string ExpectedImage { get; set; }

        [ImageIdentifier]
        public string SubmittedImage { get; set; }

        public ModerationState Status { get; set; }

        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        #region Related Entities
        public virtual ApplicationUser User { get; set; }

        public virtual string UserId { get; set; }

        public virtual int ChallengeId { get; set; }
        #endregion RelatedEntities
        }
    }