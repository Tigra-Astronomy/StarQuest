// This file is part of the MS.Gamification project
// 
// File: UnitTestImageStore.cs  Created: 2016-07-10@00:07
// Last modified: 2016-07-16@01:46

using System.Collections.Generic;
using System.IO;
using MS.Gamification.HtmlHelpers;

namespace MS.Gamification.Tests.TestHelpers
    {
    internal class UnitTestImageStore : Dictionary<string, string>, IImageStore
        {
        readonly string rootPath;

        public UnitTestImageStore(string rootPath)
            {
            this.rootPath = rootPath;
            this["NoImage"] = "NoImage.png";
            }

        public new string this[string key] { get { return base[key]; } set { base[key] = Path.Combine(rootPath, value); } }

        public string FindImage(string identifier)
            {
            if (ContainsKey(identifier))
                return this[identifier];
            return this["NoImage"];
            }

        public string MimeType(string identifier)
            {
            var image = FindImage(identifier);
            return $"image/{Path.GetExtension(image).TrimStart('.')}";
            }
        }
    }
