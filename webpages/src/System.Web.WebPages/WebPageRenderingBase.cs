﻿using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Web.Caching;
using Microsoft.Internal.Web.Utils;

namespace System.Web.WebPages {
    public abstract class WebPageRenderingBase : WebPageExecutingBase, ITemplateFile {
        private IPrincipal _user;
        private UrlDataList _urlData;
        private TemplateFileInfo _templateFileInfo;

        public virtual Cache Cache {
            get {
                if (Context != null) {
                    return Context.Cache;
                }
                return null;
            }
        }

        public abstract string Layout {
            get;
            set;
        }

        public abstract IDictionary<object, dynamic> PageData {
            get;
        }

        public abstract dynamic Page {
            get;
        }

        public WebPageContext PageContext {
            get;
            internal set;
        }

        public virtual HttpRequestBase Request {
            get {
                if (Context != null) {
                    return Context.Request;
                }
                return null;
            }
        }

        public virtual HttpResponseBase Response {
            get {
                if (Context != null) {
                    return Context.Response;
                }
                return null;
            }
        }

        public virtual HttpServerUtilityBase Server {
            get {
                if (Context != null) {
                    return Context.Server;
                }
                return null;
            }
        }

        public virtual HttpSessionStateBase Session {
            get {
                if (Context != null) {
                    return Context.Session;
                }
                return null;
            }
        }

        public virtual IList<string> UrlData {
            get {
                if (_urlData == null) {
                    WebPageMatch match = WebPageRoute.GetWebPageMatch(Context);
                    if (match != null) {
                        _urlData = new UrlDataList(match.PathInfo);
                    }
                    else {
                        // REVIEW: Can there ever be no route match?
                        _urlData = new UrlDataList(null);
                    }
                }
                return _urlData;
            }
        }

        public virtual IPrincipal User {
            get {
                if (_user == null) {
                    return Context.User;
                }
                return _user;
            }
            internal set {
                _user = value;
            }
        }

        public virtual TemplateFileInfo TemplateInfo {
            get {
                if (_templateFileInfo == null) {
                    _templateFileInfo = new TemplateFileInfo(VirtualPath);
                }
                return _templateFileInfo;
            }
        }

        public virtual bool IsPost {
            get {
                return Request.HttpMethod == "POST";
            }
        }

        public virtual bool IsAjax {
            get {
                var request = Request;
                if (request == null) {
                    return false;
                }
                return (request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest"));
            }
        }

        public virtual string Href(string path, params object[] pathParts) {
            return Util.Url(VirtualPath, path, pathParts);
        }

        public string Culture {
            get {
                return Thread.CurrentThread.CurrentCulture.Name;
            }
            set {
                if (String.IsNullOrEmpty(value)) {
                    // GetCultureInfo accepts empty strings but throws for null strings. To maintain consistency in our string handling behavior, throw
                    throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, "value");
                }
                CultureUtil.SetCulture(Thread.CurrentThread, Context, value);
            }
        }

        public string UICulture {
            get {
                return Thread.CurrentThread.CurrentUICulture.Name;
            }
            set {
                if (String.IsNullOrEmpty(value)) {
                    // GetCultureInfo accepts empty strings but throws for null strings. To maintain consistency in our string handling behavior, throw
                    throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, "value");
                }
                CultureUtil.SetUICulture(Thread.CurrentThread, Context, value);
            }
        }

        // Calls the Execute() method, and calls RunPage() if the page is an InitPage but
        // did not call RunPage().
        public abstract void ExecutePageHierarchy();

        public abstract HelperResult RenderPage(string path, params object[] data);
    }
}
