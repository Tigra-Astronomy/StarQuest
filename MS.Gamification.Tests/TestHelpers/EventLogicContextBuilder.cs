// This file is part of the MS.Gamification project
// 
// File: EventLogicContextBuilder.cs  Created: 2017-05-17@19:20
// Last modified: 2017-05-18@16:28

using System;
using AutoMapper;
using MS.Gamification.App_Start;
using MS.Gamification.Tests.TestHelpers.Fakes;

namespace MS.Gamification.Tests.TestHelpers
    {
    class EventLogicContextBuilder
        {
        readonly DataContextBuilder fakeDataBuilder = new DataContextBuilder();


        public EventLogicContext Build()
            {
            var unitOfWork = fakeDataBuilder.Build();
            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<ViewModelMappingProfile>(); });
            mapperConfiguration.AssertConfigurationIsValid();
            var mapper = mapperConfiguration.CreateMapper();
            var notifier = new FakeNotificationService();

            return new EventLogicContext
                {
                UnitOfWork = unitOfWork,
                Mapper = mapper,
                Notifier = new FakeNotificationService()
                };
            }

        public EventLogicContextBuilder WithData(Action<DataContextBuilder> dataBuilder)
            {
            dataBuilder(fakeDataBuilder);
            return this;
            }
        }
    }