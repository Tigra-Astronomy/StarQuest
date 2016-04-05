using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using FakeItEasy;
using Machine.Specifications;
using MS.Gamification.DataAccess.EntityFramework6;
using MS.Gamification.Models;

namespace MS.Gamification.Tests
    {
    [Subject(typeof(ApplicationDbContext))][Ignore("Decided this wasn't worth the effort")]
    public class When_the_database_is_empty_and_a_challenge_is_created
        {
        Establish context = () =>
            {
            fakeChallenges = new List<Challenge>();
            queryableData = fakeChallenges.AsQueryable();
            fakeDbSet = A.Fake<DbSet<Challenge>>();
            fakeQueryable = fakeDbSet as IQueryable<Challenge>;
            A.CallTo(() => fakeQueryable.Provider).Returns(queryableData.Provider);
            A.CallTo(() => fakeQueryable.Expression).Returns(queryableData.Expression);
            A.CallTo(() => fakeQueryable.ElementType).Returns(queryableData.ElementType);
            A.CallTo(() => fakeQueryable.GetEnumerator()).Returns(queryableData.GetEnumerator());
            db = A.Fake<ApplicationDbContext>();
            A.CallTo(() => db.Challenges).Returns(fakeDbSet);
            challenge = new Challenge();
            };

        Because of = () =>
            {
            db.Challenges.Add(challenge);
            db.SaveChanges();
            };

        It should_add_the_challenge_to_the_repository = () => fakeChallenges.Count().ShouldEqual(1);
        static ApplicationDbContext db;
        static Challenge challenge;
        static List<Challenge> fakeChallenges;
        public static IQueryable<Challenge> fakeQueryable;
        public static IQueryable<Challenge> queryableData;
        public static DbSet<Challenge> fakeDbSet;
        }
    }