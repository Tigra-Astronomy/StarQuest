namespace MS.Gamification.GameLogic.Preconditions
    {
    static class PreconditionExtensions
        {
        public static bool SatisfiesPrecondition<T>(this T candidate, IPredicate<T> condition)
            {
            return condition.Evaluate(candidate);
            }
        }
    }