// This file is part of the MS.Gamification project
// 
// File: ChallengeControllerSpecs.cs  Created: 2016-03-18@20:07
// Last modified: 2016-03-18@20:07 by Fern

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FakeItEasy;
using Machine.Specifications;
using MS.Gamification.Controllers;
using MS.Gamification.DataAccess;
using MS.Gamification.DataAccess.EntityFramework6;
using MS.Gamification.Models;
using MS.Gamification.Tests.TestHelpers;

namespace MS.Gamification.Tests.Controllers
    {
    /*
    Challenge Controller should:
    - Only allow administrator access
    + Allow the user to add a new challenge
        + Create action with no parameters should return a view
        + Create action with completed valid form data
            + should add a new item to the database
            + should redirect to index
        + Create action with invalid data
            + should return the view with errors
    + Allow the user to delete a challenge
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

    [Subject(typeof(ChallengeController), "ConfirmDelete Action")]
    public class when_calling_the_confirm_delete_action_with_valid_id
    {
        Establish context = () =>
        {
            NewChallenge1 = new Challenge()
            {
                Id = "01",
                BookSection = "Moon",
                Points = 1,
                Category = "Moon",
                Location = "Moon",
                Name = "See all the moon phases"
            };

            NewChallenge2 = new Challenge()
            {
                Id = "02",
                BookSection = "Planets",
                Points = 3,
                Category = "Saturn",
                Location = "Solar System",
                Name = "See Saturn"
            };

            fakeData = new List<Challenge> { NewChallenge1, NewChallenge2 };
            uow = A.Fake<IUnitOfWork>();
            FakeRepository = A.Fake<IRepository<Challenge>>();
            A.CallTo(() => uow.ChallengesRepository).Returns(FakeRepository);
            A.CallTo(() => FakeRepository.GetMaybe("01")).Returns(new Maybe<Challenge>(NewChallenge1));
            controller = new ChallengeController(uow);
        };
        Because of = () => actionResult = controller.ConfirmDelete(NewChallenge1.Id);
        It should_delete_an_item_from_the_challenges_repository =
            () => A.CallTo(() => FakeRepository.Remove(NewChallenge1)).MustHaveHappened(Repeated.Exactly.Once);
        It should_commit_the_transaction = () => A.CallTo(() => uow.Commit()).MustHaveHappened(Repeated.Exactly.Once);

        static List<Challenge> fakeData;
        static ChallengeController controller;
        static Challenge NewChallenge1;
        static Challenge NewChallenge2;
        static IRepository<Challenge> FakeRepository;
        static IUnitOfWork uow;
        static ActionResult actionResult;
    }
    [Subject(typeof(ChallengeController), "Delete Action")]
    public class when_calling_the_delete_action_with_a_valid_id
    {
        Establish context = () =>
        {
            NewChallenge1 = new Challenge()
            {
                Id = "01",
                BookSection = "Moon",
                Points = 1,
                Category = "Moon",
                Location = "Moon",
                Name = "See all the moon phases"
            };

            NewChallenge2 = new Challenge()
            {
                Id = "02",
                BookSection = "Planets",
                Points = 3,
                Category = "Saturn",
                Location = "Solar System",
                Name = "See Saturn"
            };

            fakeData = new List<Challenge> { NewChallenge1, NewChallenge2 };
            uow = A.Fake<IUnitOfWork>();
            FakeRepository = A.Fake<IRepository<Challenge>>();
            A.CallTo(() => uow.ChallengesRepository).Returns(FakeRepository);
            A.CallTo(() => FakeRepository.GetMaybe("01")).Returns(new Maybe<Challenge>(NewChallenge1));
            controller = new ChallengeController(uow);
        };
        Because of = () => actionResult = controller.Delete(NewChallenge1.Id) as ViewResult;
        It should_return_the_delete_view = () => actionResult.ViewName.ShouldEqual(string.Empty);

        It should_populate_the_viewModel_with_the_item_to_be_deleted =
            () => (actionResult.Model as Challenge).Id.ShouldEqual(NewChallenge1.Id);

        static List<Challenge> fakeData;
        static ChallengeController controller;
        static Challenge NewChallenge1;
        static Challenge NewChallenge2;
        static IRepository<Challenge> FakeRepository;
        static IUnitOfWork uow;
        static ViewResult actionResult;
    }

    [Subject(typeof(ChallengeController), "Create Action POST invalid data")]
    public class when_calling_the_create_action_with_an_invalid_model
    {
        Establish context = () =>
        {
            fakeData = new List<Challenge>();
            Uow = A.Fake<IUnitOfWork>();
            FakeRepository = A.Fake<IRepository<Challenge>>();
            A.CallTo(() => Uow.ChallengesRepository).Returns(FakeRepository);
            controller = new ChallengeController(Uow);

            InvalidChallenge = new Challenge()
            {
                BookSection = "Moon",
                Points = 0,
                Category = "Moon",
                Location = "Moon",
                Name = String.Empty
            };

            controller.ValidateModel(InvalidChallenge);
        };
        Because of = () => actionResult = controller.Create(InvalidChallenge) as ViewResult;
        It should_have_an_invalid_model_state = () => controller.ModelState.IsValid.ShouldBeFalse();
        It should_return_the_create_view = () => actionResult.ViewName.ShouldEqual(string.Empty);

        It should_populate_the_viewmodel_with_the_posted_data =
            () => (actionResult.Model as Challenge).ShouldBeLike(InvalidChallenge);

        It should_raise_an_error_for_name =
            () => controller.ModelState[nameof(InvalidChallenge.Name)].Errors.Count.ShouldBeGreaterThan(0);

        It should_raise_an_error_for_points =
            () => controller.ModelState[nameof(InvalidChallenge.Points)].Errors.Count.ShouldBeGreaterThan(0);

        It should_not_add_an_item_to_the_challenges_repository =
            () => A.CallTo(() => FakeRepository.Add(A<Challenge>.Ignored)).MustNotHaveHappened();

        static List<Challenge> fakeData;
        static ChallengeController controller;
        static ViewResult actionResult;
        static Challenge InvalidChallenge;
        static IUnitOfWork Uow;
        static IRepository<Challenge> FakeRepository;
    }

}

