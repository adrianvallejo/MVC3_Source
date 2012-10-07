﻿namespace Microsoft.Web.Mvc {
    using System;
    using System.Reflection;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AjaxOnlyAttribute : ActionMethodSelectorAttribute {

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo) {
            if (controllerContext == null) {
                throw new ArgumentNullException("controllerContext");
            }

            // Dev10 #939671 - If this attribute is going to say AJAX *only*, then we need to check the header
            // specifically, as otherwise clients can modify the form or query string to contain the name/value
            // pair we're looking for.
            return (controllerContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest");
        }

    }
}
