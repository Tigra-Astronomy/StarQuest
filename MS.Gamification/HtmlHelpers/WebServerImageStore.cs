// This file is part of the MS.Gamification project
// 
// File: WebServerImageStore.cs  Created: 2016-07-10@00:07
// Last modified: 2016-07-16@02:13

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using MS.Gamification.DataAccess;

namespace MS.Gamification.HtmlHelpers
    {
    /// <summary>
    ///   Maps image names to static image files that are store on a web server,
    ///   where the path to the images comes from a setting in the Web.config
    ///   file.
    /// </summary>
    /// <seealso cref="MS.Gamification.HtmlHelpers.IImageStore" />
    class WebServerImageStore : IImageStore
        {
        readonly List<string> acceptedExtensions = new List<string> {"png", "gif", "jpg", "bmp"};
        readonly IFileSystemService fileService;
        readonly string rootPath;
        readonly HttpServerUtilityBase webhost;

        public WebServerImageStore(HttpServerUtilityBase webhost,
            IFileSystemService fileService,
            string rootUrl = "/images")
            {
            this.webhost = webhost;
            this.fileService = fileService;
            rootPath = rootUrl;
            }

        // var imageFile = store.FindImage("image-name");
        public string FindImage(string identifier)
            {
            var candidate = FullyQualifiedFileName(identifier);
            if (fileService.FileExists(candidate))
                return candidate;
            var maybeFound = FindMatchingImage(identifier);
            if (maybeFound.None)
                return FullyQualifiedFileName("NoImage.png");
            return FullyQualifiedFileName(maybeFound.Single());
            }

        public string MimeType(string identifier)
            {
            var maybeImage = FindMatchingImage(identifier);
            if (maybeImage.None)
                return "image/png";
            var ext = Path.GetExtension(maybeImage.Single())?.TrimStart('.');
            return $"image/{ext}";
            }

        private string FullyQualifiedFileName(string filename)
            {
            var root = webhost.MapPath(rootPath);
            var path = Path.Combine(root, filename);
            return path;
            }

        private Maybe<string> FindMatchingImage(string imageName)
            {
            foreach (var extension in acceptedExtensions)
                {
                var candidate = $"{imageName}.{extension}";
                var fullFileName = FullyQualifiedFileName(candidate);
                if (fileService.FileExists(fullFileName))
                    return new Maybe<string>(candidate);
                }
            return Maybe<string>.Empty;
            }
        }
    }
