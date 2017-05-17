// This file is part of the MS.Gamification project
// 
// File: ObservingSessionsController.cs  Created: 2017-05-16@20:38
// Last modified: 2017-05-17@19:55

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using JetBrains.Annotations;
using MS.Gamification.Areas.Admin.ViewModels.ObservingSessions;
using MS.Gamification.BusinessLogic.Gamification.QuerySpecifications;
using MS.Gamification.Controllers;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using NLog;

// ReSharper disable HollowTypeName

namespace MS.Gamification.Areas.Admin.Controllers
    {
    [Authorize(Roles = "Moderator,Administrator,EventManager")]
    public class ObservingSessionsController : RequiresAuthorization
        {
        private readonly ILogger log;
        [NotNull] private readonly IMapper mapper;
        [NotNull] private readonly IUnitOfWork uow;

        public ObservingSessionsController([NotNull] IUnitOfWork uow, [NotNull] IMapper mapper, [NotNull] ILogger logger)
            {
            Contract.Requires(uow != null);
            Contract.Requires(mapper != null);
            Contract.Requires(logger != null);
            this.uow = uow;
            this.mapper = mapper;
            log = logger;
            }

        // GET: ObservingSession
        public ActionResult Index()
            {
            var query = new ObservingSessionsIndex();
            var model = uow.ObservingSessions.AllSatisfying(query);
            return View(model.ToList());
            }

        // GET: ObservingSession/Details/5
        public ActionResult Details(int id)
            {
            return View();
            }

        // GET: ObservingSession/Create
        public ActionResult Create()
            {
            var model = new CreateObservingSessionViewModel();
            return View(model);
            }

        // POST: ObservingSession/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([NotNull] CreateObservingSessionViewModel model)
            {
            Contract.Requires(model != null);
            if (!ModelState.IsValid)
                return View(model);
            try
                {
                var newEntity = mapper.Map<ObservingSession>(model);
                uow.ObservingSessions.Add(newEntity);
                await uow.CommitAsync().ConfigureAwait(false);
                log.Info(
                    $"Successfully created observing session ID={newEntity.Id}: {newEntity.StartsAt} {newEntity.Title} at {newEntity.Venue}");
                return RedirectToAction("Index");
                }
            catch (Exception e)
                {
                log.Error($"Error creating observing session '{model.Title}': {e.Message}");
                }
            return View(model);
            }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822: MarkMembersAsStatic", Justification = "Required for code contracts.")]
        [Conditional("CONTRACTS_FULL")]
        private void ObjectInvariant()
            {
            Contract.Invariant(mapper != null);
            Contract.Invariant(uow != null);
            }

        // GET: ObservingSession/Edit/5
        public ActionResult Edit(int id)
            {
            return View();
            }

// POST: ObservingSession/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
            {
            try
                {
// TODO: Add update logic here
                return RedirectToAction("Index");
                }
            catch
                {
                return View();
                }
            }

// GET: ObservingSession/Delete/5
        public ActionResult Delete(int id)
            {
            return View();
            }

// POST: ObservingSession/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
            {
            try
                {
// TODO: Add delete logic here
                return RedirectToAction("Index");
                }
            catch
                {
                return View();
                }
            }
        }
    }