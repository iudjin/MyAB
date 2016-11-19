using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.Windsor;
using MyAB.Assertions;

namespace Web.Windsor
{
    /// <summary>
    /// Implements a dependency resolver for ASP.NET Web API using Castle Windsor.
    /// </summary>
    /// <remarks>
    /// See the following post for more information:
    /// http://nikosbaxevanis.com/2012/06/04/using-the-web-api-dependency-resolver-with-castle-windsor-part-2/
    /// </remarks>
    public class WindsorHttpDependencyResolver : IDependencyResolver
    {
        private readonly IWindsorContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindsorHttpDependencyResolver"/> using the
        /// specified Windsor container.
        /// </summary>
        /// <param name="container">The container instance responsible for resolution and
        /// scoping.</param>
        public WindsorHttpDependencyResolver(IWindsorContainer container)
        {
            Ensure.ArgumentNotNull(container, "container");
            this.container = container;
        }

        /// <inheritdoc />
        public virtual object GetService(Type t)
        {
            return this.container.Kernel.HasComponent(t) ? this.container.Resolve(t) : null;
        }

        /// <inheritdoc />
        public virtual IEnumerable<object> GetServices(Type t)
        {
            return this.container.ResolveAll(t).Cast<object>().ToArray();
        }

        /// <inheritdoc />
        public virtual IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(container);
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}