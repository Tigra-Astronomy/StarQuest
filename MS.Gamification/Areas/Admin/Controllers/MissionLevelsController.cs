// This file is part of the MS.Gamification project
// 
// File: MissionLevelsController.cs  Created: 2016-11-01@19:37
// Last modified: 2017-05-16@21:08

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MS.Gamification.Areas.Admin.ViewModels.MissionLevels;
using MS.Gamification.BusinessLogic.Gamification;
using MS.Gamification.DataAccess;
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
        public ActionResult Details(int? id)
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
            var model = new MissionLevelViewModel();
            PopulateMissionPicker(model);
            return View(model);
            }

        private void PopulateMissionPicker(MissionLevelViewModel model)
            {
            model.MissionPicker = uow.Missions.PickList.ToSelectList();
            }

        // POST: Admin/MissionLevels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "Id,Name,Level,AwardTitle,Precondition,MissionId")] MissionLevelViewModel model)
            {
            if (!ModelState.IsValid)
                {
                PopulateMissionPicker(model);
                return View(model);
                }
            try
                {
                await gameEngine.CreateLevelAsync(model);
                return RedirectToAction("Index");
                }
            catch (InvalidOperationException e)
                {
                ModelState.AddModelError(string.Empty, e.Message);
                PopulateMissionPicker(model);
                return View(model);
                }
            }

        // GET: Admin/MissionLevels/Edit/5
        public ActionResult Edit(int? id)
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
            var model = mapper.Map<MissionLevel, MissionLevelViewModel>(maybeLevel.Single());
            PopulateMissionPicker(model);
            return View(model);
            }

        // POST: Admin/MissionLevels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "Id,Name,Level,AwardTitle,Precondition,MissionId")] MissionLevelViewModel model)
            {
            if (!ModelState.IsValid)
                {
                PopulateMissionPicker(model);
                return View(model);
                }
            try
                {
                await gameEngine.UpdateLevelAsync(model);
                return RedirectToAction("Index");
                }
            catch (InvalidOperationException e)
                {
                ModelState.AddModelError(string.Empty, e.Message);
                PopulateMissionPicker(model);
                return View(model);
                }
            }

        // GET: Admin/MissionLevels/Delete/5
        public ActionResult Delete(int? id)
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
        [HttpPost]
        [ActionName("Delete")]
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