// This file is part of the MS.Gamification project
// 
// File: CumulativeScoreController.cs  Created: 2016-07-29@21:38
// Last modified: 2016-07-29@22:41

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic.QuerySpecifications;

namespace MS.Gamification.Controllers.Api
    {
    public class CumulativeScoreController : ApiController
        {
        private readonly IUnitOfWork uow;

        public CumulativeScoreController(IUnitOfWork uow)
            {
            this.uow = uow;
            }

        // GET api/CumulativeScore/id
        public IHttpActionResult Get(string id)
            {
            var spec = new SingleUserWithObservations(id);
            var maybeUser = uow.Users.GetMaybe(spec);
            if (maybeUser.None)
                return BadRequest("no such user");
            var user = maybeUser.Single();
            var pointsJournal = user.Observations.Select(p => p.Challenge.Points).ToList();
            return  Ok(pointsJournal);
            }
        }
    }