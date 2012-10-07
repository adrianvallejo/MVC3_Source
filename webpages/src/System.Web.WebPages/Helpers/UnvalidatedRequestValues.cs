﻿using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace System.Web.Helpers {

    // Provides access to Request.* collections, except that these have not gone through request validation.
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unvalidated", Justification = "Matches FX45 naming.")]
    public sealed class UnvalidatedRequestValues {

        private readonly HttpRequestBase _request;
        private readonly Func<NameValueCollection> _formGetter;
        private readonly Func<NameValueCollection> _queryStringGetter;

        internal UnvalidatedRequestValues(HttpRequestBase request, Func<NameValueCollection> formGetter, Func<NameValueCollection> queryStringGetter) {
            _request = request;
            _formGetter = formGetter;
            _queryStringGetter = queryStringGetter;
        }

        public NameValueCollection Form { get { return _formGetter(); } }
        public NameValueCollection QueryString { get { return _queryStringGetter(); } }

        // this item getter follows the same logic as HttpRequest.get_Item
        public string this[string key] {
            get {
                string queryStringValue = QueryString[key];
                if (queryStringValue != null) {
                    return queryStringValue;
                }

                string formValue = Form[key];
                if (formValue != null) {
                    return formValue;
                }

                HttpCookie cookie = _request.Cookies[key];
                if (cookie != null) {
                    return cookie.Value;
                }

                string serverVarValue = _request.ServerVariables[key];
                if (serverVarValue != null) {
                    return serverVarValue;
                }

                return null;
            }
        }

    }
}
