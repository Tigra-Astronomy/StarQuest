// This file is part of the MS.Gamification project
// 
// File: EffortSpecs.cs  Created: 2016-05-22@05:14
// Last modified: 2016-05-22@06:44

using System.Linq;
using Effort.Extra;
using Machine.Specifications;
using MS.Gamification.DataAccess;
using MS.Gamification.DataAccess.EntityFramework6;
using MS.Gamification.Models;
using MS.Gamification.Tests.TestHelpers;

namespace MS.Gamification.Tests
    {
    [Subject(typeof(ApplicationDbContext), "EFFORT in memory provider")]
    public class when_applying_effort
        {
        Establish context = () =>
            {
            var data = new ObjectData(TableNamingStrategy.Pluralised);
            data.Table<Category>().Add(
                new Category {Id = 1, Name = "First Unit Test Category"},
                new Category {Id = 2, Name = "Second Unit Test Category"},
                new Category {Id = 3, Name = "Third Unit Test Category"}
                );
            var loader = new ObjectDataLoader(data);
            uow = new EffortUnitOfWorkBuilder().WithData(loader).Build();
            };
        Because of = () =>
            {
            var challenge = new Challenge
                {
                Id = 99, ValidationImage = "fake.jpg", Name = "Unit Test Challenge", BookSection = "Section 1",
                CategoryId = 2, Location = "Here", Points = 10
                };
            uow.ChallengesRepository.Add(challenge);
            uow.Commit();
            };
        Cleanup after = () => uow.Dispose();
        It should_work_just_like_a_real_database = () => uow.ChallengesRepository.GetAll().Count().ShouldEqual(1);
        static IUnitOfWork uow;
        }
    }