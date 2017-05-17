using MS.Gamification.Areas.Admin.ViewModels.ObservingSessions;

namespace MS.Gamification.BusinessLogic.EventManagement {
    internal interface IObservingSessionManager {
        void Create(CreateObservingSessionViewModel model);
        }
    }