// This file is part of the MS.Gamification project
// 
// File: WebServerImageStore.cs  Created: 2016-08-19@02:14
// Last modified: 2016-08-19@02:31

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using MS.Gamification.DataAccess;

namespace MS.Gamification.BusinessLogic.Gamification
    {
    /// <summary>
    ///     Maps image names to static image files that are store on a web server, where the path to the images comes
    ///     from a setting in the Web.config file.
    /// </summary>
    /// <seealso cref="IImageStore" />
    internal class WebServerImageStore : IImageStore
        {
        private readonly List<string> acceptedExtensions = new List<string> {"png", "gif", "jpg", "bmp"};
        private readonly IFileSystemService fileService;
        private readonly string rootPath;
        private readonly HttpServerUtilityBase webhost;

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

        public void Save(Stream imageStream, string identifier)
            {
            var bitmap = Image.FromStream(imageStream);
            var filename = FullyQualifiedFileName(identifier + ".png");
            //ToDo: Use the IFileSystemService to save the file, and unit test it.
            bitmap.Save(filename, ImageFormat.Png);
            }

        /// <summary>
        ///     Enumerates the image identifiers in the store.
        /// </summary>
        /// <returns>IEnumerable{string}.</returns>
        public IEnumerable<string> EnumerateImages()
            {
            var root = webhost.MapPath(rootPath);
            var files = Directory.EnumerateFiles(root, "*.*", SearchOption.TopDirectoryOnly);
            var identifiers = files.Select(p => p.ToImageIdentifier());
            return identifiers;
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