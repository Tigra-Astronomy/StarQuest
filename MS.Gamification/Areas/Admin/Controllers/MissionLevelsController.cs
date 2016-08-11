// This file is part of the MS.Gamification project
// 
// File: MissionLevelsController.cs  Created: 2016-08-05@22:52
// Last modified: 2016-08-11@00:56

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic;
using MS.Gamification.Models;

namespace MS.Gamification.Areas.Admin.Controllers
    {
    public class MissionLevelsController : RequiresAdministratorRights
        {
        private readonly IGameEngineService gameEngine;
        private readonly IMapper mapper;
        private readonly IUnitOfWork uow;

        public MissionLevelsController(IUnitOfWork uow, IMapper mapper, IGameEngineService gameEngine)
            {
            this.uow = uow;
            this.mapper = mapper;
            this.gameEngine = gameEngine;
            }

        // GET: Admin/MissionLevels
        public ActionResult Index()
            {
            var missionLevels = uow.MissionLevels.GetAll().ToList();
            return View(missionLevels);
            }

        // GET: Admin/MissionLevels/Details/5
        public async Task<ActionResult> Details(int? id)
            {
            if (id == null)
                {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            var missionLevel = uow.MissionLevels.GetMaybe(id.Value);
            if (missionLevel.None)
                {
                return HttpNotFound();
                }
            return View(missionLevel.Single());
            }

        // GET: Admin/MissionLevels/Create
        public ActionResult Create()
            {
            PopulateMissionPicker();
            return View();
            }

        private void PopulateMissionPicker(int? selectedItem = null)
            {
            if (selectedItem.HasValue)
                {
                ViewBag.MissionId = new SelectList(uow.Missions.GetAll(), "Id", "Title", selectedItem.Value);
                }
            else
                {
                uow.Missions.PickList.ToSelectList();
                ViewBag.MissionId = new SelectList(uow.Missions.GetAll(), "Id", "Title");
                }
            }

        // POST: Admin/MissionLevels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "Id,Name,Level,AwardTitle,Precondition,MissionId")] MissionLevel missionLevel)
            {
            if (!ModelState.IsValid)
                {
                PopulateMissionPicker();
                return View(missionLevel);
                }
            try
                {
                await gameEngine.CreateLevelAsync(missionLevel);
                return RedirectToAction("Index");
                }
            catch (InvalidOperationException e)
                {
                ModelState.AddModelError(string.Empty, e.Message);
                PopulateMissionPicker();
                return View(missionLevel);
                }
            }

        // GET: Admin/MissionLevels/Edit/5
        public async Task<ActionResult> Edit(int? id)
            {
            if (id == null)
                {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            var maybeLevel = uow.MissionLevels.GetMaybe(id.Value);
            if (maybeLevel.None)
                {
                return HttpNotFound();
                }
            var missionLevel = maybeLevel.Single();
            PopulateMissionPicker(missionLevel.MissionId);
            return View(missionLevel);
            }

        // POST: Admin/MissionLevels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "Id,Name,Level,AwardTitle,Precondition,MissionId")] MissionLevel missionLevel)
            {
            if (!ModelState.IsValid)
                {
                PopulateMissionPicker(missionLevel.MissionId);
                return View(missionLevel);
                }
            try
                {
                await gameEngine.UpdateLevelAsync(missionLevel);
                return RedirectToAction("Index");
                }
            catch (InvalidOperationException e)
                {
                ModelState.AddModelError(string.Empty, e.Message);
                PopulateMissionPicker();
                return View(missionLevel);
                }
            }

        // GET: Admin/MissionLevels/Delete/5
        public async Task<ActionResult> Delete(int? id)
            {
            if (id == null)
                {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            var maybeLevel = uow.MissionLevels.GetMaybe(id.Value);
            if (maybeLevel.None)
                {
                return HttpNotFound();
                }
            return View(maybeLevel.Single());
            }

        // POST: Admin/MissionLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
            {
            var missionLevel = uow.MissionLevels.Get(id);
            try
                {
                await gameEngine.DeleteLevelAsync(missionLevel.Id);
                }
            catch (InvalidOperationException e)
                {
                ModelState.AddModelError(string.Empty, e.Message);
                return View("Delete", missionLevel);
                }

            return RedirectToAction("Index");
            }
        }
    }