// This file is part of the MS.Gamification project
// 
// File: ObservationBuilder.cs  Created: 2016-07-04@01:05
// Last modified: 2016-07-04@19:45

using System;
using System.Threading;
using System.Web.Mvc;
using MS.Gamification.Models;

namespace MS.Gamification.Tests.TestHelpers
    {
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
        readonly string observingSite = "Nowhere";
        readonly AntoniadiScale seeing = AntoniadiScale.Unknown;
        readonly string submittedImage = MissingImage;
        readonly TransparencyLevel transparency = TransparencyLevel.Unknown;
        int challengeId = 100;
        int observationId;
        ModerationState observationStatus = ModerationState.Approved;
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

        public ObservationBuilder<TController> ForUserId(string user)
            {
            userId = user;
            return this;
            }

        public ObservationBuilder<TController> AwaitingModeration()
            {
            observationStatus = ModerationState.AwaitingModeration;
            return this;
            }

        public ObservationBuilder<TController> WithId(int id)
            {
            observationId = id;
            return this;
            }

        public ObservationBuilder<TController> Rejected()
            {
            observationStatus = ModerationState.Rejected;
            return this;
            }

        public ObservationBuilder<TController> Approved()
            {
            observationStatus = ModerationState.Approved;
            return this;
            }
        }
    }