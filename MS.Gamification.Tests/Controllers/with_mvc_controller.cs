// This file is part of the MS.Gamification project
// 
// File: with_mvc_controller.cs  Created: 2016-05-26@03:51
// Last modified: 2016-07-09@22:33

using System;
using System.Web.Mvc;
using Machine.Specifications;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic;
using MS.Gamification.Tests.TestHelpers;

namespace MS.Gamification.Tests.Controllers
    {
    /// <summary>
    ///     Test context base class for testing MVC Controllers. Use of this class provides a fluent builder for
    ///     initializing a transient in-memory data context and the HTTP request context, and ensures that  everything
    ///     is correctly cleaned up at the end of the test context.
    /// </summary>
    /// <typeparam name="TController">The type of the MVC controller under test.</typeparam>
    /// <remarks>
    ///     The unit under test should be stored in the field <see cref="ControllerUnderTest" />. This should be built
    ///     using the <see cref="ControllerContextBuilder{TController}" /> in <see cref="ContextBuilder" />.
    /// </remarks>
    class with_mvc_controller<TController> where TController : ControllerBase
        {
        protected static ControllerContextBuilder<TController> ContextBuilder;
        protected static TController ControllerUnderTest;
        Cleanup after = () =>
            {
            ContextBuilder.UnitOfWork.Dispose();
            (ControllerUnderTest as IDisposable)?.Dispose();
            };
        Establish context = () => ContextBuilder = new ControllerContextBuilder<TController>();

        protected static IUnitOfWork UnitOfWork => ContextBuilder.UnitOfWork;

        protected static IGameEngineService RulesService => ContextBuilder.RulesService;

        }
    }