// This file is part of the MS.Gamification project
// 
// File: QuerySpecification.cs  Created: 2016-05-13@18:12
// Last modified: 2016-05-14@21:33

using System.Linq;

namespace MS.Gamification.BusinessLogic
    {
    public abstract class QuerySpecification<T> : IQuerySpecification<T> where T : class
        {
        public abstract IQueryable<T> GetQuery(IQueryable<T> items);

        public IFetchStrategy<T> FetchStrategy { get; } = new GenericFetchStrategy<T>();
        }
    }