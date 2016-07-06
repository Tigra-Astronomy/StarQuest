// This file is part of the MS.Gamification project
// 
// File: BuilderExtensions.cs  Created: 2016-07-02@02:53
// Last modified: 2016-07-07@00:06

using System.Web.Mvc;

namespace MS.Gamification.Tests.TestHelpers
    {
    static class BuilderExtensions
        {
        public static MissionBuilder<TController> WithMissionLevel<TController>(
            this ControllerContextBuilder<TController> context, int missionId = 1)
            where TController : ControllerBase
            {
            return new MissionBuilder<TController>(context, missionId);
            }

        public static ObservationBuilder<TController> WithObservation<TController>(
            this ControllerContextBuilder<TController> context)
            where TController : ControllerBase
            {
            return new ObservationBuilder<TController>(context);
            }
        }
    }