// This file is part of the MS.Gamification project
// 
// File: EffortSpecs.cs  Created: 2016-05-26@03:51
// Last modified: 2016-07-02@18:49

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
            // ID is DbGenerated so the 99 should be ignored.
            var category = new Category {Id = 99, Name = "The Last Category"};
            uow.CategoriesRepository.Add(category);
            uow.Commit();
            };
        Cleanup after = () => uow.Dispose();
        It should_have_4_categories = () => uow.CategoriesRepository.GetAll().Count().ShouldEqual(4);
        It should_have_category_4 = () => uow.CategoriesRepository.Get(4).Id.ShouldEqual(4);
        static IUnitOfWork uow;
        }
    }