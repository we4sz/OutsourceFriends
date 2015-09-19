using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SelfieJobs.Filters
{
    public class RequireSecureConnectionFilter : RequireHttpsAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.HttpContext.Request.IsLocal)
            {
                // when connection to the application is local, don't do any HTTPS stuff
                return;
            }

            base.OnAuthorization(filterContext);
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ParameterNameMapAttribute : ActionFilterAttribute
    {
        public string InboundParameterName { get; set; }
        public string ActionParameterName { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object value = filterContext.RequestContext.HttpContext.Request.Unvalidated[InboundParameterName];

            if (filterContext.ActionParameters.ContainsKey(ActionParameterName))
            {
                filterContext.ActionParameters[ActionParameterName] = value;
            }
            else
            {
                throw new Exception("Parameter not found on controller: " + ActionParameterName);
            }
        }
    }
}