// This file is part of the MS.Gamification project
// 
// File: MissionTracksController.cs  Created: 2016-08-05@22:52
// Last modified: 2016-08-18@02:47

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MS.Gamification.Areas.Admin.ViewModels.MissionTracks;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic;
using MS.Gamification.GameLogic.QuerySpecifications;
using MS.Gamification.Models;

namespace MS.Gamification.Areas.Admin.Controllers
    {
    public class MissionTracksController : RequiresAdministratorRights
        {
        private readonly IGameEngineService gameEngine;
        private readonly IMapper mapper;
        private readonly IUnitOfWork uow;

        public MissionTracksController(IUnitOfWork uow, IGameEngineService gameEngine, IMapper mapper)
            {
            this.uow = uow;
            this.gameEngine = gameEngine;
            this.mapper = mapper;
            }

        // GET: Admin/MissionTracks
        public ActionResult Index()
            {
            var trackSpecification = new MissionTrackWithBadgeAndLevel();
            var missionTracks = uow.MissionTracks.AllSatisfying(trackSpecification);
            return View(missionTracks);
            }

        // GET: Admin/MissionTracks/Details/5
        public ActionResult Details(int? id)
            {
            if (id == null)
                {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            var missionTrack = uow.MissionTracks.Get(id.Value);
            if (missionTrack == null)
                {
                return HttpNotFound();
                }
            var model = mapper.Map<MissionTrack, MissionTrackViewModel>(missionTrack);
            return View(model);
            }

        // GET: Admin/MissionTracks/Create
        public ActionResult Create()
            {
            var model = new MissionTrackViewModel();
            PopulatePickLists(model);
            return View(model);
            }

        private void PopulatePickLists(MissionTrackViewModel model)
            {
            var badgeSelector = uow.Badges.PickList.ToSelectList();
            var levelSelector = uow.MissionLevels.PickList.ToSelectList();
            model.BadgePicker = badgeSelector;
            model.LevelPicker = levelSelector;
            }

        // POST: Admin/MissionTracks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "Id,Name,Number,AwardTitle,BadgeId,MissionLevelId")] MissionTrackViewModel missionTrack)
            {
            if (!ModelState.IsValid)
                {
                PopulatePickLists(missionTrack);
                return View(missionTrack);
                }
            try
                {
                var track = mapper.Map<MissionTrackViewModel, MissionTrack>(missionTrack);
                await gameEngine.CreateTrackAsync(track);
                return RedirectToAction("Index");
                }
            catch (InvalidOperationException e)
                {
                ModelState.AddModelError(string.Empty, e.Message);
                PopulatePickLists(missionTrack);
                return View(missionTrack);
                }
            }

        // GET: Admin/MissionTracks/Edit/5
        public ActionResult Edit(int? id)
            {
            if (id == null)
                {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            var maybeTrack = uow.MissionTracks.GetMaybe(id.Value);
            if (maybeTrack.None)
                {
                return HttpNotFound();
                }
            var missionTrack = maybeTrack.Single();
            var model = mapper.Map<MissionTrack, MissionTrackViewModel>(missionTrack);
            PopulatePickLists(model);
            return View(model);
            }

        // POST: Admin/MissionTracks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "Id,Name,Number,AwardTitle,BadgeId,MissionLevelId")] MissionTrackViewModel missionTrack)
            {
            if (!ModelState.IsValid)
                {
                PopulatePickLists(missionTrack);
                return View(missionTrack);
                }
            try
                {
                await gameEngine.UpdateTrackAsync(missionTrack);
                return RedirectToAction("Index");
                }
            catch (Exception e)
                {
                ModelState.AddModelError(string.Empty, e.Message);
                PopulatePickLists(missionTrack);
                return View(missionTrack);
                }
            }

        // GET: Admin/MissionTracks/Delete/5
        public ActionResult Delete(int? id)
            {
            if (id == null)
                {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            var maybeTrack = uow.MissionTracks.GetMaybe(id.Value);
            if (maybeTrack.None)
                {
                return HttpNotFound();
                }
            var missionTrack = maybeTrack.Single();
            var model = mapper.Map<MissionTrack, MissionTrackViewModel>(missionTrack);
            return View(model);
            }

        // POST: Admin/MissionTracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
            {
            await gameEngine.DeleteTrackAsync(id);
            return RedirectToAction("Index");
            }
        }
    }