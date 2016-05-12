using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MS.Gamification.HtmlHelpers
    {
    public interface IImageStore
        {
        /// <summary>
        ///     Checks whether a file exists on physical storage.
        /// </summary>
        /// <param name="fullyQualifiedFileName">The absolute fully qualified directory, filename and extension.</param>
        /// <returns><c>true</c> if the file exists; otherwise <c>false</c></returns>
        bool FileExists(string fullyQualifiedFileName);
        }

    internal class WebServerImageStore : IImageStore
        {
        public bool FileExists(string fullyQualifiedFileName)
            {
            return File.Exists(fullyQualifiedFileName);
            }
        }
    }