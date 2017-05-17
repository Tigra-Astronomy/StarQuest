// This file is part of the MS.Gamification project
// 
// File: ObservingSessionsIndex.cs  Created: 2017-05-17@02:41
// Last modified: 2017-05-17@03:37

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JetBrains.Annotations;
using MS.Gamification.App_Start;
using MS.Gamification.Areas.Admin.ViewModels.ObservingSessions;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.Gamification.QuerySpecifications
    {
    public class ObservingSessionsIndex : QuerySpecification<ObservingSession, EditObservingSessionViewModel>
        {
        private readonly IMapper mapper;
        [NotNull] private readonly MapperConfiguration mapperConfiguration;

        public ObservingSessionsIndex()
            {
            mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<ViewModelMappingProfile>(); });
            }

        [NotNull]
        public override IQueryable<EditObservingSessionViewModel> GetQuery([NotNull] IQueryable<ObservingSession> items)
            {
            Contract.Requires(items != null);
            Contract.Ensures(Contract.Result<IQueryable<EditObservingSessionViewModel>>() != null);
            var query = from observingSession in items
                        orderby observingSession.StartsAt descending
                        select observingSession;
            var modelItems = query.ProjectTo<EditObservingSessionViewModel>(mapperConfiguration);
            return modelItems;
            }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822: MarkMembersAsStatic", Justification = "Required for code contracts.")]
        [Conditional("CONTRACTS_FULL")]
        private void ObjectInvariant()
            {
            Contract.Invariant(mapperConfiguration != null);
            }
        }
    }