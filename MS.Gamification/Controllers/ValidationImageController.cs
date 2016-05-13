// This file is part of the MS.Gamification project
// 
// File: ValidationImageController.cs  Created: 2016-05-11@23:01
// Last modified: 2016-05-11@23:14

using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using MS.Gamification.HtmlHelpers;
using MS.Gamification.Models;

namespace MS.Gamification.Controllers
    {
    public class ValidationImageController : Controller
        {
        readonly IImageStore imageStore;

        public ValidationImageController( IImageStore imageStore )
            {
            this.imageStore = imageStore;
            }

        public ActionResult GetImage(string filename)
            {
            var safeFilename = filename ?? Challenge.NoImagePlaceholder;
            //ToDo - validate that filename is well-formed
            if (!imageStore.FileExists(safeFilename))
                safeFilename = Challenge.NoImagePlaceholder;
            var fullyQualifiedFileName = imageStore.FullyQualifiedFileName(safeFilename);
            var contentType = imageStore.MimeType(safeFilename);
            return base.File(fullyQualifiedFileName, contentType);
            }
        }
    }