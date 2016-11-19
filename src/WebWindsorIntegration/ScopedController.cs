using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using log4net;
using MyAB.Assertions;

namespace Web.Windsor
{
    /// <summary>
    /// Wraps a standard ASP.NET MVC controller and provides deterministic disposal of the Windsor
    /// dependency scope under which it was created.
    /// </summary>
    public class ScopedController : Controller, IDisposable
    {
        private static readonly MethodInfo BeginExecuteMethod =
            typeof(Controller).GetMethod("BeginExecute",
                BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly MethodInfo EndExecuteMethod =
            typeof(Controller).GetMethod("EndExecute",
                BindingFlags.Instance | BindingFlags.NonPublic);

        private readonly IController actualController;
        private readonly IDisposable scope;
        private readonly bool isStandardController;
        private readonly ILog log = LogManager.GetLogger(typeof(ScopedController));

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopedController"/> class with a reference
        /// to the actual controller and the original Windsor lifestyle scope.
        /// </summary>
        /// <param name="actualController">The controller which will actually handle
        /// requests.</param>
        /// <param name="scope">A disposable object representing the Windsor lifestyle scope under
        /// which the <paramref name="actualController"/> was first resolved.</param>
        public ScopedController(IController actualController, IDisposable scope)
        {
            Ensure.ArgumentNotNull(actualController, "actualController");
            Ensure.ArgumentNotNull(scope, "scope");
            this.actualController = actualController;
            this.scope = scope;
            this.isStandardController = actualController is Controller;
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                log.DebugFormat("Disposing scoped controller, original type = {0}",
                    actualController.GetType().FullName);
                scope.Dispose();
                log.Debug("Dependency scope disposed");
            }
            base.Dispose(disposing);
        }

        /// <inheritdoc />
        protected override IAsyncResult BeginExecute(RequestContext requestContext,
            AsyncCallback callback, object state)
        {
            return (IAsyncResult)BeginExecuteMethod.Invoke(actualController,
                new[] { requestContext, callback, state });
        }

        /// <inheritdoc />
        protected override void EndExecute(IAsyncResult asyncResult)
        {
            if (!isStandardController)
                throw new InvalidOperationException(string.Format("Cannot execute an " +
                    "asynchronous method because the wrapped controller does not inherit from {0}.",
                    typeof(Controller).FullName));
            EndExecuteMethod.Invoke(actualController, new[] { asyncResult });
        }

        /// <inheritdoc />
        protected override void Execute(RequestContext requestContext)
        {
            if (!isStandardController)
                throw new InvalidOperationException(string.Format("Cannot execute an " +
                    "asynchronous method because the wrapped controller does not inherit from {0}.",
                    typeof(Controller).FullName));
            actualController.Execute(requestContext);
        }
    }
}