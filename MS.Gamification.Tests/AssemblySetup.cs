// This file is part of the MS.Gamification project
// 
// File: AssemblySetup.cs  Created: 2016-07-09@22:31
// Last modified: 2016-07-09@22:37

using Machine.Specifications;

namespace MS.Gamification.Tests
    {
    public class AssemblySetup : IAssemblyContext
        {
        public void OnAssemblyStart()
            {
            MapperConfig.RegisterMaps();
            }

        public void OnAssemblyComplete() {}
        }
    }