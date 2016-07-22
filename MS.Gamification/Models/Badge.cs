// This file is part of the MS.Gamification project
// 
// File: Badge.cs  Created: 2016-07-21@12:10
// Last modified: 2016-07-22@01:18

using System.Collections.Generic;
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

        #region Navigation
        /// <summary>
        ///     The list of users who have been awarded this badge.
        /// </summary>
        /// <value>The users.</value>
        public virtual List<ApplicationUser> Users { get; set; }
        #endregion Navigation

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        }
    }