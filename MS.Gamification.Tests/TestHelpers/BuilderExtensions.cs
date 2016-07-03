// This file is part of the MS.Gamification project
// 
// File: BuilderExtensions.cs  Created: 2016-07-02@02:53
// Last modified: 2016-07-03@00:36

using System;
using System.Threading;
using System.Web.Mvc;
using MS.Gamification.Models;

namespace MS.Gamification.Tests.TestHelpers
    {
    static class BuilderExtensions
        {
        public static MissionBuilder<TController> WithMission<TController>(this ControllerContextBuilder<TController> context)
            where TController : ControllerBase
            {
            return new MissionBuilder<TController>(context);
            }

        public static ObservationBuilder<TController> WithObservation<TController>(
            this ControllerContextBuilder<TController> context)
            where TController : ControllerBase
            {
            return new ObservationBuilder<TController>(context);
            }
        }

    class ObservationBuilder<TController> where TController : ControllerBase
        {
        const string MissingImage = "NoImage.png";
        // ReSharper disable once StaticMemberInGenericType
        static int uniqueId;
        readonly ControllerContextBuilder<TController> context;
        readonly ObservingEquipment equipment = ObservingEquipment.NakedEye;
        readonly string expectedImage = MissingImage;
        readonly string notes = "Lorem ipsum dolor sit amet";
        readonly DateTime observationDateTimeUtc = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        readonly int observationId;
        readonly ModerationState observationStatus = ModerationState.Approved;
        readonly string observingSite = "Nowhere";
        readonly AntoniadiScale seeing = AntoniadiScale.Unknown;
        readonly string submittedImage = MissingImage;
        readonly TransparencyLevel transparency = TransparencyLevel.Unknown;
        int challengeId = 100;
        string userId = "user";

        public ObservationBuilder(ControllerContextBuilder<TController> context)
            {
            this.context = context;
            observationId = Interlocked.Increment(ref uniqueId);
            }

        public ControllerContextBuilder<TController> BuildObservation()
            {
            var observation = new Observation
                {
                Id = observationId,
                ChallengeId = challengeId,
                Equipment = equipment,
                ExpectedImage = expectedImage,
                Notes = notes,
                ObservationDateTimeUtc = observationDateTimeUtc,
                ObservingSite = observingSite,
                Seeing = seeing,
                Status = observationStatus,
                SubmittedImage = submittedImage,
                Transparency = transparency,
                UserId = userId
                };
            context.WithEntity(observation);
            return context;
            }

        public ObservationBuilder<TController> ForChallenge(int challengeId)
            {
            this.challengeId = challengeId;
            return this;
            }

        public ObservationBuilder<TController> ForUser(string user)
            {
            userId = user;
            return this;
            }
        }
    }