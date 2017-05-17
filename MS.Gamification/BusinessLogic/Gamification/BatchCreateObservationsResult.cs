// This file is part of the MS.Gamification project
// 
// File: BatchCreateObservationsResult.cs  Created: 2016-07-24@06:46
// Last modified: 2016-07-24@06:48

using System.Collections.Generic;

namespace MS.Gamification.BusinessLogic.Gamification
    {
    public class BatchCreateObservationsResult
        {
        public BatchCreateObservationsResult()
            {
            Errors=new Dictionary<string, string>();
            }
        /// <summary>
        ///     The total number of observations that were created successfully.
        /// </summary>
        /// <value>The succeeded.</value>
        public int Succeeded { get; set; }

        /// <summary>
        ///     The total number of observations that were not created.
        /// </summary>
        /// <value>The failed.</value>
        public int Failed { get; set; }

        /// <summary>
        ///     The error that occurred for each user.
        /// </summary>
        /// <value>The errors.</value>
        public Dictionary<string, string> Errors { get; set; }
        }
    }