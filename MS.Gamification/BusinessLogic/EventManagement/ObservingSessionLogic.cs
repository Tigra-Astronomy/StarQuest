// This file is part of the MS.Gamification project
// 
// File: ObservingSessionLogic.cs  Created: 2017-05-17@19:37
// Last modified: 2017-05-18@18:09

using System.Threading.Tasks;
using AutoMapper;
using MS.Gamification.Areas.Admin.ViewModels.ObservingSessions;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using NLog;
using NLog.Fluent;

namespace MS.Gamification.BusinessLogic.EventManagement
    {
    internal class ObservingSessionLogic : IObservingSessionManager
        {
        private ILogger log = LogManager.GetCurrentClassLogger();
        private IMapper mapper;
        private IUnitOfWork uow;

        public ObservingSessionLogic(IMapper mapper, IUnitOfWork uow)
            {
            this.mapper = mapper;
            this.uow = uow;
            }
        public async Task CreateAsync(CreateObservingSessionViewModel model)
            {
            log.Info().Message("Creating observing session")
                .Property(nameof(model), model)
                .Write();
            var session = mapper.Map<ObservingSession>(model);
            uow.ObservingSessions.Add(session);
            await uow.CommitAsync().ConfigureAwait(false);
            log.Debug().Message("Commit successful").Property("Entity", session);
            // ToDo - sned and/or schedule notifications
            }
        }
    }