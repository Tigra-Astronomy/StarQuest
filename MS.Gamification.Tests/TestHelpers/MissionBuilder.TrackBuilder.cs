// This file is part of the MS.Gamification project
// 
// File: MissionBuilder.TrackBuilder.cs  Created: 2016-07-09@20:14
// Last modified: 2016-07-27@19:55

using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using MS.Gamification.Models;

namespace MS.Gamification.Tests.TestHelpers
    {
    partial class MissionBuilder<TContoller> where TContoller : ControllerBase
        {
        internal partial class TrackBuilder
            {
            static int uniqueId;
            readonly List<Challenge> challenges = new List<Challenge>();
            readonly MissionBuilder<TContoller> mission;
            readonly string trackAwardTitle;
            readonly Badge badge = new Badge {Id = uniqueId, ImageIdentifier = $"image-{uniqueId}", Name = $"Bade {uniqueId}"};
            int trackId;
            string trackName;
            int trackNumber;

            public TrackBuilder(MissionBuilder<TContoller> mission, int trackNumber)
                {
                this.mission = mission;
                this.trackNumber = trackNumber;
                trackId = Interlocked.Increment(ref uniqueId);
                trackAwardTitle = $"Track {trackNumber} Tester";
                trackName = $"Track {trackNumber}";
                }

            public TrackBuilder WithName(string name)
                {
                trackName = name;
                return this;
                }

            public TrackBuilder WithNumber(int n)
                {
                trackNumber = n;
                return this;
                }

            public MissionBuilder<TContoller> BuildTrack()
                {
                var track = new MissionTrack
                    {
                    Id = trackId,
                    AwardTitle = trackAwardTitle,
                    Challenges = challenges,
                    Name = trackName,
                    Number = trackNumber,
                    BadgeId = badge.Id,
                    Badge = badge
                    };
                mission.tracks.Add(track);
                return mission;
                }

            internal ChallengeBuilder WithChallenge()
                {
                return new ChallengeBuilder(this);
                }

            internal ChallengeBuilder WithChallenge(string name)
                {
                return new ChallengeBuilder(this).WithName(name);
                }

            public TrackBuilder WithId(int id)
                {
                trackId = id;
                return this;
                }
            }
        }
    }