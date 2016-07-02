// This file is part of the MS.Gamification project
// 
// File: MissionController.cs  Created: 2016-07-01@00:05
// Last modified: 2016-07-02@01:04

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MS.Gamification.BusinessLogic.QuerySpecifications;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;

namespace MS.Gamification.Controllers
    {
    public class MissionController : Controller
        {
        private readonly IUnitOfWork uow;

        public MissionController(IUnitOfWork uow)
            {
            this.uow = uow;
            }

        // GET Mission/Level/1
        public ActionResult Level(int missionId, int? levelId)
            {
            var model = new MissionProgressViewModel();
            model.Level = levelId ?? 1;
            var query = new MissionLevelProgress(missionId, model.Level);
            var maybeMission = uow.Missions.GetMaybe(query);
            if (maybeMission.None)
                return HttpNotFound("The specified Mission ID was not found");
            var mission = maybeMission.Single();
            model.Tracks = new List<MissionTrack>(mission.Tracks);
            return View(model);
            }
        }
    }