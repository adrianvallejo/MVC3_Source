﻿using System.IO;
using System.Collections.Generic;
using System.Web.WebPages.ApplicationParts;

namespace System.Web.WebPages.Test {
    public abstract class TestResourceAssembly : IResourceAssembly {
        public abstract string Name {
            get;
        }

        public abstract Stream GetManifestResourceStream(string name);

        public abstract IEnumerable<string> GetManifestResourceNames();

        public abstract IEnumerable<Type> GetTypes();
    }
}
