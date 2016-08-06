//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using System.Web.Mvc;
//using MS.Gamification.DataAccess;
//using MS.Gamification.Models;

//namespace MS.Gamification.Areas.Admin.Controllers
//{
//    public class MissionLevelsController : RequiresAdministratorRights
//    {
//        private readonly IUnitOfWork uow;

//        public MissionLevelsController(IUnitOfWork uow)
//            {
//            this.uow = uow;
//            }

//        // GET: Admin/MissionLevels
//        public ActionResult Index()
//            {
//            var missionLevels = uow.MissionLevels.GetAll().ToList();
//            return View(missionLevels);
//            }

//        // GET: Admin/MissionLevels/Details/5
//        public async Task<ActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            var missionLevel = uow.MissionLevels.GetMaybe(id.Value);
//            if (missionLevel.None)
//            {
//                return HttpNotFound();
//            }
//            return View(missionLevel);
//        }

//        // GET: Admin/MissionLevels/Create
//        public ActionResult Create()
//        {
//            ViewBag.MissionId = new SelectList(db.Missions, "Id", "Title");
//            return View();
//        }

//        // POST: Admin/MissionLevels/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Level,AwardTitle,Precondition,MissionId")] MissionLevel missionLevel)
//        {
//            if (ModelState.IsValid)
//            {
//                db.MissionLevels.Add(missionLevel);
//                await db.SaveChangesAsync();
//                return RedirectToAction("Index");
//            }

//            ViewBag.MissionId = new SelectList(db.Missions, "Id", "Title", missionLevel.MissionId);
//            return View(missionLevel);
//        }

//        // GET: Admin/MissionLevels/Edit/5
//        public async Task<ActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            MissionLevel missionLevel = await db.MissionLevels.FindAsync(id);
//            if (missionLevel == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.MissionId = new SelectList(db.Missions, "Id", "Title", missionLevel.MissionId);
//            return View(missionLevel);
//        }

//        // POST: Admin/MissionLevels/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Level,AwardTitle,Precondition,MissionId")] MissionLevel missionLevel)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(missionLevel).State = EntityState.Modified;
//                await db.SaveChangesAsync();
//                return RedirectToAction("Index");
//            }
//            ViewBag.MissionId = new SelectList(db.Missions, "Id", "Title", missionLevel.MissionId);
//            return View(missionLevel);
//        }

//        // GET: Admin/MissionLevels/Delete/5
//        public async Task<ActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            MissionLevel missionLevel = await db.MissionLevels.FindAsync(id);
//            if (missionLevel == null)
//            {
//                return HttpNotFound();
//            }
//            return View(missionLevel);
//        }

//        // POST: Admin/MissionLevels/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> DeleteConfirmed(int id)
//        {
//            MissionLevel missionLevel = await db.MissionLevels.FindAsync(id);
//            db.MissionLevels.Remove(missionLevel);
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
