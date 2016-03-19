// This file is part of the MS.Gamification project
// 
// File: ChallengeControllerSpecs.cs  Created: 2016-03-18@20:07
// Last modified: 2016-03-18@20:07 by Fern

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FakeItEasy;
using Machine.Specifications;
using MS.Gamification.Controllers;
using MS.Gamification.DataAccess;
using MS.Gamification.DataAccess.EntityFramework6;
using MS.Gamification.Models;

namespace MS.Gamification.Tests.Controllers
    {
    /*
    Challenge Controller should:
    - Allow the user to add a new challenge
        + Create action with no parameters should return a view
        - Create action with completed valid form data
            - should add a new item to the database
            + should redirect to index
        - Create action with invalid data
            - should return the view with errors
    - Allow the user to delete a challenge
    - Allow the user to update a challenge
    + Show all the challenges
        + It should return a view (the Index view of the Challenge controller)
        + The view model should contain all the challenges from the database
    - Show all challenges according to some criteria
     */

    [Subject(typeof(ChallengeController), "Index action")]
    public class when_viewing_all_challenges
    {
        Establish context = () =>
            {
                fakeData = new List<Challenge> { new Challenge(), new Challenge() };
                var uow = A.Fake<IUnitOfWork>();
                var challengesRepository = A.Fake<IRepository<Challenge>>();
                A.CallTo(() => challengesRepository.GetAll()).Returns(fakeData);
                A.CallTo(() => uow.ChallengesRepository).Returns(challengesRepository);
                controller = new ChallengeController(uow);
            };
        Because of = () => actionResult = controller.Index() as ViewResult;
        It should_return_the_index_view = () => actionResult.ViewName.ShouldEqual(string.Empty);
        It should_populate_the_viewmodel_with_all_challenges_in_the_database = () =>
            {
                var model = actionResult.Model as IEnumerable<Challenge>;
                model.Count().ShouldEqual(fakeData.Count());
            };

        private static ChallengeController controller;
        private static ViewResult actionResult;
        static List<Challenge> fakeData;
    }

    [Subject(typeof(ChallengeController), "Create Action")]
    public class when_calling_the_create_action_with_no_parameters
    {
        Establish context = () =>
        {
            fakeData = new List<Challenge> { new Challenge(), new Challenge() };
            var uow = A.Fake<IUnitOfWork>();
            var challengesRepository = A.Fake<IRepository<Challenge>>();
            //A.CallTo(() => challengesRepository.GetAll()).Returns(fakeData);
            //A.CallTo(() => uow.ChallengesRepository).Returns(challengesRepository);
            controller = new ChallengeController(uow);
        };
        Because of = () => actionResult = controller.Create() as ViewResult;
        It should_return_the_create_view = () => actionResult.ViewName.ShouldEqual(string.Empty);

        static List<Challenge> fakeData;
        static ChallengeController controller;
        static ViewResult actionResult;
    }

    [Subject(typeof(ChallengeController), "Create Action POST valid data")]
    public class when_calling_the_create_action_with_valid_form_data
    {
        Establish context = () =>
            {
            fakeData = new List<Challenge>();
            Uow = A.Fake<IUnitOfWork>();
            FakeRepository = A.Fake<IRepository<Challenge>>();
            A.CallTo(() => Uow.ChallengesRepository).Returns(FakeRepository);

                controller = new ChallengeController(Uow);
            ValidChallenge = new Challenge()
                {
                BookSection = "Moon",
                Points = 1,
                Category = "Moon",
                Location = "Moon",
                Name = "See all the moon phases"
                };
            };
        Because of = () => actionResult = controller.Create(ValidChallenge) as RedirectToRouteResult;
        It should_return_redirect_to_index = () => actionResult.RouteValues["Action"].ShouldEqual("Index");

        It should_add_an_item_to_the_challenges_repository =
            () => A.CallTo(() => FakeRepository.Add(ValidChallenge)).MustHaveHappened(Repeated.Exactly.Once);
        It should_commit_the_transaction = () => A.CallTo(() => Uow.Commit()).MustHaveHappened(Repeated.Exactly.Once);

        static List<Challenge> fakeData;
        static ChallengeController controller;
        static RedirectToRouteResult actionResult;
        static Challenge ValidChallenge;
        static IUnitOfWork Uow;
        static IRepository<Challenge> FakeRepository;
    }
}