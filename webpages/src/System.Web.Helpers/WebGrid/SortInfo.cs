﻿
namespace System.Web.Helpers {
    internal sealed class SortInfo : IEquatable<SortInfo> {
        public string SortColumn { get; set; }

        public SortDirection SortDirection { get; set; }

        public bool Equals(SortInfo other) {
            return other != null 
                && String.Equals(SortColumn, other.SortColumn, StringComparison.OrdinalIgnoreCase) 
                && SortDirection == other.SortDirection;
        }

        public override bool Equals(object obj) {
            SortInfo sortInfo = obj as SortInfo;
            if (sortInfo != null) {
                return this.Equals(sortInfo);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return SortColumn.GetHashCode();
        }
    }
}
