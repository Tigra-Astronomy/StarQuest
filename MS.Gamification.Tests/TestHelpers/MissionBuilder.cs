// This file is part of the MS.Gamification project
// 
// File: MissionBuilder.cs  Created: 2016-07-02@02:52
// Last modified: 2016-07-07@00:07

using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using MS.Gamification.Models;

namespace MS.Gamification.Tests.TestHelpers
    {
    partial class MissionBuilder<TContoller> where TContoller : ControllerBase
        {
        static int uniqueId;
        readonly string awardTitle;
        readonly ControllerContextBuilder<TContoller> context;
        readonly int missionId;
        readonly List<MissionTrack> tracks;
        int missionLevel;
        int missionLevelId;
        string missionName;

        public MissionBuilder(ControllerContextBuilder<TContoller> context, int missionId = 1)
            {
            this.context = context;
            this.missionId = missionId;
            missionLevelId = Interlocked.Increment(ref uniqueId);
            missionLevel = 1;
            missionName = "Unit Test Mission";
            awardTitle = "Unit Tester";
            tracks = new List<MissionTrack>();
            }

        public MissionBuilder<TContoller> WithId(int id)
            {
            missionLevelId = id;
            missionName = $"Unit Test Mission {id}";

            return this;
            }

        public TrackBuilder WithTrack(int trackNumber)
            {
            return new TrackBuilder(this, trackNumber);
            }

        public ControllerContextBuilder<TContoller> BuildMission()
            {
            context.WithEntity(new Mission
                {
                Title = "Unit Test Mission",
                Id = missionId
                });
            var level = new MissionLevel
                {
                AwardTitle = awardTitle,
                Id = missionLevelId,
                Level = missionLevel,
                Name = missionName,
                Tracks = tracks,
                MissionId = 1
                };
            foreach (var track in tracks)
                {
                foreach (var challenge in track.Challenges)
                    {
                    // Make sure the challenges are attached to this track and added to the dataset.
                    challenge.MissionTrackId = track.Id;
                    context.WithEntity(challenge);
                    }
                track.MissionLevelId = missionLevelId;
                context.WithEntity(track);
                }
            context.WithEntity(level);
            return context;
            }

        public MissionBuilder<TContoller> Level(int level)
            {
            missionLevel = level;
            return this;
            }
        }
    }