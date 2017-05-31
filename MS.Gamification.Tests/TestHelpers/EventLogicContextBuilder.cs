// This file is part of the MS.Gamification project
// 
// File: EventLogicContextBuilder.cs  Created: 2017-05-17@19:20
// Last modified: 2017-05-30@23:34

using System;
using AutoMapper;
using MS.Gamification.App_Start;
using MS.Gamification.BusinessLogic.EventManagement;
using MS.Gamification.Tests.QueueProcessing;
using MS.Gamification.Tests.TestHelpers.Fakes;

namespace MS.Gamification.Tests.TestHelpers
    {
    class EventLogicContextBuilder
        {
        readonly DataContextBuilder fakeDataBuilder = new DataContextBuilder();
        DateTime currentDateTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        public EventLogicContext Build()
            {
            var unitOfWork = fakeDataBuilder.Build();
            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<ViewModelMappingProfile>(); });
            mapperConfiguration.AssertConfigurationIsValid();
            var mapper = mapperConfiguration.CreateMapper();
            var notifier = new FakeNotificationService();
            var clock = new UnitTestTimeProvider {UtcNow = currentDateTime};

            return new EventLogicContext
                {
                UnitOfWork = unitOfWork,
                TimeProvider = clock,
                Mapper = mapper,
                Notifier = new FakeNotificationService(),
                SessionManager = new ObservingSessionLogic(mapper, unitOfWork, clock)
                };
            }

        public EventLogicContextBuilder WithData(Action<DataContextBuilder> dataBuilder)
            {
            dataBuilder(fakeDataBuilder);
            return this;
            }

        public EventLogicContextBuilder WithCurrentDateTime(DateTime dateTime)
            {
            currentDateTime = dateTime;
            return this;
            }
        }
    }