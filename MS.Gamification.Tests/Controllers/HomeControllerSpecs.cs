// This file is part of the MS.Gamification project
// 
// Copyright © 2016 TiGra Networks, all rights reserved.
// 
// File: HomeControllerSpecs.cs  Created: 2016-03-14@00:16
// Last modified: 2016-03-14@00:41 by Fern

using System.Web.Mvc;
using Machine.Specifications;
using MS.Gamification.Controllers;

namespace MS.Gamification.Tests.Controllers
    {
    //ToDo: ensure that this is the default route.
    [Subject(typeof(HomeController))]
    public class when_calling_the_index_action_of_the_home_controller
        {
        Establish context = () => Controller = new HomeController();
        Because of = () => Result = Controller.Index() as ViewResult;
        It should_return_a_view = () => Result.ShouldNotBeNull();
        It should_be_the_default_view = () => Result.ViewName.ShouldBeEmpty();
        static HomeController Controller;
        static ViewResult Result;
        }
    }
