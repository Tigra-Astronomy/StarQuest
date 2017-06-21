// This file is part of the MS.Gamification project
// 
// File: ObservingSessionsController.cs  Created: 2017-05-16@20:38
// Last modified: 2017-06-21@21:12

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
using MS.Gamification.BusinessLogic.EventManagement;
using MS.Gamification.BusinessLogic.Gamification.QuerySpecifications;
using MS.Gamification.Controllers;
using MS.Gamification.DataAccess;
using NLog;

// ReSharper disable HollowTypeName

namespace MS.Gamification.Areas.Admin.Controllers
    {
    [Authorize(Roles = "Moderator,Administrator,EventManager")]
    public class ObservingSessionsController : RequiresAuthorization
        {
        private readonly ILogger log;
        [NotNull] private readonly IMapper mapper;
        [NotNull] private readonly IObservingSessionManager sessionManager;
        [NotNull] private readonly IUnitOfWork uow;

        public ObservingSessionsController([NotNull] IUnitOfWork uow, [NotNull] IMapper mapper, [NotNull] ILogger logger,
            [NotNull] IObservingSessionManager sessionManager)
            {
            Contract.Requires(uow != null);
            Contract.Requires(mapper != null);
            Contract.Requires(logger != null);
            Contract.Requires(sessionManager != null);
            this.uow = uow;
            this.mapper = mapper;
            log = logger;
            this.sessionManager = sessionManager;
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
            var maybeSession = uow.ObservingSessions.GetMaybe(id);
            if (maybeSession.None)
                return HttpNotFound($"The requested observing session with Id={id} could not be found");
            var model = mapper.Map<ObservingSessionDetailsViewModel>(maybeSession.Single());
            return View(model);
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
                //var newEntity = mapper.Map<ObservingSession>(model);
                //uow.ObservingSessions.Add(newEntity);
                //await uow.CommitAsync().ConfigureAwait(false);
                await sessionManager.CreateAsync(model);
                log.Info($"Successfully created observing session: {model}");
                return RedirectToAction("Index");
                }
            catch (Exception e)
                {
                log.Error($"Error creating observing session {model}");
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
            Contract.Invariant(sessionManager != null);
            }

        // GET: ObservingSession/Edit/5
        public ActionResult Edit(int id) => View();

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

        // GET: ObservingSession/Cancel/5
        // Confirms cancellation of an observing session
        public ActionResult Cancel(int id)
            {
            var maybeSession = uow.ObservingSessions.GetMaybe(id);
            if (maybeSession.None)
                return HttpNotFound($"There is no observing session with ID={id}");
            var session = maybeSession.Single();
            var model = mapper.Map<EditObservingSessionViewModel>(session);
            return View(model);
            }

        // POST: ObservingSession/Cancel/5
        // Sets the status of a session to Cancelled and removes any pending reminders.
        // Optionally, notifies users of the cancellation.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel(CancelObservingSessionViewModel model)
            {
            try
                {
                await sessionManager.CancelAsync(model.Id, model.NotifyMembers, model.Message);
                return RedirectToAction("Index");
                }
            catch
                {
                return RedirectToAction("Cancel", new {id = model.Id});
                }
            }

        public async Task<ActionResult> Start(int id)
            {
            //ToDo: implement start session functionality in Sessionmanager, then call:
            //sessionManager.StartSession(id);
            var session = uow.ObservingSessions.Get(id);
            session.ScheduleState = ScheduleState.InProgress;
            await uow.CommitAsync();
            return RedirectToAction(nameof(Index));
            }

        public async Task<ActionResult> Finish(int id)
            {
            //ToDo: sessionManager.FinishSession(id);
            var session = uow.ObservingSessions.Get(id);
            session.ScheduleState = ScheduleState.Closed;
            await uow.CommitAsync();
            return RedirectToAction(nameof(Index));
            }
        }
    }