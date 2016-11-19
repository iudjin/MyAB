using System.Web.Mvc;
using Castle.Windsor;

namespace Web.Windsor
{
    /// <summary>
    /// Implements a dependency resolver for ASP.NET MVC 4 using Castle Windsor.
    /// </summary>
    public class WindsorMvcDependencyResolver : WindsorDependencyScope, IDependencyResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindsorMvcDependencyResolver"/> using the
        /// specified Windsor container.
        /// </summary>
        /// <param name="container">The container instance responsible for resolution and
        /// scoping.</param>
        public WindsorMvcDependencyResolver(IWindsorContainer container)
            : base(container)
        {
        }
    }
}