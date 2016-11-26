// This file is part of the MS.Gamification project
// 
// File: BatchOperationConfirmationViewModel.cs  Created: 2016-11-01@19:37
// Last modified: 2016-11-26@00:06

using System.Collections.Generic;

namespace MS.Gamification.ViewModels
    {
    public class BatchOperationConfirmationViewModel
        {
        public BatchOperationConfirmationViewModel(string operationDescription)
            {
            OperationDescription = operationDescription;
            }

        public string OperationDescription { get; set; }

        public Dictionary<string, string> FailedAddresses { get; set; }

        public List<string> SuccessfulAddresses { get; set; }

        public int FailedTotal { get; set; }

        public int SucceededTotal { get; set; }
        }
    }