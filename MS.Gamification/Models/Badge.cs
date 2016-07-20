// This file is part of the MS.Gamification project
// 
// File: Badge.cs  Created: 2016-07-20@10:56
// Last modified: 2016-07-20@11:02

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MS.Gamification.DataAccess;
using MS.Gamification.HtmlHelpers;

namespace MS.Gamification.Models
    {
    public class Badge : IDomainEntity<int>
        {
        /// <summary>
        ///     Identifies the storage location of a badge bitmap to an <see cref="IImageStore" /> service.
        /// </summary>
        /// <value>The file identifier.</value>
        public string ImageIdentifier { get; set; }

        /// <summary>
        ///     The display name of the badge.
        /// </summary>
        /// <value>The badge name.</value>
        public string Name { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        }
    }