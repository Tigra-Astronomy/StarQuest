﻿// This file is part of the MS.Gamification project
// 
// File: ValidationImageController.cs  Created: 2016-07-10@00:07
// Last modified: 2016-07-16@03:15

using System.Web.Mvc;
using MS.Gamification.HtmlHelpers;
using Ninject;

namespace MS.Gamification.Controllers
    {
    public class ValidationImageController : Controller
        {
        readonly IImageStore imageStore;

        public ValidationImageController([Named("ValidationImageStore")] IImageStore imageStore)
            {
            this.imageStore = imageStore;
            }

        [OutputCache]
        public ActionResult GetImage(string id)
            {
            if (id == null)
                id = string.Empty;
            var fullyQualifiedFileName = imageStore.FindImage(id);
            var contentType = imageStore.MimeType(id);
            return File(fullyQualifiedFileName, contentType);
            }
        }
    }
