using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MS.Gamification.DataAccess;

namespace MS.Gamification.Controllers
{
    public class MissionController : Controller
    {
    readonly IUnitOfWork uow;

    public MissionController(IUnitOfWork uow)
        {
        this.uow = uow;
        }

    // GET Mission/Level/1
    public ActionResult Level(int? level)
        {
        return View();
        }
    }
}