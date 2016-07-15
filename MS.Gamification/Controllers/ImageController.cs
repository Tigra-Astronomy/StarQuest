// This file is part of the MS.Gamification project
// 
// File: ImageController.cs  Created: 2016-05-11@23:01
// Last modified: 2016-05-11@23:14

using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using MS.Gamification.HtmlHelpers;
using MS.Gamification.Models;

namespace MS.Gamification.Controllers
    {
    public class ImageController : Controller
        {
        readonly IImageStore imageStore;

        public ImageController( IImageStore imageStore )
            {
            this.imageStore = imageStore;
            }

        public ActionResult ValidationImage(string filename)
            {
            var safeFilename = filename ?? Challenge.NoImagePlaceholder;
            //ToDo - validate that filename is well-formed
            if (!imageStore.FileExists(safeFilename))
                safeFilename = Challenge.NoImagePlaceholder;
            var fullyQualifiedFileName = imageStore.FullyQualifiedFileName(safeFilename);
            var contentType = imageStore.MimeType(safeFilename);
            return base.File(fullyQualifiedFileName, contentType);
            }

        public ActionResult Badge(string filename)
            {
            var safeFilename = filename ?? Challenge.NoImagePlaceholder;
            //ToDo: create a placeholder for badges



        }
    }
    }