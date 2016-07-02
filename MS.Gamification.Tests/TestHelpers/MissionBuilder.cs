// This file is part of the MS.Gamification project
// 
// File: MissionBuilder.cs  Created: 2016-07-02@02:52
// Last modified: 2016-07-02@03:47

using System.Collections.Generic;
using System.Threading;
using MS.Gamification.Controllers;
using MS.Gamification.Models;

namespace MS.Gamification.Tests.TestHelpers
    {
    internal partial class MissionBuilder
        {
        private static int uniqueId;
        private readonly string awardTitle;
        private readonly ControllerContextBuilder<MissionController> context;
        private readonly List<MissionTrack> tracks;
        private int missionId;
        private int missionLevel;
        private string missionName;

        public MissionBuilder(ControllerContextBuilder<MissionController> context)
            {
            this.context = context;
            missionId = Interlocked.Increment(ref uniqueId);
            missionLevel = 1;
            missionName = "Unit Test Mission";
            awardTitle = "Unit Tester";
            tracks = new List<MissionTrack>();
            }

        public MissionBuilder WithId(int id)
            {
            missionId = id;
            missionName = $"Unit Test Mission {id}";

            return this;
            }

        public TrackBuilder WithTrack(int trackNumber)
            {
            return new TrackBuilder(this, trackNumber);
            }

        public ControllerContextBuilder<MissionController> BuildMission()
            {
            var mission = new Mission
                {
                AwardTitle = awardTitle,
                Id = missionId,
                Level = missionLevel,
                Name = missionName,
                Tracks = tracks
                };
            foreach (var track in tracks)
                {
                track.MissionId = missionId;
                context.WithEntity(track);
                }
            context.WithEntity(mission);
            return context;
            }

        public MissionBuilder Level(int level)
            {
            missionLevel = level;
            return this;
            }
        }
    }