// This file is part of the MS.Gamification project
// 
// File: BadgeImageController.cs  Created: 2016-07-16@04:48
// Last modified: 2016-07-19@03:07

using System.Web.Mvc;
using MS.Gamification.HtmlHelpers;
using Ninject;

namespace MS.Gamification.Controllers
    {
    public class BadgeImageController : Controller
        {
        private readonly IImageStore imageStore;

        public BadgeImageController([Named("BadgeImageStore")] IImageStore imageStore)
            {
            this.imageStore = imageStore;
            }

        [OutputCache(Duration = 60 * 60 * 24)]
        public ActionResult GetImage(string id)
            {
            var fullyQualifiedFileName = imageStore.FindImage(id);
            var contentType = imageStore.MimeType(id);
            return File(fullyQualifiedFileName, contentType);
            }
        }
    }