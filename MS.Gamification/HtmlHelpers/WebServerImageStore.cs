// This file is part of the MS.Gamification project
// 
// File: FileSystem.cs  Created: 2016-05-12@00:29
// Last modified: 2016-05-13@16:54

using System.IO;
using System.Web;
using NLog.Internal;

namespace MS.Gamification.HtmlHelpers
    {
    class WebServerImageStore : IImageStore
        {
        readonly HttpServerUtilityBase webhost;
        string rootPath;

        public WebServerImageStore(HttpServerUtilityBase webhost)
            {
            this.webhost = webhost;
            var config = new ConfigurationManager();
            rootPath = config.AppSettings["validationImagesRootPath"];
            }

        public bool FileExists(string filename)
            {
            return File.Exists(this.FullyQualifiedFileName(filename));
            }

        public string FullyQualifiedFileName(string filename)
            {
            var root = webhost.MapPath(rootPath);
            var path = Path.Combine(root, filename);
            return path;
            }

        public string MimeType(string filename)
            {
            var ext = Path.GetExtension(filename).TrimStart('.');
            return $"image/{ext}";
            }
        }
    }