// This file is part of the MS.Gamification project
// 
// File: IObservingSessionManager.cs  Created: 2017-05-17@19:43
// Last modified: 2017-05-18@18:23

using System.Threading.Tasks;
using MS.Gamification.Areas.Admin.ViewModels.ObservingSessions;

namespace MS.Gamification.BusinessLogic.EventManagement
    {
    internal interface IObservingSessionManager
        {
        Task CreateAsync(CreateObservingSessionViewModel model);
        }
    }