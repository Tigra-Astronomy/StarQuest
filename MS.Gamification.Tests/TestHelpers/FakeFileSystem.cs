using System.Collections.Generic;
using MS.Gamification.HtmlHelpers;

namespace MS.Gamification.Tests.TestHelpers
    {
    class FakeFileSystem : IFileSystemService
        {
        readonly List<string> fakeFiles = new List<string>();

        public FakeFileSystem(params string[] files)
            {
            fakeFiles = new List<string>(files);
            }

        public bool FileExists(string fullyQualifiedFileName)
            {
            return fakeFiles.Contains(fullyQualifiedFileName);
            }

        public override string ToString()
            {
            var result = $"Contains {fakeFiles.Count} files";
            return result;
            }
        }
    }