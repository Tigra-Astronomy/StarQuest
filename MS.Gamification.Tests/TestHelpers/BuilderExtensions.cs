// This file is part of the MS.Gamification project
// 
// File: BuilderExtensions.cs  Created: 2016-07-02@02:53
// Last modified: 2016-07-02@03:03

using MS.Gamification.Controllers;

namespace MS.Gamification.Tests.TestHelpers
    {
    internal static class BuilderExtensions
        {
        public static MissionBuilder WithMission(this ControllerContextBuilder<MissionController> context)
            {
            return new MissionBuilder(context);
            }
        }
    }