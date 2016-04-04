// This file is part of the MS.Gamification project
// 
// File: Challenge.cs  Created: 2016-03-14@23:12
// Last modified: 2016-03-21@22:04 by Fern

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MS.Gamification.DataAccess;

namespace MS.Gamification.Models
    {
    public class Challenge : IDomainEntity<int>
        {
        [Required]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int Points { get; set; }
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }
        public string Location { get; set; }
        //ToDo: This property seems like a bad fit, since we may have challenges that are not in the book, or in a different book
        public string BookSection { get; set; }
        public int Id { get; set; }
        }
    }
