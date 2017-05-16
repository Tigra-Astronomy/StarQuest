// This file is part of the MS.Gamification project
// 
// File: ChallengeControllerSpecs.cs  Created: 2016-11-01@19:37
// Last modified: 2016-12-12@23:37

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Machine.Specifications;
using MS.Gamification.Areas.Admin.Controllers;
using MS.Gamification.Models;
using MS.Gamification.Tests.TestHelpers;
using MS.Gamification.ViewModels;

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

    [Subject(typeof(ChallengesController), "Index action")]
    class when_viewing_all_challenges : with_standard_mission<ChallengesController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => actionResult = ControllerUnderTest.Index() as ViewResult;
        It should_return_the_index_view = () => actionResult.ViewName.ShouldEqual(string.Empty);
        It should_populate_the_viewmodel_with_all_challenges_in_the_database =
            () => ((IEnumerable<Challenge>) actionResult.Model).Count().ShouldEqual(6);
        static ViewResult actionResult;
        }

    [Subject(typeof(ChallengesController), "Create Action")]
    class when_calling_the_create_action_with_no_parameters : with_mvc_controller<ChallengesController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => Result = ControllerUnderTest.Create() as ViewResult;
        It should_return_the_create_view = () => Result.ViewName.ShouldEqual(string.Empty);
        static ViewResult Result;
        }

    [Subject(typeof(ChallengesController), "Create Action POST valid data")]
    class when_calling_the_create_action_with_valid_form_data : with_standard_mission<ChallengesController>
        {
        Establish context = () =>
            {
            ValidChallenge = new CreateChallengeViewModel
                {
                BookSection = "Moon",
                Points = 1,
                CategoryId = 10,
                Location = "Moon",
                Name = "See all the moon phases",
                MissionTrackId = 1,
                ValidationImage = "no-image"
                };
            ControllerUnderTest = ContextBuilder.Build();
            originalCount = UnitOfWork.Challenges.GetAll().Count();
            };
        Because of = () => Result = ControllerUnderTest.Create(ValidChallenge) as RedirectToRouteResult;
        It should_return_redirect_to_index = () => Result.RouteValues["Action"].ShouldEqual("Index");
        It should_add_one_item_to_the_challenges_repository =
            () => ContextBuilder.UnitOfWork.Challenges.GetAll().Count().ShouldEqual(originalCount + 1);
        static RedirectToRouteResult Result;
        static CreateChallengeViewModel ValidChallenge;
        static int originalCount;
        }

    [Subject(typeof(ChallengesController), "ConfirmDelete Action")]
    class when_calling_the_confirm_delete_action_with_valid_id : with_mvc_controller<ChallengesController>
        {
        Establish context = () =>
            {
            ControllerUnderTest = ContextBuilder
                .WithData(d => d
                    .WithEntity(new Category {Id = 1})
                    .WithMissionLevel().WithTrack(1)
                    .WithChallenge("See all the moon phases").WithId(1).InCategory(1).BuildChallenge()
                    .WithChallenge("See Saturn").WithId(2).InCategory(1).BuildChallenge()
                    .BuildTrack().BuildMission())
                .Build();
            };

        Because of = () => Result = ControllerUnderTest.ConfirmDelete(1);
        It should_leave_only_id_2_in_the_repository =
            () => ContextBuilder.UnitOfWork.Challenges.GetAll().Single().Id.ShouldEqual(2);
        static ActionResult Result;
        }

    [Subject(typeof(ChallengesController), "Delete Action")]
    class when_calling_the_delete_action_with_a_valid_id : with_standard_mission<ChallengesController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => Result = ControllerUnderTest.Delete(100) as ViewResult;
        It should_return_the_delete_view = () => Result.ViewName.ShouldEqual(string.Empty);
        It should_populate_the_viewModel_with_the_item_to_be_deleted =
            () => (Result.Model as Challenge).Id.ShouldEqual(100);
        static ViewResult Result;
        }

    [Subject(typeof(ChallengesController), "Create Action POST invalid data")]
    class when_calling_the_create_action_with_an_invalid_model : with_mvc_controller<ChallengesController>
        {
        Establish context = () =>
            {
            InvalidChallenge = new CreateChallengeViewModel
                {
                BookSection = "Moon",
                Points = 0,
                CategoryId = 1,
                Location = "Moon",
                Name = string.Empty
                };
            ControllerUnderTest = ContextBuilder
                .WithData(d => d.WithEntity(new Category {Id = 1}))
                .Build();
            ControllerUnderTest.ValidateModel(InvalidChallenge);
            };
        Because of = () => Result = ControllerUnderTest.Create(InvalidChallenge) as ViewResult;
        It should_have_an_invalid_model_state = () => ControllerUnderTest.ModelState.IsValid.ShouldBeFalse();
        It should_return_the_create_view = () => Result.ViewName.ShouldEqual(string.Empty);
        It should_populate_the_viewmodel_with_the_posted_data =
            () => (Result.Model as CreateChallengeViewModel).ShouldBeLike(InvalidChallenge);
        It should_raise_an_error_for_name =
            () => ControllerUnderTest.ModelState[nameof(InvalidChallenge.Name)].Errors.Count.ShouldBeGreaterThan(0);
        It should_raise_an_error_for_points =
            () => ControllerUnderTest.ModelState[nameof(InvalidChallenge.Points)].Errors.Count.ShouldBeGreaterThan(0);
        It should_not_add_an_item_to_the_challenges_repository =
            () => ContextBuilder.UnitOfWork.Challenges.GetAll().ShouldBeEmpty();
        static CreateChallengeViewModel InvalidChallenge;
        static ViewResult Result;
        }

    [Subject(typeof(ChallengesController), "Edit Action POST invalid data")]
    class when_calling_the_edit_action_with_an_invalid_model : with_mvc_controller<ChallengesController>
        {
        Establish context = () =>
            {
            InvalidChallenge = new CreateChallengeViewModel
                {
                BookSection = "Moon",
                Points = 0,
                CategoryId = 1,
                Location = "Moon",
                Name = string.Empty
                };
            ControllerUnderTest = ContextBuilder
                .WithData(d => d.WithEntity(new Category {Id = 1}))
                .Build();
            ControllerUnderTest.ValidateModel(InvalidChallenge);
            };
        Because of = () => Result = ControllerUnderTest.Edit(InvalidChallenge) as ViewResult;
        It should_have_an_invalid_model_state = () => ControllerUnderTest.ModelState.IsValid.ShouldBeFalse();
        It should_return_the_edit_view = () => Result.ViewName.ShouldEqual(string.Empty);
        It should_populate_the_viewmodel_with_the_posted_data =
            () => (Result.Model as CreateChallengeViewModel).ShouldBeLike(InvalidChallenge);
        It should_raise_an_error_for_name =
            () => ControllerUnderTest.ModelState[nameof(InvalidChallenge.Name)].Errors.Count.ShouldBeGreaterThan(0);
        It should_raise_an_error_for_points =
            () => ControllerUnderTest.ModelState[nameof(InvalidChallenge.Points)].Errors.Count.ShouldBeGreaterThan(0);
        It should_not_add_an_item_to_the_challenges_repository = () =>
                UnitOfWork.Challenges.GetAll().ShouldBeEmpty();
        static CreateChallengeViewModel InvalidChallenge;
        static ViewResult Result;
        }

    [Subject(typeof(ChallengesController), "Edit Action")]
    class when_sending_a_get_request_to_the_edit_action_with_a_valid_id
        : with_standard_mission<ChallengesController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => Result = ControllerUnderTest.Edit(100) as ViewResult;
        It should_return_the_edit_view = () => Result.ViewName.ShouldEqual(string.Empty);
        It should_populate_the_viewModel_with_the_item_to_be_edited =
            () => (Result.Model as CreateChallengeViewModel).Id.ShouldEqual(100);
        static ViewResult Result;
        }

    [Subject(typeof(ChallengesController), "Edit Action")]
    class when_sending_a_get_request_to_the_edit_action_with_an_invalid_id
        : with_standard_mission<ChallengesController>
        {
        /*
         * We put something in the database because an empty database would be a guarantee of 
         * not finding anything and that might prejudice the test. We want to know it wasn't found because
         * the IDs didn't match, not because the DB was empty.
         */
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithData(d => d
                .WithEntity(new Category {Id = 1})
                .WithEntity(new Challenge {Id = 99, CategoryId = 1, MissionTrackId = 1}))
            .Build();
        Because of = () => Result = ControllerUnderTest.Edit(3);
        It should_return_a_not_found_error = () => Result.ShouldBeOfExactType<HttpNotFoundResult>();
        static ActionResult Result;
        }

    [Subject(typeof(ChallengesController), "Edit Action")]
    class when_sending_a_post_to_the_edit_action_with_a_valid_model
        : with_standard_mission<ChallengesController>
        {
        Establish context = () =>
            {
            ModifiedChallenge = new CreateChallengeViewModel
                {
                Id = 200,
                BookSection = "Planets",
                Points = 1,
                CategoryId = 10,
                Location = "Mars",
                Name = "See Mars", MissionTrackId = 2
                };
            ControllerUnderTest = ContextBuilder.Build();
            expectedChallengeCount = UnitOfWork.Challenges.GetAll().Count();
            ControllerUnderTest.ValidateModel(ModifiedChallenge);
            };
        Because of = () => Result = ControllerUnderTest.Edit(ModifiedChallenge) as RedirectToRouteResult;
        It should_successfully_validate_the_model = () => ControllerUnderTest.ModelState.IsValid.ShouldBeTrue();
        It should_return_a_redirect_to_the_index_action = () => Result.RouteValues["Action"].ShouldEqual("Index");
        It should_update_the_repository = () =>
                UnitOfWork.Challenges.Get(200).Name.ShouldEqual("See Mars");
        It should_not_change_the_count_of_challenges =
            () => UnitOfWork.Challenges.GetAll().Count().ShouldEqual(expectedChallengeCount);
        static RedirectToRouteResult Result;
        static CreateChallengeViewModel ModifiedChallenge;
        static int expectedChallengeCount;
        }
    }