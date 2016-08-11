// This file is part of the MS.Gamification project
// 
// File: MissionTracksController.cs  Created: 2016-08-05@22:52
// Last modified: 2016-08-11@00:14

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
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
        private readonly IUnitOfWork uow;

        public MissionTracksController(IUnitOfWork uow, IGameEngineService gameEngine)
            {
            this.uow = uow;
            this.gameEngine = gameEngine;
            }

        // GET: Admin/MissionTracks
        public async Task<ActionResult> Index()
            {
            var trackSpecification = new MissionTrackWithBadgeAndLevel();
            var missionTracks = uow.MissionTracks.AllSatisfying(trackSpecification);
            return View(missionTracks);
            }

        // GET: Admin/MissionTracks/Details/5
        public async Task<ActionResult> Details(int? id)
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
            return View(missionTrack);
            }

        // GET: Admin/MissionTracks/Create
        public ActionResult Create()
            {
            //PopulatePickLists();
            var model = new MissionTrackViewModel();
            model.BadgePicker = uow.Badges.PickList.Select(p => new SelectListItem {Value = p.Id.ToString(), Text = p.DisplayName});
            model.LevelPicker =
                uow.MissionLevels.PickList.Select(p => new SelectListItem {Value = p.Id.ToString(), Text = p.DisplayName});
            return View(model);
            }

        private void PopulatePickLists(int selectedBadge = 1, int selectedLevel = 1)
            {
            var badgeSelector = uow.Badges.PickList.ToSelectList(selectedBadge);
            var levelSelector = uow.MissionLevels.PickList.ToSelectList(selectedLevel);
            ViewBag.BadgePicker = badgeSelector;
            ViewBag.MissionLevelPicker = levelSelector;
            }

        // POST: Admin/MissionTracks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "Id,Name,Number,AwardTitle,BadgeId,MissionLevelId")] MissionTrack missionTrack)
            {
            if (!ModelState.IsValid)
                {
                PopulatePickLists(missionTrack.BadgeId, missionTrack.MissionLevelId);
                return View(missionTrack);
                }
            try
                {
                await gameEngine.CreateTrackAsync(missionTrack);
                return RedirectToAction("Index");
                }
            catch (InvalidOperationException e)
                {
                ModelState.AddModelError(string.Empty, e.Message);
                PopulatePickLists(missionTrack.BadgeId, missionTrack.MissionLevelId);
                return View(missionTrack);
                }
            }

        // GET: Admin/MissionTracks/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            PopulatePickLists(missionTrack.BadgeId, missionTrack.MissionLevelId);
            return View(missionTrack);
            }

        // POST: Admin/MissionTracks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "Id,Name,Number,AwardTitle,BadgeId,MissionLevelId")] MissionTrack missionTrack)
            {
            if (ModelState.IsValid)
                {
                //ToDo: delegate the update operation to the game engine
                return RedirectToAction("Index");
                }
            PopulatePickLists(missionTrack.BadgeId, missionTrack.MissionLevelId);
            return View(missionTrack);
            }

        // GET: Admin/MissionTracks/Delete/5
        public async Task<ActionResult> Delete(int? id)
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
            return View(missionTrack);
            }

        // POST: Admin/MissionTracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
            {
            gameEngine.DeleteTrackAsync(id);
            return RedirectToAction("Index");
            }
        }
    }