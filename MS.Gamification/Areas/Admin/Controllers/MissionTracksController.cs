// This file is part of the MS.Gamification project
// 
// File: MissionTracksController.cs  Created: 2016-08-05@22:52
// Last modified: 2016-08-09@22:32

using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic.QuerySpecifications;
using MS.Gamification.Models;

namespace WebApplication2.Areas.Admin.Controllers
    {
    public class MissionTracksController : Controller
        {
        private readonly IUnitOfWork uow;

        public MissionTracksController(IUnitOfWork uow)
            {
            this.uow = uow;
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
            PopulatePickLists();
            return View();
            }

        private void PopulatePickLists(int selectedBadge = 1, int selectedLevel = 1)
            {
            var badgeSelector = uow.Badges.PickList.ToSelectList(selectedBadge);
            var levelSelector = uow.MissionLevels.PickList.ToSelectList(selectedLevel);
            ViewBag.BadgeId = badgeSelector;
            ViewBag.MissionLevelId = levelSelector;
            }

        // POST: Admin/MissionTracks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "Id,Name,Number,AwardTitle,BadgeId,MissionLevelId")] MissionTrack missionTrack)
            {
            if (ModelState.IsValid)
                {
                //ToDo: delegate to game engine
                uow.MissionTracks.Add(missionTrack);
                await uow.CommitAsync();
                return RedirectToAction("Index");
                }
            PopulatePickLists(missionTrack.BadgeId, missionTrack.MissionLevelId);
            return View(missionTrack);
            }

        // GET: Admin/MissionTracks/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            var missionTrack = uow.MissionTracks.Get(id.Value);
            if (missionTrack == null)
                {
                return HttpNotFound();
                }
            return View(missionTrack);
            }

        // POST: Admin/MissionTracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
            {
            //ToDo: delegate the delete operation to the game engine
            return RedirectToAction("Index");
            }
        }
    }