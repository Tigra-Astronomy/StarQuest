// This file is part of the MS.Gamification project
// 
// File: FakeRepositoryBuilder.cs  Created: 2016-05-22@03:44
// Last modified: 2016-05-22@05:28

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FakeItEasy;
using MS.Gamification.BusinessLogic;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;

namespace MS.Gamification.Tests.TestHelpers
    {
    class FakeRepositoryBuilder<TEntity, TKey> where TEntity : class, IDomainEntity<TKey>
        {
        readonly Dictionary<TKey, TEntity> fakeData = new Dictionary<TKey, TEntity>();

        public IRepository<TEntity, TKey> Build()
            {
            var repository = A.Fake<IRepository<TEntity, TKey>>();
            A.CallTo(() => repository.Get(A<TKey>.Ignored)).ReturnsLazily((TKey id) => fakeData[id]);
            A.CallTo(() => repository.GetAll()).Returns(fakeData.Values.AsEnumerable());
            A.CallTo(() => repository.Add(A<TEntity>.Ignored)).Invokes((TEntity e) => fakeData[e.Id] = e);
            A.CallTo(() => repository.Find(A<Expression<Func<TEntity, bool>>>.Ignored))
                .ReturnsLazily((Func<TEntity, bool> predicate) => fakeData.Values.Where(predicate).AsEnumerable());
            A.CallTo(() => repository.GetMaybe(A<TKey>.Ignored))
                .ReturnsLazily((TKey id) => new Maybe<TEntity>(fakeData[id]));
            A.CallTo(() => repository.Remove(A<TEntity>.Ignored)).Invokes((TEntity e) => fakeData.Remove(e.Id));
            A.CallTo(() => repository.Single(A<IQuerySpecification<TEntity>>.Ignored))
                .ReturnsLazily(
                    (IQuerySpecification<TEntity> spec) => spec.GetQuery(fakeData.Values.AsQueryable()).Single());
            A.CallTo(() => repository.AllSatisfying(A<IQuerySpecification<TEntity>>.Ignored))
                .ReturnsLazily(
                    (IQuerySpecification<TEntity> spec) => spec.GetQuery(fakeData.Values.AsQueryable()).ToList());
            A.CallTo(() => repository.PickList)
                .ReturnsLazily(() => fakeData.Values.Select(item => new PickListItem<TKey>(item.Id, item.ToString())));
            return repository;
            }
        }

    class FakeUnitOfWorkBuilder
        {
        public IUnitOfWork Build()
            {
            var challenges = new FakeRepositoryBuilder<Challenge, int>().Build();
            var categories = new FakeRepositoryBuilder<Category, int>().Build();
            var uow = A.Fake<IUnitOfWork>();
            A.CallTo(() => uow.CategoriesRepository).Returns(categories);
            A.CallTo(() => uow.ChallengesRepository).Returns(challenges);
            return uow;
            }
        }
    }