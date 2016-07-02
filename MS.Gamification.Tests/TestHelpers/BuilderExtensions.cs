// This file is part of the MS.Gamification project
// 
// File: BuilderExtensions.cs  Created: 2016-07-02@02:53
// Last modified: 2016-07-02@18:25

using System.Web.Mvc;

namespace MS.Gamification.Tests.TestHelpers
    {
    internal static class BuilderExtensions
        {
        public static MissionBuilder<TController> WithMission<TController>(this ControllerContextBuilder<TController> context)
            where TController : ControllerBase
            {
            return new MissionBuilder<TController>(context);
            }
        }
    }