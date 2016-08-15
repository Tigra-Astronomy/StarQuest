// This file is part of the MS.Gamification project
// 
// File: BadgesController.cs  Created: 2016-08-13@22:24
// Last modified: 2016-08-15@01:28

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic;
using MS.Gamification.HtmlHelpers;
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
        public async Task<ActionResult> Upload()
            {
            var fileNames = (IEnumerable<string>) Request.Files.AllKeys;
            if (fileNames.Count() > 1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Multiple files not allowed");
            var index = fileNames.Single();
            var postedFile = Request.Files[index];
            var postedFileName = postedFile.FileName;
            var identifier = GenerateImageIdentifier(postedFileName);
            try
                {
                store.Save(postedFile.InputStream, identifier);
                var badge = new Badge
                    {
                    ImageIdentifier = identifier,
                    Name = identifier
                    };
                uow.Badges.Add(badge);
                await uow.CommitAsync();
                return Json(new {imageIdentifier = identifier});
                }
            catch (Exception e)
                {
                return Json(new {error = e.Message});
                }
            }

        public string GenerateImageIdentifier(string fileName)
            {
            var result = Path.GetFileNameWithoutExtension(fileName);
            result = result.ToLower(CultureInfo.InvariantCulture);
            result = result.Keep(ImageIdentifierAllowedCharacters, '-');
            return result;
            }
        }
    }