// This file is part of the MS.Gamification project
// 
// File: MissionsController.cs  Created: 2016-08-05@22:52
// Last modified: 2016-08-06@03:22

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MS.Gamification.BusinessLogic.Gamification;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using NLog;

namespace MS.Gamification.Areas.Admin.Controllers
    {
    /// <summary>
    ///     CRUD operations on Missions.
    /// </summary>
    /// <seealso cref="RequiresAdministratorRights" />
    public class MissionsController : RequiresAdministratorRights
        {
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();
        private readonly IGameEngineService gameEngine;
        private readonly IUnitOfWork uow;

        public MissionsController(IUnitOfWork uow, IGameEngineService gameEngine)
            {
            this.uow = uow;
            this.gameEngine = gameEngine;
            }

        // GET: Admin/Missions
        public ActionResult Index()
            {
            var missions = uow.Missions.GetAll();
            return View(missions);
            }

        // GET: Admin/Missions/Details/5
        public ActionResult Details(int? id)
            {
            if (id == null)
                {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            var mission = uow.Missions.GetMaybe(id.Value);
            if (mission.None)
                {
                return HttpNotFound();
                }
            return View(mission.Single());
            }

        // GET: Admin/Missions/Create
        public ActionResult Create()
            {
            return View();
            }

        // POST: Admin/Missions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Precondition")] Mission mission)
            {
            if (ModelState.IsValid)
                {
                uow.Missions.Add(mission);
                await uow.CommitAsync();
                return RedirectToAction("Index");
                }

            return View(mission);
            }

        // GET: Admin/Missions/Edit/5
        public ActionResult Edit(int? id)
            {
            if (id == null)
                {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            var mission = uow.Missions.GetMaybe(id.Value);
            if (mission.None)
                {
                return HttpNotFound();
                }
            return View(mission.Single());
            }

        // POST: Admin/Missions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Precondition")] Mission mission)
            {
            if (ModelState.IsValid)
                {
                var maybeMission = uow.Missions.GetMaybe(mission.Id);
                if (maybeMission.None)
                    {
                    ModelState.AddModelError("", "Mission not found in the database");
                    return View(mission);
                    }
                var dbMission = maybeMission.Single();
                dbMission.Precondition = mission.Precondition;
                dbMission.Title = mission.Title;
                await uow.CommitAsync();
                return RedirectToAction("Index");
                }
            return View(mission);
            }

        // GET: Admin/Missions/Delete/5
        public ActionResult Delete(int? id)
            {
            if (id == null)
                {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            var maybeMission = uow.Missions.GetMaybe(id.Value);
            if (maybeMission.None)
                {
                return HttpNotFound();
                }
            return View(maybeMission.Single());
            }

        // POST: Admin/Missions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
            {
            var maybeMission = uow.Missions.GetMaybe(id);
            if (maybeMission.None)
                {
                return HttpNotFound();
                }
            var mission = maybeMission.Single();
            try
                {
                await gameEngine.DeleteMissionAsync(id);
                return RedirectToAction("Index");
                }
            catch (Exception e)
                {
                Log.Error(e, $"Deleting mission id={id} Title={mission.Title}");
                ModelState.AddModelError(string.Empty, $"Delete failed: {e.Message}");
                return View("Delete", mission);
                }
            }
        }
    }