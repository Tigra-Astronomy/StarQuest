using System.Web.Mvc;
using MS.Gamification.GameLogic;
using MS.Gamification.HtmlHelpers;
using Ninject;

namespace MS.Gamification.Controllers
    {
    public class StaticImageController : Controller
        {
        private readonly IImageStore imageStore;

        public StaticImageController([Named("StaticImageStore")] IImageStore imageStore)
            {
            this.imageStore = imageStore;
            }

        [OutputCache(Duration = 60*60*24)]
        public ActionResult GetImage(string id)
            {
            var fullyQualifiedFileName = imageStore.FindImage(id);
            var contentType = imageStore.MimeType(id);
            return File(fullyQualifiedFileName, contentType);
            }
        }
    }