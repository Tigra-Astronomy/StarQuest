// This file is part of the MS.Gamification project
// 
// File: MissionBuilder.cs  Created: 2016-07-02@02:52
// Last modified: 2016-07-02@18:27

using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using MS.Gamification.Models;

namespace MS.Gamification.Tests.TestHelpers
    {
    internal partial class MissionBuilder<TContoller> where TContoller : ControllerBase
        {
        private static int uniqueId;
        private readonly string awardTitle;
        private readonly ControllerContextBuilder<TContoller> context;
        private readonly List<MissionTrack> tracks;
        private int missionId;
        private int missionLevel;
        private string missionName;

        public MissionBuilder(ControllerContextBuilder<TContoller> context)
            {
            this.context = context;
            missionId = Interlocked.Increment(ref uniqueId);
            missionLevel = 1;
            missionName = "Unit Test Mission";
            awardTitle = "Unit Tester";
            tracks = new List<MissionTrack>();
            }

        public MissionBuilder<TContoller> WithId(int id)
            {
            missionId = id;
            missionName = $"Unit Test Mission {id}";

            return this;
            }

        public TrackBuilder WithTrack(int trackNumber)
            {
            return new TrackBuilder(this, trackNumber);
            }

        public ControllerContextBuilder<TContoller> BuildMission()
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
                foreach (var challenge in track.Challenges)
                    {
                    // Make sure the challenges are attached to this track and added to the dataset.
                    challenge.MissionTrackId = track.Id;
                    context.WithEntity(challenge);
                    }
                track.MissionId = missionId;
                context.WithEntity(track);
                }
            context.WithEntity(mission);
            return context;
            }

        public MissionBuilder<TContoller> Level(int level)
            {
            missionLevel = level;
            return this;
            }
        }
    }