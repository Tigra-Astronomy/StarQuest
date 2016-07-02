﻿// This file is part of the MS.Gamification project
// 
// File: MissionBuilder.TrackBuilder.cs  Created: 2016-07-02@03:44
// Last modified: 2016-07-02@18:24

using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using MS.Gamification.Models;

namespace MS.Gamification.Tests.TestHelpers
    {
    internal partial class MissionBuilder<TContoller> where TContoller : ControllerBase
        {
        internal partial class TrackBuilder
            {
            private static int uniqueId;
            private readonly List<Challenge> challenges = new List<Challenge>();
            private readonly MissionBuilder<TContoller> mission;
            private readonly string trackAwardTitle;
            private readonly int trackId;
            private string trackName;
            private int trackNumber;

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
                    Number = trackNumber
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
            }
        }
    }