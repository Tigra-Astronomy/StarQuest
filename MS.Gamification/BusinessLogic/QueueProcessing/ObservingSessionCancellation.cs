using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QueueProcessing {
    internal class ObservingSessionCancellation : QueuedWorkItem
        {
        public int ObservingSessionId { get; set; }

        public string Message { get; set; }
        }
    }