//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using WebApplication2;

//namespace WebApplication2.Areas.Admin.Controllers
//{
//    public class MissionTracksController : Controller
//    {
//        private DummyDbContext db = new DummyDbContext();

//        // GET: Admin/MissionTracks
//        public async Task<ActionResult> Index()
//        {
//            var missionTracks = db.MissionTracks.Include(m => m.Badge).Include(m => m.MissionLevel);
//            return View(await missionTracks.ToListAsync());
//        }

//        // GET: Admin/MissionTracks/Details/5
//        public async Task<ActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            MissionTrack missionTrack = await db.MissionTracks.FindAsync(id);
//            if (missionTrack == null)
//            {
//                return HttpNotFound();
//            }
//            return View(missionTrack);
//        }

//        // GET: Admin/MissionTracks/Create
//        public ActionResult Create()
//        {
//            ViewBag.BadgeId = new SelectList(db.Badges, "Id", "ImageIdentifier");
//            ViewBag.MissionLevelId = new SelectList(db.MissionLevels, "Id", "Name");
//            return View();
//        }

//        // POST: Admin/MissionTracks/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Number,AwardTitle,BadgeId,MissionLevelId")] MissionTrack missionTrack)
//        {
//            if (ModelState.IsValid)
//            {
//                db.MissionTracks.Add(missionTrack);
//                await db.SaveChangesAsync();
//                return RedirectToAction("Index");
//            }

//            ViewBag.BadgeId = new SelectList(db.Badges, "Id", "ImageIdentifier", missionTrack.BadgeId);
//            ViewBag.MissionLevelId = new SelectList(db.MissionLevels, "Id", "Name", missionTrack.MissionLevelId);
//            return View(missionTrack);
//        }

//        // GET: Admin/MissionTracks/Edit/5
//        public async Task<ActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            MissionTrack missionTrack = await db.MissionTracks.FindAsync(id);
//            if (missionTrack == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.BadgeId = new SelectList(db.Badges, "Id", "ImageIdentifier", missionTrack.BadgeId);
//            ViewBag.MissionLevelId = new SelectList(db.MissionLevels, "Id", "Name", missionTrack.MissionLevelId);
//            return View(missionTrack);
//        }

//        // POST: Admin/MissionTracks/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Number,AwardTitle,BadgeId,MissionLevelId")] MissionTrack missionTrack)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(missionTrack).State = EntityState.Modified;
//                await db.SaveChangesAsync();
//                return RedirectToAction("Index");
//            }
//            ViewBag.BadgeId = new SelectList(db.Badges, "Id", "ImageIdentifier", missionTrack.BadgeId);
//            ViewBag.MissionLevelId = new SelectList(db.MissionLevels, "Id", "Name", missionTrack.MissionLevelId);
//            return View(missionTrack);
//        }

//        // GET: Admin/MissionTracks/Delete/5
//        public async Task<ActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            MissionTrack missionTrack = await db.MissionTracks.FindAsync(id);
//            if (missionTrack == null)
//            {
//                return HttpNotFound();
//            }
//            return View(missionTrack);
//        }

//        // POST: Admin/MissionTracks/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> DeleteConfirmed(int id)
//        {
//            MissionTrack missionTrack = await db.MissionTracks.FindAsync(id);
//            db.MissionTracks.Remove(missionTrack);
//            await db.SaveChangesAsync();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
