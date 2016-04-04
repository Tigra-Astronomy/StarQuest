// This file is part of the MS.Gamification project
// 
// File: ChallengeControllerSpecs.cs  Created: 2016-03-18@20:19
// Last modified: 2016-03-23@13:04 by Fern

using System;
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

        [UsedImplicitly]
        Cleanup after = () =>
            {
            fakeData = null;
            uow = null;
            challengesRepository = null;
            controller = null;
            };

        [UsedImplicitly]
        Establish context = () =>
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
    public class when_viewing_all_challenges : with_challenge_controller_and_fake_repository
        {
        Because of = () => actionResult = controller.Index() as ViewResult;
        It should_return_the_index_view = () => actionResult.ViewName.ShouldEqual(string.Empty);

        It should_populate_the_viewmodel_with_all_challenges_in_the_database = () =>
            {
            var model = actionResult.Model as IEnumerable<Challenge>;
            model.Count().ShouldEqual(fakeData.Count());
            };

        private static ViewResult actionResult;
        }

    [Subject(typeof(ChallengeController), "Create Action")]
    public class when_calling_the_create_action_with_no_parameters : with_challenge_controller_and_fake_repository
        {
        Because of = () => Result = controller.Create() as ViewResult;
        It should_return_the_create_view = () => Result.ViewName.ShouldEqual(string.Empty);
        static ViewResult Result;
        }

    [Subject(typeof(ChallengeController), "Create Action POST valid data")]
    public class when_calling_the_create_action_with_valid_form_data : with_challenge_controller_and_fake_repository
        {
        Establish context = () =>
            {
            ValidChallenge = new Challenge()
                {
                BookSection = "Moon",
                Points = 1,
                CategoryId = 1,
                Location = "Moon",
                Name = "See all the moon phases"
                };
            };

        Because of = () => Result = controller.Create(ValidChallenge) as RedirectToRouteResult;
        It should_return_redirect_to_index = () => Result.RouteValues["Action"].ShouldEqual("Index");

        It should_add_an_item_to_the_challenges_repository =
            () => A.CallTo(() => challengesRepository.Add(ValidChallenge)).MustHaveHappened(Repeated.Exactly.Once);

        It should_commit_the_transaction = () => A.CallTo(() => uow.Commit()).MustHaveHappened(Repeated.Exactly.Once);

        static RedirectToRouteResult Result;
        static Challenge ValidChallenge;
        }

    [Subject(typeof(ChallengeController), "ConfirmDelete Action")]
    public class when_calling_the_confirm_delete_action_with_valid_id : with_challenge_controller_and_fake_repository
        {
        Establish context = () =>
            {
            NewChallenge1 = new Challenge()
                {
                Id = 1,
                BookSection = "Moon",
                Points = 1,
                CategoryId = 1,
                Location = "Moon",
                Name = "See all the moon phases"
                };

            NewChallenge2 = new Challenge()
                {
                Id = 2,
                BookSection = "Planets",
                Points = 3,
                CategoryId = 1,
                Location = "Solar System",
                Name = "See Saturn"
                };

            fakeData.Add(NewChallenge1);
            fakeData.Add(NewChallenge2);
            A.CallTo(() => challengesRepository.GetMaybe(1)).Returns(new Maybe<Challenge>(NewChallenge1));
            };

        Because of = () => Result = controller.ConfirmDelete(NewChallenge1.Id);

        It should_delete_an_item_from_the_challenges_repository =
            () => A.CallTo(() => challengesRepository.Remove(NewChallenge1)).MustHaveHappened(Repeated.Exactly.Once);

        It should_commit_the_transaction = () => A.CallTo(() => uow.Commit()).MustHaveHappened(Repeated.Exactly.Once);

        static Challenge NewChallenge1;
        static Challenge NewChallenge2;
        static ActionResult Result;
        }

    [Subject(typeof(ChallengeController), "Delete Action")]
    public class when_calling_the_delete_action_with_a_valid_id : with_challenge_controller_and_fake_repository
        {
        Establish context = () =>
            {
            NewChallenge1 = new Challenge()
                {
                Id = 1,
                BookSection = "Moon",
                Points = 1,
                CategoryId = 1,
                Location = "Moon",
                Name = "See all the moon phases"
                };

            NewChallenge2 = new Challenge()
                {
                Id = 2,
                BookSection = "Planets",
                Points = 3,
                CategoryId = 1,
                Location = "Solar System",
                Name = "See Saturn"
                };
            fakeData.Add(NewChallenge1);
            fakeData.Add(NewChallenge2);
            A.CallTo(() => challengesRepository.GetMaybe(1)).Returns(new Maybe<Challenge>(NewChallenge1));
            };

        Because of = () => Result = controller.Delete(NewChallenge1.Id) as ViewResult;
        It should_return_the_delete_view = () => Result.ViewName.ShouldEqual(string.Empty);

        It should_populate_the_viewModel_with_the_item_to_be_deleted =
            () => (Result.Model as Challenge).Id.ShouldEqual(NewChallenge1.Id);

        static Challenge NewChallenge1;
        static Challenge NewChallenge2;
        static ViewResult Result;
        }

    [Subject(typeof(ChallengeController), "Create Action POST invalid data")]
    public class when_calling_the_create_action_with_an_invalid_model : with_challenge_controller_and_fake_repository
        {
        Establish context = () =>
            {
            InvalidChallenge = new Challenge()
                {
                BookSection = "Moon",
                Points = 0,
                CategoryId = 1,
                Location = "Moon",
                Name = String.Empty
                };

            controller.ValidateModel(InvalidChallenge);
            };

        Because of = () => Result = controller.Create(InvalidChallenge) as ViewResult;
        It should_have_an_invalid_model_state = () => controller.ModelState.IsValid.ShouldBeFalse();
        It should_return_the_create_view = () => Result.ViewName.ShouldEqual(string.Empty);

        It should_populate_the_viewmodel_with_the_posted_data =
            () => (Result.Model as Challenge).ShouldBeLike(InvalidChallenge);

        It should_raise_an_error_for_name =
            () => controller.ModelState[nameof(InvalidChallenge.Name)].Errors.Count.ShouldBeGreaterThan(0);

        It should_raise_an_error_for_points =
            () => controller.ModelState[nameof(InvalidChallenge.Points)].Errors.Count.ShouldBeGreaterThan(0);

        It should_not_add_an_item_to_the_challenges_repository =
            () => A.CallTo(() => challengesRepository.Add(A<Challenge>.Ignored)).MustNotHaveHappened();

        static Challenge InvalidChallenge;
        static ViewResult Result;
        }

    [Subject(typeof(ChallengeController), "Edit Action POST invalid data")]
    public class when_calling_the_edit_action_with_an_invalid_model : with_challenge_controller_and_fake_repository
        {
        Establish context = () =>
            {
            InvalidChallenge = new Challenge()
                {
                BookSection = "Moon",
                Points = 0,
                CategoryId = 1,
                Location = "Moon",
                Name = String.Empty
                };

            controller.ValidateModel(InvalidChallenge);
            };

        Because of = () => Result = controller.Edit(InvalidChallenge) as ViewResult;
        It should_have_an_invalid_model_state = () => controller.ModelState.IsValid.ShouldBeFalse();
        It should_return_the_edit_view = () => Result.ViewName.ShouldEqual(string.Empty);

        It should_populate_the_viewmodel_with_the_posted_data =
            () => (Result.Model as Challenge).ShouldBeLike(InvalidChallenge);

        It should_raise_an_error_for_name =
            () => controller.ModelState[nameof(InvalidChallenge.Name)].Errors.Count.ShouldBeGreaterThan(0);

        It should_raise_an_error_for_points =
            () => controller.ModelState[nameof(InvalidChallenge.Points)].Errors.Count.ShouldBeGreaterThan(0);

        It should_not_add_an_item_to_the_challenges_repository =
            () => A.CallTo(() => challengesRepository.Add(A<Challenge>.Ignored)).MustNotHaveHappened();

        static Challenge InvalidChallenge;
        static ViewResult Result;
        }

    [Subject(typeof(ChallengeController), "Edit Action")]
    public class when_sending_a_get_request_to_the_edit_action_with_a_valid_id
        : with_challenge_controller_and_fake_repository
        {
        Establish context = () =>
            {
            Challenge1 = new Challenge()
                {
                Id = 1,
                BookSection = "Moon",
                Points = 1,
                CategoryId = 1,
                Location = "Moon",
                Name = "See all the moon phases"
                };

            fakeData.Add(Challenge1);
            A.CallTo(() => challengesRepository.GetMaybe(1)).Returns(new Maybe<Challenge>(Challenge1));
            };

        Because of = () => Result = controller.Edit(Challenge1.Id) as ViewResult;
        It should_return_the_edit_view = () => Result.ViewName.ShouldEqual(string.Empty);

        It should_populate_the_viewModel_with_the_item_to_be_edited =
            () => (Result.Model as Challenge).Id.ShouldEqual(Challenge1.Id);

        static Challenge Challenge1;
        static ViewResult Result;
        }

    [Subject(typeof(ChallengeController), "Edit Action")]
    public class when_sending_a_get_request_to_the_edit_action_with_an_invalid_id
        : with_challenge_controller_and_fake_repository
        {
        Establish context = () =>
            {
            Challenge1 = new Challenge()
                {
                Id = 1,
                BookSection = "Moon",
                Points = 1,
                CategoryId = 1,
                Location = "Moon",
                Name = "See all the moon phases"
                };
            A.CallTo(() => challengesRepository.GetMaybe(A<int>.Ignored)).Returns(Maybe<Challenge>.Empty);
            };

        Because of = () => Result = controller.Edit(3);
        It should_return_a_not_found_error = () => Result.ShouldBeOfExactType<HttpNotFoundResult>();

        static Challenge Challenge1;
        static ActionResult Result;
        }

    [Subject(typeof(ChallengeController), "Edit Action")]
    public class when_sending_a_post_to_the_edit_action_with_a_valid_model
        : with_challenge_controller_and_fake_repository
        {
        Establish context = () =>
            {
            UnmodifiedChallenge = new Challenge()
                {
                Id = 1,
                BookSection = "Moon",
                Points = 1,
                CategoryId = 1,
                Location = "Moon",
                Name = "See all the moon phases"
                };

            ModifiedChallenge = new Challenge()
                {
                Id = 1,
                BookSection = "Planets",
                Points = 1,
                CategoryId = 1,
                Location = "Mars",
                Name = "See Mars"
                };

            fakeData.Add(UnmodifiedChallenge);
            A.CallTo(() => challengesRepository.GetMaybe(1))
                .Returns(new Maybe<Challenge>(UnmodifiedChallenge));
            controller.ValidateModel(ModifiedChallenge);
            };

        Because of = () => Result = controller.Edit(ModifiedChallenge) as RedirectToRouteResult;
        It should_successfully_validate_the_model = () => controller.ModelState.IsValid.ShouldBeTrue();
        It should_return_a_redirect_to_the_index_action = () => Result.RouteValues["Action"].ShouldEqual("Index");
        It should_update_the_repository = () => fakeData.Single().ShouldBeLike(ModifiedChallenge);
        It should_commit_the_transaction = () => A.CallTo(() => uow.Commit()).MustHaveHappened(Repeated.Exactly.Once);

        static Challenge UnmodifiedChallenge;
        static RedirectToRouteResult Result;
        static Challenge ModifiedChallenge;
        }
    }
