using System.Collections.Generic;

namespace MS.Gamification.ViewModels
    {
    public class CreateUsersConfirmationViewModel {
        public Dictionary<string, string> FailedAddresses { get; set; }

        public List<string> SuccessfulAddresses { get; set; }

        public int FailedTotal { get; set; }

        public int SucceededTotal { get; set; }
        }
    }