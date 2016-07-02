// This file is part of the MS.Gamification project
// 
// File: MissionBuilder.ChallengeBuilder.cs  Created: 2016-07-02@16:26
// Last modified: 2016-07-02@18:24

using System.Threading;
using System.Web.Mvc;
using MS.Gamification.Models;

namespace MS.Gamification.Tests.TestHelpers
    {
    internal partial class MissionBuilder<TContoller> where TContoller : ControllerBase
        {
        internal partial class TrackBuilder
            {
            internal class ChallengeBuilder
                {
                private static int uniqueId;
                private readonly int awardPoints = 1;
                private readonly string bookSection = "Unspecified";
                private readonly string location = "Nowhere";
                private readonly TrackBuilder trackBuilder;
                private readonly string validationImage = "NoImage.png";
                private int categoryId = 1;
                private int challengeId;
                private string challengeName = "No name";

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