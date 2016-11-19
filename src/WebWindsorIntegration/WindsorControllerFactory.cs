using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using log4net;
using MyAB.Assertions;

namespace Web.Windsor
{
    /// <summary>
    /// Implements an MVC controller factory based on Windsor, supporting the scoped lifestyle per
    /// controller.
    /// </summary>
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel kernel;
        private readonly ILog log = LogManager.GetLogger(typeof(WindsorControllerFactory));

        /// <summary>
        /// Initializes a new instance of the <see cref="WindsorControllerFactory"/> class using
        /// the specified kernel.
        /// </summary>
        /// <param name="kernel">The Windsor kernel instance responsible for resolving
        /// dependencies.</param>
        public WindsorControllerFactory(IKernel kernel)
        {
            Ensure.ArgumentNotNull(kernel, "kernel");
            this.kernel = kernel;
        }

        /// <inheritdoc />
        protected override IController GetControllerInstance(RequestContext requestContext,
            Type controllerType)
        {
            if (controllerType == null)
                throw new HttpException(404,
                    string.Format("The controller for path '{0}' could not be found.", 
                        requestContext.HttpContext.Request.Path));
            log.DebugFormat("Attempting to resolve controller, type = {0}",
                controllerType.FullName);
            if (!kernel.HasComponent(controllerType))
                throw new InvalidOperationException(string.Format("Type '{0}' was not " +
                    "registered in the container.", controllerType.FullName));
            var scope = kernel.BeginScope();
            var actualController = (IController)kernel.Resolve(controllerType);
            return new ScopedController(actualController, scope);
        }

        /// <inheritdoc />
        public override void ReleaseController(IController controller)
        {
            log.DebugFormat("Releasing controller, type = {0}",
                controller.GetType().FullName);
            var disposable = controller as IDisposable;
            if (disposable != null)
            {
                log.Debug("Controller implements IDisposable. Going to dispose.");
                disposable.Dispose();
                log.Debug("Controller disposed.");
            }
            else
                log.Warn("Controller does not implement IDisposable. This may indicate a " +
                    "misconfiguration and potentially lead to memory or resource leaks.");
        }
    }
}