// This file is part of the MS.Gamification project
// 
// File: NinjectDependencyResolver.cs  Created: 2016-07-29@22:20
// Last modified: 2016-07-29@22:22

using System.Web.Http.Dependencies;
using MS.Gamification.App_Start;
using Ninject;

namespace MS.Gamification
    {
    // Provides a Ninject implementation of IDependencyScope
    // which resolves services using the Ninject container.

    // This class is the resolver, but it is also the global scope
    // so we derive from NinjectScope.
    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
        {
        private readonly IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel) : base(kernel)
            {
            this.kernel = kernel;
            }

        public IDependencyScope BeginScope()
            {
            return new NinjectDependencyScope(kernel.BeginBlock());
            }
        }
    }