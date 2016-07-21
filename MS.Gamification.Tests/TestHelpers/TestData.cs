// This file is part of the MS.Gamification project
// 
// File: TestData.cs  Created: 2016-07-20@21:56
// Last modified: 2016-07-20@23:33

using System.IO;
using System.Reflection;
using JetBrains.Annotations;

namespace MS.Gamification.Tests.TestHelpers
    {
    static class TestData
        {
        [ContractAnnotation("")]
        internal static string FromEmbeddedResource(string resourceName)
            {
            var asm = Assembly.GetExecutingAssembly();
            var asmName = asm.GetName().Name;
            var resourceRoot = $"{asmName}.TestData";
            var resource = $"{resourceRoot}.{resourceName}";
            using (var stream = asm.GetManifestResourceStream(resource))
                {
                var reader = new StreamReader(stream);
                return reader.ReadToEnd();
                }
            }
        }
    }