using System.Linq;

namespace MS.Gamification.BusinessLogic
    {
    public interface IQuerySpecification<T> where T : class
        {
        IQueryable<T> GetQuery(IQueryable<T> items);
        IFetchStrategy<T> FetchStrategy { get; }
        }
    }