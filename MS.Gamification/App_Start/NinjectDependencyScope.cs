// This file is part of the MS.Gamification project
// 
// File: NinjectDependencyScope.cs  Created: 2016-07-29@22:21
// Last modified: 2016-07-29@22:23

using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Syntax;

namespace MS.Gamification
    {
    public class NinjectDependencyScope : IDependencyScope
        {
        private IResolutionRoot resolver;

        public NinjectDependencyScope(IResolutionRoot resolver)
            {
            this.resolver = resolver;
            }

        public object GetService(Type serviceType)
            {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has been disposed");

            return resolver.TryGet(serviceType);
            }

        public IEnumerable<object> GetServices(Type serviceType)
            {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has been disposed");

            return resolver.GetAll(serviceType);
            }

        public void Dispose()
            {
            var disposable = resolver as IDisposable;
            if (disposable != null)
                disposable.Dispose();

            resolver = null;
            }
        }
    }