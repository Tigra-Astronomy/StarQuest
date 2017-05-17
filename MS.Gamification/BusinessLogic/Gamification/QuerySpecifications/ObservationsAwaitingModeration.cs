// This file is part of the MS.Gamification project
// 
// File: ObservationsAwaitingModeration.cs  Created: 2016-07-09@20:14
// Last modified: 2016-08-18@05:00

using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MS.Gamification.App_Start;
using MS.Gamification.Models;
using MS.Gamification.ViewModels.Moderation;

namespace MS.Gamification.BusinessLogic.Gamification.QuerySpecifications
    {
    public class ObservationsAwaitingModeration : QuerySpecification<Observation, ModerationQueueItem>
        {
        public ObservationsAwaitingModeration()
            {
            Mapper.Initialize(config => config.AddProfile(new ViewModelMappingProfile()));
            }

        public override IQueryable<ModerationQueueItem> GetQuery(IQueryable<Observation> items)
            {
            //var query = from item in items
            //            where item.Status == ModerationState.AwaitingModeration
            //            select item;
            var moderationQueue = items
                .Where(p => p.Status == ModerationState.AwaitingModeration)
                .ProjectTo<ModerationQueueItem>();
            return moderationQueue;
            }
        }
    }