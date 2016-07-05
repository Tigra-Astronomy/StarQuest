// This file is part of the MS.Gamification project
// 
// File: MissionBuilder.ChallengeBuilder.cs  Created: 2016-07-02@16:26
// Last modified: 2016-07-05@05:10

using System.Threading;
using System.Web.Mvc;
using MS.Gamification.Models;

namespace MS.Gamification.Tests.TestHelpers
    {
    partial class MissionBuilder<TContoller> where TContoller : ControllerBase
        {
        internal partial class TrackBuilder
            {
            internal class ChallengeBuilder
                {
                static int uniqueId;
                readonly int awardPoints = 1;
                readonly string bookSection = "Unspecified";
                readonly string location = "Nowhere";
                readonly TrackBuilder trackBuilder;
                readonly string validationImage = "NoImage.png";
                int categoryId = 10;
                int challengeId;
                string challengeName = "No name";

                public ChallengeBuilder(TrackBuilder trackBuilder)
                    {
                    this.trackBuilder = trackBuilder;
                    challengeId = Interlocked.Increment(ref uniqueId);
                    }

                public TrackBuilder BuildChallenge()
                    {
                    // the MissionTrackID property is set by the TrackBuilder.
                    var challenge = new Challenge
                        {
                        BookSection = bookSection,
                        Id = challengeId,
                        Name = challengeName,
                        CategoryId = categoryId,
                        Location = location,
                        Points = awardPoints,
                        ValidationImage = validationImage
                        };
                    trackBuilder.challenges.Add(challenge);
                    return trackBuilder;
                    }

                public ChallengeBuilder InCategory(int id)
                    {
                    categoryId = id;
                    return this;
                    }

                public ChallengeBuilder WithName(string name)
                    {
                    challengeName = name;
                    return this;
                    }

                public ChallengeBuilder WithId(int id)
                    {
                    challengeId = id;
                    return this;
                    }
                }
            }
        }
    }