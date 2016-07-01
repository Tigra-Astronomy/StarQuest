// This file is part of the MS.Gamification project
// 
// File: MissionControllerSpecs.cs  Created: 2016-07-01@07:36
// Last modified: 2016-07-01@07:44

using System.Web.Mvc;
using Machine.Specifications;
using MS.Gamification.Controllers;

namespace MS.Gamification.Tests.Controllers
    {
    [Subject(typeof(MissionController), "ID parameter omitted")]
    class when_invoking_the_level_action_with_no_id_parameter : with_mvc_controller<MissionController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => result = ControllerUnderTest.Level(null);
        It should_assume_level_1;
        static ActionResult result;
        }
    }