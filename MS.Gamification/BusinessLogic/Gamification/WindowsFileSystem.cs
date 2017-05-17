// This file is part of the MS.Gamification project
// 
// File: WindowsFileSystem.cs  Created: 2016-07-16@03:02
// Last modified: 2016-07-16@03:04

using System.IO;

namespace MS.Gamification.BusinessLogic.Gamification
    {
    internal class WindowsFileSystem : IFileSystemService
        {
        public bool FileExists(string fullyQualifiedFileName)
            {
            return File.Exists(fullyQualifiedFileName);
            }
        }
    }
