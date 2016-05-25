// This file is part of the MS.Gamification project
// 
// File: ChallengeControllerSpecs.cs  Created: 2016-05-10@22:28
// Last modified: 2016-05-25@23:27

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FakeItEasy;
using Machine.Specifications;
using Machine.Specifications.Annotations;
using MS.Gamification.Controllers;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using MS.Gamification.Tests.TestHelpers;

// ReSharper disable PublicMembersMustHaveComments

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
        + GET with valid id
        + GET with invalid id
        + POST with valid data
        + POST with invalid data
            + Model fails validation
            + Model ID not found in the database
    + Show all the challenges
        + It should return a view (the Index view of the Challenge controller)
        + The view model should contain all the challenges from the database
     */

    #region context base classes
    public class with_challenge_controller_and_fake_repository
        {
        protected static IRepository<Challenge, int> challengesRepository;

        protected static ChallengeController controller;
        protected static List<Challenge> fakeData;
        protected static IUnitOfWork uow;

        [UsedImplicitly] Cleanup after = () =>
            {
            fakeData = null;
            uow = null;
            challengesRepository = null;
            controller = null;
            };

        [UsedImplicitly] Establish context = () =>
            {
            fakeData = new List<Challenge>();
            uow = A.Fake<IUnitOfWork>();
            challengesRepository = A.Fake<IRepository<Challenge, int>>();
            A.CallTo(() => challengesRepository.GetAll()).Returns(fakeData);
            A.CallTo(() => uow.ChallengesRepository).Returns(challengesRepository);
            controller = new ChallengeController(uow);
            };
        }
    #endregion context base classes

    [Subject(typeof(ChallengeController), "Index action")]
    class when_viewing_all_challenges : with_mvc_controller<ChallengeController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithEntity(new Category {Id = 99, Name = "Unit Test"})
            .WithEntity(new Challenge {Id = 1, CategoryId = 99})
            .WithEntity(new Challenge {Id = 2, CategoryId = 99})
            .WithEntity(new Challenge {Id = 3, CategoryId = 99})
            .Build();
        Because of = () => actionResult = ControllerUnderTest.Index() as ViewResult;
        It should_return_the_index_view = () => actionResult.ViewName.ShouldEqual(string.Empty);
        It should_populate_the_viewmodel_with_all_challenges_in_the_database =
            () => ((IEnumerable<Challenge>) actionResult.Model).Count().ShouldEqual(3);
        static ViewResult actionResult;
        }

    [Subject(typeof(ChallengeController), "Create Action")]
    class when_calling_the_create_action_with_no_parameters : with_mvc_controller<ChallengeController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => Result = ControllerUnderTest.Create() as ViewResult;
        It should_return_the_create_view = () => Result.ViewName.ShouldEqual(string.Empty);
        static ViewResult Result;
        }

    [Subject(typeof(ChallengeController), "Create Action POST valid data")]
    class when_calling_the_create_action_with_valid_form_data : with_mvc_controller<ChallengeController>
        {
        Establish context = () =>
            {
            ValidChallenge = new Challenge
                {
                BookSection = "Moon",
                Points = 1,
                CategoryId = 1,
                Location = "Moon",
                Name = "See all the moon phases"
                };
            ControllerUnderTest = ContextBuilder
                .WithEntity(new Category {Id = 1, Name = "Unit Test"})
                .Build();
            };

        Because of = () => Result = ControllerUnderTest.Create(ValidChallenge) as RedirectToRouteResult;
        It should_return_redirect_to_index = () => Result.RouteValues["Action"].ShouldEqual("Index");
        It should_add_one_item_to_the_challenges_repository =
            () => ContextBuilder.UnitOfWork.ChallengesRepository.GetAll().Count().ShouldEqual(1);
        static RedirectToRouteResult Result;
        static Challenge ValidChallenge;
        }

    [Subject(typeof(ChallengeController), "ConfirmDelete Action")]
    class when_calling_the_confirm_delete_action_with_valid_id : with_mvc_controller<ChallengeController>
        {
        Establish context = () =>
            {
            ControllerUnderTest = ContextBuilder
                .WithEntity(new Category {Id = 1})
                .WithEntity(new Challenge
                    {
                    Id = 1,
                    BookSection = "Moon",
                    Points = 1,
                    CategoryId = 1,
                    Location = "Moon",
                    Name = "See all the moon phases"
                    })
                .WithEntity(new Challenge
                    {
                    Id = 2,
                    BookSection = "Planets",
                    Points = 3,
                    CategoryId = 1,
                    Location = "Solar System",
                    Name = "See Saturn"
                    })
                .Build();
            };

        Because of = () => Result = ControllerUnderTest.ConfirmDelete(1);
        It should_leave_only_id_2_in_the_repository =
            () => ContextBuilder.UnitOfWork.ChallengesRepository.GetAll().Single().Id.ShouldEqual(2);
        static ActionResult Result;
        }

    [Subject(typeof(ChallengeController), "Delete Action")]
    class when_calling_the_delete_action_with_a_valid_id : with_mvc_controller<ChallengeController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithEntity(new Category {Id = 1})
            .WithEntity(new Challenge {Id = 1, CategoryId = 1})
            .Build();
        Because of = () => Result = ControllerUnderTest.Delete(1) as ViewResult;
        It should_return_the_delete_view = () => Result.ViewName.ShouldEqual(string.Empty);
        It should_populate_the_viewModel_with_the_item_to_be_deleted =
            () => (Result.Model as Challenge).Id.ShouldEqual(1);
        static ViewResult Result;
        }

    [Subject(typeof(ChallengeController), "Create Action POST invalid data")]
    class when_calling_the_create_action_with_an_invalid_model : with_mvc_controller<ChallengeController>
        {
        Establish context = () =>
            {
            InvalidChallenge = new Challenge
                {
                BookSection = "Moon",
                Points = 0,
                CategoryId = 1,
                Location = "Moon",
                Name = string.Empty
                };
            ControllerUnderTest = ContextBuilder
                .WithEntity(new Category {Id = 1})
                .Build();
            ControllerUnderTest.ValidateModel(InvalidChallenge);
            };
        Because of = () => Result = ControllerUnderTest.Create(InvalidChallenge) as ViewResult;
        It should_have_an_invalid_model_state = () => ControllerUnderTest.ModelState.IsValid.ShouldBeFalse();
        It should_return_the_create_view = () => Result.ViewName.ShouldEqual(string.Empty);
        It should_populate_the_viewmodel_with_the_posted_data =
            () => (Result.Model as Challenge).ShouldBeLike(InvalidChallenge);
        It should_raise_an_error_for_name =
            () => ControllerUnderTest.ModelState[nameof(InvalidChallenge.Name)].Errors.Count.ShouldBeGreaterThan(0);
        It should_raise_an_error_for_points =
            () => ControllerUnderTest.ModelState[nameof(InvalidChallenge.Points)].Errors.Count.ShouldBeGreaterThan(0);
        It should_not_add_an_item_to_the_challenges_repository =
            () => ContextBuilder.UnitOfWork.ChallengesRepository.GetAll().ShouldBeEmpty();
        static Challenge InvalidChallenge;
        static ViewResult Result;
        }

    [Subject(typeof(ChallengeController), "Edit Action POST invalid data")]
    class when_calling_the_edit_action_with_an_invalid_model : with_mvc_controller<ChallengeController>
        {
        Establish context = () =>
            {
            InvalidChallenge = new Challenge
                {
                BookSection = "Moon",
                Points = 0,
                CategoryId = 1,
                Location = "Moon",
                Name = string.Empty
                };
            ControllerUnderTest = ContextBuilder.WithEntity(new Category {Id = 1})
                .Build();
            ControllerUnderTest.ValidateModel(InvalidChallenge);
            };
        Because of = () => Result = ControllerUnderTest.Edit(InvalidChallenge) as ViewResult;
        It should_have_an_invalid_model_state = () => ControllerUnderTest.ModelState.IsValid.ShouldBeFalse();
        It should_return_the_edit_view = () => Result.ViewName.ShouldEqual(string.Empty);
        It should_populate_the_viewmodel_with_the_posted_data =
            () => (Result.Model as Challenge).ShouldBeLike(InvalidChallenge);
        It should_raise_an_error_for_name =
            () => ControllerUnderTest.ModelState[nameof(InvalidChallenge.Name)].Errors.Count.ShouldBeGreaterThan(0);
        It should_raise_an_error_for_points =
            () => ControllerUnderTest.ModelState[nameof(InvalidChallenge.Points)].Errors.Count.ShouldBeGreaterThan(0);
        It should_not_add_an_item_to_the_challenges_repository = () =>
            UnitOfWork.ChallengesRepository.GetAll().ShouldBeEmpty();
        static Challenge InvalidChallenge;
        static ViewResult Result;
        }

    [Subject(typeof(ChallengeController), "Edit Action")]
    class when_sending_a_get_request_to_the_edit_action_with_a_valid_id
        : with_mvc_controller<ChallengeController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithEntity(new Category {Id = 1})
            .WithEntity(new Challenge {Id = expectedId, CategoryId = 1})
            .Build();
        Because of = () => Result = ControllerUnderTest.Edit(expectedId) as ViewResult;
        It should_return_the_edit_view = () => Result.ViewName.ShouldEqual(string.Empty);
        It should_populate_the_viewModel_with_the_item_to_be_edited =
            () => (Result.Model as Challenge).Id.ShouldEqual(expectedId);
        static ViewResult Result;
        const int expectedId = 9;
        }

    [Subject(typeof(ChallengeController), "Edit Action")]
    class when_sending_a_get_request_to_the_edit_action_with_an_invalid_id
        : with_mvc_controller<ChallengeController>
        {
        /*
         * We put something in the database because an empty database would be a guarantee of 
         * not finding anything and that might prejudice the test. We want to know it wasn't found because
         * the IDs didn't match, not because the DB was empty.
         */
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithEntity(new Category {Id = 1})
            .WithEntity(new Challenge {Id = 9, CategoryId = 1})
            .Build();
        Because of = () => Result = ControllerUnderTest.Edit(3);
        It should_return_a_not_found_error = () => Result.ShouldBeOfExactType<HttpNotFoundResult>();
        static ActionResult Result;
        }

    [Subject(typeof(ChallengeController), "Edit Action")]
    class when_sending_a_post_to_the_edit_action_with_a_valid_model
        : with_mvc_controller<ChallengeController>
        {
        Establish context = () =>
            {
            ModifiedChallenge = new Challenge
                {
                Id = 1,
                BookSection = "Planets",
                Points = 1,
                CategoryId = 1,
                Location = "Mars",
                Name = "See Mars"
                };
            ControllerUnderTest = ContextBuilder
                .WithEntity(new Category {Id = 1})
                .WithEntity(new Challenge
                    {
                    Id = 1,
                    BookSection = "Moon",
                    Points = 1,
                    CategoryId = 1,
                    Location = "Moon",
                    Name = "See all the moon phases"
                    })
                .Build();
            ControllerUnderTest.ValidateModel(ModifiedChallenge);
            };
        Because of = () => Result = ControllerUnderTest.Edit(ModifiedChallenge) as RedirectToRouteResult;
        It should_successfully_validate_the_model = () => ControllerUnderTest.ModelState.IsValid.ShouldBeTrue();
        It should_return_a_redirect_to_the_index_action = () => Result.RouteValues["Action"].ShouldEqual("Index");
        It should_update_the_repository =
            () =>
                UnitOfWork.ChallengesRepository.GetAll()
                    .Contains(ModifiedChallenge, new ChallengeWithoutNavigationPropertiesComparer())
                    .ShouldBeTrue();
        It should_leave_one_item_in_the_repository = () => UnitOfWork.ChallengesRepository.GetAll().Count().ShouldEqual(1);
        static RedirectToRouteResult Result;
        static Challenge ModifiedChallenge;
        }
    }