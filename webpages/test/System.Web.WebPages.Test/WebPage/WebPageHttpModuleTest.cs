﻿using System;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.WebPages.TestUtils;

namespace System.Web.WebPages.Test {
    [TestClass]
    public class WebPageHttpModuleTest {
        [TestMethod]
        public void InitializeApplicationTest() {
            AppDomainUtils.RunInSeparateAppDomain(() => {
                var moduleEvents = new ModuleEvents();
                var app = new MyHttpApplication();
                WebPageHttpModule.InitializeApplication(app,
                    moduleEvents.OnApplicationPostResolveRequestCache,
                    moduleEvents.Initialize);
                Assert.IsTrue(moduleEvents.CalledInitialize);
            });

        }

        [TestMethod]
        public void StartApplicationTest() {
            AppDomainUtils.RunInSeparateAppDomain(() => {
                var moduleEvents = new ModuleEvents();
                var app = new MyHttpApplication();
                WebPageHttpModule.StartApplication(app, moduleEvents.ExecuteStartPage, moduleEvents.ApplicationStart);
                Assert.AreEqual(1, moduleEvents.CalledExecuteStartPage);
                Assert.AreEqual(1, moduleEvents.CalledApplicationStart);

                // Call a second time to make sure the methods are only called once
                WebPageHttpModule.StartApplication(app, moduleEvents.ExecuteStartPage, moduleEvents.ApplicationStart);
                Assert.AreEqual(1, moduleEvents.CalledExecuteStartPage);
                Assert.AreEqual(1, moduleEvents.CalledApplicationStart);
            });
        }

        public class MyHttpApplication : HttpApplication {
            public MyHttpApplication() {
            }
        }

        public class ModuleEvents {
            public void OnApplicationPostResolveRequestCache(object sender, EventArgs e) {
            }

            public void OnBeginRequest(object sender, EventArgs e) {
            }

            public void OnEndRequest(object sender, EventArgs e) {
            }

            public bool CalledInitialize;
            public void Initialize(object sender, EventArgs e) {
                CalledInitialize = true;
            }

            public int CalledExecuteStartPage;
            public void ExecuteStartPage(HttpApplication application) {
                CalledExecuteStartPage++;
            }

            public int CalledApplicationStart;
            public void ApplicationStart(object sender, EventArgs e) {
                CalledApplicationStart++;
            }
        }
    }
}
