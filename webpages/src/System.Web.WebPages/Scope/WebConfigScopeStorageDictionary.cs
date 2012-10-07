﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Configuration;
using System.Web.WebPages.Resources;

namespace System.Web.WebPages.Scope {
    internal class WebConfigScopeDictionary : IDictionary<object, object> {
        private readonly Lazy<Dictionary<object, object>> _items;

        public WebConfigScopeDictionary()
            : this(WebConfigurationManager.AppSettings) {
        }

        public WebConfigScopeDictionary(NameValueCollection appSettings) {
            _items = new Lazy<Dictionary<object, object>>(
                () => appSettings.AllKeys.ToDictionary(key => key, key => (object)appSettings[key], ScopeStorageComparer.Instance)
            );
        }

        public object this[object key] {
            get {
                object value;
                TryGetValue(key, out value);
                return value;
            }
            set {
                throw new NotSupportedException(WebPageResources.StateStorage_ScopeIsReadOnly);
            }
        }

        private IDictionary<object, object> Items {
            get {
                return _items.Value;
            }
        }

        public bool TryGetValue(object key, out object value) {
            return Items.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<object, object>> GetEnumerator() {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(object key, object value) {
            throw new NotSupportedException(WebPageResources.StateStorage_ScopeIsReadOnly);
        }

        public bool ContainsKey(object key) {
            return Items.ContainsKey(key);
        }

        public ICollection<object> Keys {
            get {
                return Items.Keys;
            }
        }

        public bool Remove(object key) {
            throw new NotSupportedException(WebPageResources.StateStorage_ScopeIsReadOnly);
        }

        public ICollection<object> Values {
            get {
                return Items.Values;
            }
        }

        public void Add(KeyValuePair<object, object> item) {
            throw new NotSupportedException(WebPageResources.StateStorage_ScopeIsReadOnly);
        }

        public void Clear() {
            throw new NotSupportedException(WebPageResources.StateStorage_ScopeIsReadOnly);
        }

        public bool Contains(KeyValuePair<object, object> item) {
            return Items.Contains(item);
        }

        public void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex) {
            Items.CopyTo(array, arrayIndex);
        }

        public int Count {
            get {
                return Items.Count;
            }
        }

        public bool IsReadOnly {
            get {
                return true;
            }
        }

        public bool Remove(KeyValuePair<object, object> item) {
            throw new NotSupportedException(WebPageResources.StateStorage_ScopeIsReadOnly);
        }
    }
}
