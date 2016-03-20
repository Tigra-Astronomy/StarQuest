using System.ComponentModel.DataAnnotations;
using Microsoft.Ajax.Utilities;
using MS.Gamification.DataAccess;

namespace MS.Gamification.Models
    {
    public class Challenge : IDomainEntity
        {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(1,int.MaxValue)]
        public int Points { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        //ToDo: This property seems like a bad fit, since we may have challenges that are not in the book, or in a different book
        public string BookSection { get; set; }
        }
    }