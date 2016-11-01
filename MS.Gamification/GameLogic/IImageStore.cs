// This file is part of the MS.Gamification project
// 
// File: IImageStore.cs  Created: 2016-08-19@02:15
// Last modified: 2016-08-19@02:31

using System.Collections.Generic;
using System.IO;

namespace MS.Gamification.GameLogic
    {
    /// <summary>
    ///     Represents a persistent store of named images.
    /// </summary>
    public interface IImageStore
        {
        /// <summary>
        ///     Finds and returns a matching image from the store; i fnone found,
        ///     returns the placeholder image.
        /// </summary>
        /// <param name="identifier">
        ///     The image identifier. How this is translated into an image file name is implementation specific.
        /// </param>
        /// <returns>
        ///     The fully qualified file name on disk of a matching image,
        ///     or if no matches were found, the fully qualified filename on disk of
        ///     a placeholder image (which is assumed to always exist).
        /// </returns>
        string FindImage(string identifier);

        /// <summary>
        ///     Gets the MIME type of the specified image file
        /// </summary>
        /// <param name="identifier">The image identifier.</param>
        /// <returns>The MIME type of the identified image, for example, "image/png".</returns>
        string MimeType(string identifier);

        /// <summary>
        ///     Persists the image bitmap into the image store, using the <paramref name="identifier" /> as the key.
        /// </summary>
        /// <param name="imageStream">The image bitmap.</param>
        /// <param name="identifier">The image identifier that can later be used to retrieve the image.</param>
        void Save(Stream imageStream, string identifier);

        /// <summary>
        ///     Enumerates the image identifiers in the store.
        /// </summary>
        /// <returns>IEnumerable{string}.</returns>
        IEnumerable<string> EnumerateImages();
        }
    }