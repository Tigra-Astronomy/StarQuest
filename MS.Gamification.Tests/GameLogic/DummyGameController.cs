// This file is part of the MS.Gamification project
// 
// File: DummyGameController.cs  Created: 2016-07-24@06:19
// Last modified: 2016-07-24@06:48

using System.Web.Mvc;
using MS.Gamification.GameLogic;

namespace MS.Gamification.Tests.GameLogic
    {
    class DummyGameController : Controller
        {
        readonly IGameEngineService gameEngine;

        public DummyGameController(IGameEngineService gameEngine)
            {
            this.gameEngine = gameEngine;
            }
        }
    }