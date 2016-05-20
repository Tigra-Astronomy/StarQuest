// This file is part of the MS.Gamification project
// 
// File: ModerationControllerSpecs.cs  Created: 2016-05-20@23:03
// Last modified: 2016-05-21@00:29

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FakeItEasy;
using Machine.Specifications;
using MS.Gamification.BusinessLogic;
using MS.Gamification.Controllers;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;

namespace MS.Gamification.Tests.Controllers
    {
    /*
     * ModerationController specs
     * 
     * Given: a logged in moderator, some observations with status Submitted
     * When: a call is made to the index action
     * Then: 
     *  it should return the index view
     *  it should populate the ViewModel with the observations  with Status AwaitingModeration
     */

    [Subject(typeof(ModerationController), "Index")]
    public class when_the_index_action_is_called
        {
        Establish context = () =>
            {
            Observations = new List<Observation>
                {
                new Observation {Id = 1, Status = ModerationState.Approved},
                new Observation {Id = 2, Status = ModerationState.AwaitingModeration},
                new Observation {Id = 3, Status = ModerationState.Rejected}
                };
            uow = A.Fake<IUnitOfWork>();
            controller = new ModerationController(uow);
            A.CallTo(() => uow.ObservationsRepository.AllSatisfying(A<IQuerySpecification<Observation>>.Ignored))
                .Returns(Observations.Where(p => p.Status == ModerationState.AwaitingModeration));
            };

        Because of = () => { Result = controller.Index() as ViewResult; };

        It should_return_the_index_view = () => Result.ViewName.ShouldEqual(string.Empty);
        It should_populate_the_viewmodel_with_the_observations_awaiting_moderation =
            () => ((IEnumerable<Observation>) Result.Model).Single()
                .Status.ShouldEqual(ModerationState.AwaitingModeration);
        static ModerationController controller;
        static ViewResult Result;
        static IUnitOfWork uow;
        static List<Observation> Observations;
        }
    }