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
        readonly HttpServerUtilityBase webServer;
        readonly IImageStore imageStore;

        public ValidationImageController(HttpServerUtilityBase webServer, IImageStore imageStore )
            {
            this.webServer = webServer;
            this.imageStore = imageStore;
            }

        public ActionResult GetImage(string filename)
            {
            var safeFilename = filename ?? Challenge.NoImagePlaceholder;
            //ToDo - validate that filename is well-formed
            if (!imageStore.FileExists(safeFilename))
                safeFilename = Challenge.NoImagePlaceholder;
            var dir = webServer.MapPath("/App_Data/ValidationImages");
            var path = Path.Combine(dir, safeFilename);
            var ext = Path.GetExtension(safeFilename).TrimStart('.');
            return base.File(path, $"image/{ext}");
            }
        }
    }