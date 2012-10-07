﻿using System.Collections.Generic;

namespace System.Web.WebPages.Scope {
    /// <summary>
    /// Custom comparer for the context dictionaries
    /// The comparer treats strings as a special case, performing case insesitive comparison. 
    /// This guaratees that we remain consistent throughout the chain of contexts since PageData dictionary 
    /// behaves in this manner.
    /// </summary>
    internal class ScopeStorageComparer : IEqualityComparer<object> {
        private readonly IEqualityComparer<object> _defaultComparer = EqualityComparer<object>.Default;
        private readonly IEqualityComparer<string> _stringComparer = StringComparer.OrdinalIgnoreCase;
        private static IEqualityComparer<object> _instance;

        public static IEqualityComparer<object> Instance {
            get {
                if (_instance == null) {
                    _instance = new ScopeStorageComparer();
                }
                return _instance;
            }
        }

        private ScopeStorageComparer() { }

        public new bool Equals(object x, object y) {
            string xString = x as string;
            string yString = y as string;

            if ((xString != null) && (yString != null)) {
                return _stringComparer.Equals(xString, yString);
            }

            return _defaultComparer.Equals(x, y);
        }

        public int GetHashCode(object obj) {
            string objString = obj as string;
            if (objString != null) {
                return _stringComparer.GetHashCode(objString);
            }

            return _defaultComparer.GetHashCode(obj);
        }
    }
}
