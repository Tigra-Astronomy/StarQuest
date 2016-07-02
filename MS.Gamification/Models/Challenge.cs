// This file is part of the MS.Gamification project
// 
// File: Challenge.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-02@16:23

using System.ComponentModel.DataAnnotations;
using MS.Gamification.DataAccess;
using TA.SoftwareLicensing.Models;

namespace MS.Gamification.Models
    {
    public class Challenge : IDomainEntity<int>
        {
        internal const string NoImagePlaceholder = "NoImage.png";


        [Required]
        [FileNameWithoutPath]
        [MaxLength(255)]
        public string ValidationImage { get; set; } = NoImagePlaceholder;

        [Required]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int Points { get; set; }

        public string Location { get; set; }

        //ToDo: This property seems like a bad fit, since we may have challenges that are not in the book, or in a different book
        public string BookSection { get; set; }

        public int Id { get; set; }

        #region Navigation properties
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int MissionTrackId { get; set; }

        public virtual MissionTrack MissionTrack { get; set; }
        #endregion Navigation properties
        }
    }