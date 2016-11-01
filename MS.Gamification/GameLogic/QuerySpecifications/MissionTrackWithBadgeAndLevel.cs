using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic.QuerySpecifications
    {
    public class MissionTrackWithBadgeAndLevel : QuerySpecification<MissionTrack>
        {
        public MissionTrackWithBadgeAndLevel()
            {
            FetchStrategy.Include(p => p.Badge);
            FetchStrategy.Include(p => p.MissionLevel);
            }
        public override IQueryable<MissionTrack> GetQuery(IQueryable<MissionTrack> items)
            {
            return items;
            }
        }
    }