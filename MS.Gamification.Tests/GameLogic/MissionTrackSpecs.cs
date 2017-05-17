// This file is part of the MS.Gamification project
// 
// File: MissionTrackSpecs.cs  Created: 2016-11-01@19:37
// Last modified: 2016-12-12@23:33

using System;
using System.Linq;
using Machine.Specifications;
using MS.Gamification.Areas.Admin.ViewModels.MissionTracks;
using MS.Gamification.BusinessLogic.Gamification;
using MS.Gamification.Models;
using MS.Gamification.Tests.Controllers;
using MS.Gamification.Tests.TestHelpers;

namespace MS.Gamification.Tests.GameLogic
    {
    /*
     * Behaviours for Mission Track CRUD operations
     * 
     * Concern: Create new
     * When creating a new track, and there is already a track in the target mission with the same number
     * + It should throw
     * + It should not create the track
     * When creating a new track and it references a non-existent level
     * + It should throw
     * + It should not create the track
     * When creating a new track and it references a non-existent badge
     * + It should throw
     * + It should not create the track
     * When creating a new track with valid data
     * + It should add the track to the database
     * 
     * Concern: Update existing
     * When changing a track's level association, and the target level already had that track number
     * + It should throw
     * + It should not modify the track
     * When changing a track's level association and there are observations associated with the track
     * + It should throw
     * + It should not modify the track
     * When changing an existing track with valid data
     * + It should update the database.
     * 
     * Concern: Delete track
     * When deleting a track with no associated observations
     * + It should delete the track from the database
     * When deleting a track with associated observations
     * - It should throw
     * - It should not delete the track
     */

    #region Create concern
    [Subject(typeof(GameRulesService), "Create New")]
    class when_adding_a_new_track_with_valid_data : with_standard_mission<DummyGameController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => RulesService.CreateTrackAsync(new MissionTrack
            {
            BadgeId = UnitOfWork.Badges.GetAll().First().Id,
            AwardTitle = "dummy",
            MissionLevelId = 1,
            Name = "Unit Test",
            Number = 99
            }).Await();
        It should_add_the_new_track_to_the_database =
            () => UnitOfWork.MissionTracks.GetAll().Count(p => p.Number == 99).ShouldEqual(1);
        }

    [Subject(typeof(GameRulesService), "Create New")]
    class when_adding_a_new_track_with_a_duplicate_track_number : with_standard_mission<DummyGameController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => Exception = Catch.Exception(() =>
            {
            RulesService.CreateTrackAsync(new MissionTrack
                {
                BadgeId = UnitOfWork.Badges.GetAll().First().Id,
                AwardTitle = "dummy",
                MissionLevelId = 1,
                Name = "Unit Test",
                Number = 1
                }).Await();
            });
        It should_throw = () => Exception.ShouldBeOfExactType<InvalidOperationException>();
        It should_not_add_the_new_track_to_the_database =
            () => UnitOfWork.MissionTracks.GetAll().Any(p => p.AwardTitle == "dummy").ShouldBeFalse();
        static Exception Exception;
        }

    [Subject(typeof(GameRulesService), "Create New")]
    class when_adding_a_new_track_that_references_a_non_existent_level : with_standard_mission<DummyGameController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => Exception = Catch.Exception(() =>
            {
            RulesService.CreateTrackAsync(new MissionTrack
                {
                BadgeId = UnitOfWork.Badges.GetAll().First().Id,
                AwardTitle = "dummy",
                MissionLevelId = 99,
                Name = "Unit Test",
                Number = 99
                }).Await();
            });
        It should_throw = () => Exception.ShouldBeOfExactType<InvalidOperationException>();
        It should_not_add_the_new_track_to_the_database =
            () => UnitOfWork.MissionTracks.GetAll().Any(p => p.AwardTitle == "dummy").ShouldBeFalse();
        static Exception Exception;
        }

    [Subject(typeof(GameRulesService), "Create New")]
    class when_adding_a_new_track_that_references_a_non_existent_badge : with_standard_mission<DummyGameController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => Exception = Catch.Exception(() =>
            {
            RulesService.CreateTrackAsync(new MissionTrack
                {
                BadgeId = UnitOfWork.Badges.GetAll().Max(p => p.Id) + 1,
                AwardTitle = "dummy",
                MissionLevelId = 1,
                Name = "Unit Test",
                Number = 99
                }).Await();
            });
        It should_throw = () => Exception.ShouldBeOfExactType<InvalidOperationException>();
        It should_not_add_the_new_track_to_the_database =
            () => UnitOfWork.MissionTracks.GetAll().Any(p => p.AwardTitle == "dummy").ShouldBeFalse();
        static Exception Exception;
        }
    #endregion Create concern

    #region Update concern
    [Subject(typeof(GameRulesService), "Update existing")]
    class when_updating_a_track_with_valid_data : with_standard_mission<DummyGameController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () =>
            {
            var dbTrack = UnitOfWork.MissionTracks.Get(1);
            var model = Mapper.Map<MissionTrack, MissionTrackViewModel>(dbTrack);
            model.Name = "Updated";
            RulesService.UpdateTrackAsync(model);
            };
        It should_update_the_database =
            () => UnitOfWork.MissionTracks.Get(1).Name.ShouldEqual("Updated");
        }

    [Subject(typeof(GameRulesService), "Update existing")]
    class when_updating_a_track_level_association_and_the_target_level_already_has_that_track_number
        : with_standard_mission<DummyGameController>
        {
        /*
         * Augments the standard mission with a second mission,
         *  with a single level ID-99,
         *  with a single track number 1
         */
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithData(d => d
                .WithMissionLevel(2).WithId(99).Level(1)
                .WithTrack(1).WithName("target").BuildTrack()
                .BuildMission())
            .Build();
        Because of = () =>
            {
            var dbTrack = UnitOfWork.MissionTracks.Get(1);
            var model = Mapper.Map<MissionTrack, MissionTrackViewModel>(dbTrack);
            model.Name = "Updated";
            model.MissionLevelId = 99;
            Exception = Catch.Exception(() => RulesService.UpdateTrackAsync(model).Await());
            };
        It should_throw = () => Exception.ShouldBeOfExactType<InvalidOperationException>();
        It should_not_update_the_database =
            () => UnitOfWork.MissionTracks.Get(1).MissionLevelId.ShouldEqual(1);
        static Exception Exception;
        }
    #endregion Update concern

    #region Delete concern
    [Subject(typeof(GameRulesService), "Delete track")]
    class when_deleting_a_track_with_no_associated_observations : with_standard_mission<DummyGameController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => RulesService.DeleteTrackAsync(1).Await();
        It should_delete_the_track_from_the_database = () => UnitOfWork.MissionTracks.GetMaybe(1).Any().ShouldBeFalse();
        }

    [Subject(typeof(GameRulesService), "Delete track")]
    class when_deleting_a_track_with_associated_observations : with_standard_mission<DummyGameController>
        {
        Establish context = () =>
            {
            ControllerUnderTest = ContextBuilder
                .WithData(d => d
                    .WithStandardUser("user", "Joe Test")
                    .WithObservation().ForUserId("user").ForChallenge(100).BuildObservation())
                .Build();
            };
        Because of = () => Exception = Catch.Exception(() => RulesService.DeleteTrackAsync(1).Await());
        It should_not_delete_the_track = () => UnitOfWork.MissionTracks.GetMaybe(1).Any().ShouldBeTrue();
        It should_throw = () => Exception.ShouldBeOfExactType<InvalidOperationException>();
        static Exception Exception;
        }
    #endregion Delete concern
    }