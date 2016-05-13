namespace MS.Gamification.HtmlHelpers
    {
    public interface IImageStore
        {
        /// <summary>
        ///     Checks whether a file exists on physical storage.
        /// </summary>
        /// <param name="filename">The absolute fully qualified directory, filename and extension.</param>
        /// <returns><c>true</c> if the file exists; otherwise <c>false</c></returns>
        bool FileExists(string filename);

        /// <summary>
        ///     Gets the fully qualified file name for the specified image.
        ///     The format of the fully qualified name is storage implementation specific.
        /// </summary>
        /// <param name="filename"></param>
        string FullyQualifiedFileName(string filename);

        /// <summary>
        ///     Gets the MIME type of the specified image file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>A MIME type.</returns>
        string MimeType(string filename);
        }
    }