// This file is part of the MS.Gamification project
// 
// File: QuerySpecification.cs  Created: 2016-05-13@01:21
// Last modified: 2016-05-13@01:31

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic
    {
    public interface IQuerySpecification<T> where T : class
        {
        IQueryable<T> GetQuery(IQueryable<T> items);
        IFetchStrategy<T> FetchStrategy { get; }
        }

    public abstract class QuerySpecification<T> : IQuerySpecification<T> where T:class
        {
        public abstract IQueryable<T> GetQuery(IQueryable<T> items);

        public IFetchStrategy<T> FetchStrategy { get; } = new GenericFetchStrategy<T>();
        }


    class ChallengesInCategory : QuerySpecification<Challenge>
        {
        readonly Category category;

        public ChallengesInCategory(Category category)
            {
            this.category = category;
            }

        public override IQueryable<Challenge> GetQuery(IQueryable<Challenge> items)
            {
            return from item in items
                   where item.CategoryId == category.Id
                   select item;
            }
        }
    }