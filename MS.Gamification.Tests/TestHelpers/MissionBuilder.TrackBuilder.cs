// This file is part of the MS.Gamification project
// 
// File: MissionBuilder.TrackBuilder.cs  Created: 2016-07-02@03:44
// Last modified: 2016-07-02@03:47

using System.Collections.Generic;
using System.Threading;
using MS.Gamification.Models;

namespace MS.Gamification.Tests.TestHelpers
    {
    internal partial class MissionBuilder
        {
        internal class TrackBuilder
            {
            private static int uniqueId;
            private readonly MissionBuilder mission;
            private readonly string trackAwardTitle;
            private readonly int trackId;
            private string trackName;
            private int trackNumber;

            public TrackBuilder(MissionBuilder mission, int trackNumber)
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

            public MissionBuilder BuildTrack()
                {
                var track = new MissionTrack
                    {
                    Id = trackId,
                    AwardTitle = trackAwardTitle,
                    Challenges = new List<Challenge>(),
                    Name = trackName,
                    Number = trackNumber
                    };
                mission.tracks.Add(track);
                return mission;
                }
            }
        }
    }