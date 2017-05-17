// This file is part of the MS.Gamification project
// 
// File: BadgesController.cs  Created: 2016-08-20@23:12
// Last modified: 2016-11-01@19:23

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MS.Gamification.BusinessLogic.Gamification;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using Ninject;

namespace MS.Gamification.Areas.Admin.Controllers
    {
    public class BadgesController : RequiresAdministratorRights
        {
        private const string ImageIdentifierAllowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWZYZabcdefghijklmnopqrstuvwxyz0123456789-";
        private readonly IImageStore store;
        private readonly IUnitOfWork uow;

        public BadgesController([Named("BadgeImageStore")] IImageStore store, IUnitOfWork uow)
            {
            this.store = store;
            this.uow = uow;
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload()
            {
            var fileNames = (IEnumerable<string>) Request.Files.AllKeys;
            if (fileNames.Count() > 1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Multiple files not allowed");
            var index = fileNames.Single();
            var postedFile = Request.Files[index];
            var postedFileName = postedFile.FileName;
            var identifier = postedFileName.ToImageIdentifier();
            try
                {
                store.Save(postedFile.InputStream, identifier);
                var badge = new Badge
                    {
                    ImageIdentifier = identifier,
                    Name = identifier
                    };
                uow.Badges.Add(badge);
                uow.Commit();
                return Json(new {imageIdentifier = identifier, badgeId = badge.Id});
                }
            catch (Exception e)
                {
                return Json(new {error = e.Message});
                }
            }

        //    result = result.Keep(ImageIdentifierAllowedCharacters, '-');
        //    result = result.ToLower(CultureInfo.InvariantCulture);
        //    var result = Path.GetFileNameWithoutExtension(fileName);
        //    {

        //public string GenerateImageIdentifier(string fileName)
        //    return result;
        //    }
        }
    }