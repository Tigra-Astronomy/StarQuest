// This file is part of the MS.Gamification project
// 
// File: Category.cs  Created: 2016-04-03@22:41
// Last modified: 2016-04-03@22:41 by Fern

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MS.Gamification.DataAccess;

namespace MS.Gamification.Models
    {
    public class Category : IDomainEntity<int>
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        }
    }