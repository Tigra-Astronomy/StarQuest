﻿// This file is part of the MS.Gamification project
// 
// File: QueuedWorkItem.cs  Created: 2017-05-18@22:31
// Last modified: 2017-05-19@00:27

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MS.Gamification.BusinessLogic.QueueProcessing;
using MS.Gamification.DataAccess;

namespace MS.Gamification.Models
    {
    /// <summary>
    ///     Base class for queued work items
    /// </summary>
    public abstract class QueuedWorkItem : IDomainEntity<int>
        {
        /// <summary>
        ///     The earliest moment in time at which the queued item is valid for processing.
        /// </summary>
        [Index(IsUnique = false)]
        public DateTime ProcessAfter { get; set; }

        /// <summary>
        ///     The name of the queue associated with the work item
        /// </summary>
        /// <value>The name of the queue.</value>
        [Required]
        [MaxLength(8)]
        [MinLength(1)]
        [RegularExpression("[A-Za-z]")]
        public string QueueName { get; set; }

        public WorkItemDisposition Disposition { get; set; }

        public int Id { get; set; }
        }
    }