using System.Collections.Generic;
using Machine.Specifications;
using MS.Gamification.GameLogic.Preconditions;
using MS.Gamification.Models;
using MS.Gamification.Tests.TestHelpers;

namespace MS.Gamification.Tests.GameLogic
    {
    class with_game_logic_context
        {
        protected static GameLogicContextBuilder GameContextBuilder;
        protected static GameLogicContext GameContext;
        Cleanup after = () =>
            {
            GameContext = null;
            GameContextBuilder = null;
            };
        Establish context = () => GameContextBuilder = new GameLogicContextBuilder();

        protected static ApplicationUser User => GameContext.User;

        protected static LevelPreconditionParser Parser => GameContext.Parser;

        protected static IPredicate<ApplicationUser> Precondition => GameContext.Precondition;

        protected static IEnumerable<Badge> Badges => GameContext.Badges;
        }
    }