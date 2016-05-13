using System.Collections.Generic;
using System.IO;
using System.Linq;
using MS.Gamification.HtmlHelpers;

namespace MS.Gamification.Tests.Controllers
    {
    internal class UnitTestImageStore : IImageStore {
        readonly string rootPath;
        readonly IEnumerable<string> validFiles;

        public UnitTestImageStore(string rootPath, IEnumerable<string> validFiles)
            {
            this.rootPath = rootPath;
            this.validFiles = validFiles;
            }

        public bool FileExists(string filename)
            {
            return validFiles.Contains(filename);
            }

        public string FullyQualifiedFileName(string filename)
            {
            return Path.Combine(rootPath, filename);
            }

        public string MimeType(string filename)
            {
            return $"image/{Path.GetExtension(filename).TrimStart('.')}";
            }
    }
    }