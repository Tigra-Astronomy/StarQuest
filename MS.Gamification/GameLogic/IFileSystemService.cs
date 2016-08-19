namespace MS.Gamification.GameLogic
    {
    /// <summary>
    ///   Provides file system services
    /// </summary>
    public interface IFileSystemService
        {
        /// <summary>
        ///   Checks whether a file exists on disk.
        /// </summary>
        /// <param name="fullyQualifiedFileName">The fully qualified name of the file.</param>
        /// <returns><c>true</c> if the file exists, <c>false</c> otherwise.</returns>
        bool FileExists(string fullyQualifiedFileName);
        }
    }