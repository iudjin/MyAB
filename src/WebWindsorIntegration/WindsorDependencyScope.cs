using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using log4net;
using MyAB.Assertions;

namespace Web.Windsor
{
    /// <summary>
    /// Implements a dependency scope using the Castle Windsor Call Context.
    /// </summary>
    /// <remarks>
    /// See the following post for more information:
    /// http://nikosbaxevanis.com/2012/07/16/using-the-web-api-dependency-resolver-with-castle-windsor-scoped-lifetime/
    /// </remarks>
    public class WindsorDependencyScope : IDependencyScope
    {
        private readonly ILog log = LogManager.GetLogger(typeof(WindsorDependencyScope));
        private readonly IWindsorContainer container;
        private readonly IDisposable scope;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindsorDependencyScope"/> using the
        /// specified Windsor container.
        /// </summary>
        /// <param name="container">The container instance responsible for resolution and
        /// scoping.</param>
        public WindsorDependencyScope(IWindsorContainer container)
        {
            Ensure.ArgumentNotNull(container, "container");
            log.Debug("Dependency scope initializing");
            this.container = container;
            this.scope = container.BeginScope();
            log.Debug("Dependency scope initialized");
        }

        /// <inheritdoc />
        public object GetService(Type t)
        {
            log.DebugFormat("Service requested, type = {0}", t.FullName);
            return container.Kernel.HasComponent(t) ? this.container.Resolve(t) : null;
        }

        /// <inheritdoc />
        public IEnumerable<object> GetServices(Type t)
        {
            log.DebugFormat("Multiple service requested, type = {0}", t.FullName);
            return container.ResolveAll(t).Cast<object>().ToArray();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            log.DebugFormat("Dependency scope disposed");
            scope.Dispose();
        }
    }
}