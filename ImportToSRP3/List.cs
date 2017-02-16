//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ImportToSRP3
{
    using System;
    using System.Collections.Generic;
    
    public partial class List
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public List()
        {
            this.ListDisplayColumns = new HashSet<ListDisplayColumn>();
            this.ListFilterColumns = new HashSet<ListFilterColumn>();
            this.ListSortColumns = new HashSet<ListSortColumn>();
        }
    
        public long Id { get; set; }
        public string Name { get; set; }
        public string ListType { get; set; }
        public string ListSubType { get; set; }
        public string EntityType { get; set; }
        public string ListKey { get; set; }
        public string ListGroup { get; set; }
        public string QueryPattern { get; set; }
        public string MainTable { get; set; }
        public bool IsPredefined { get; set; }
        public Nullable<short> Order { get; set; }
        public bool IsDefault { get; set; }
        public Nullable<long> ReferenceId { get; set; }
        public bool HasQuickFilter { get; set; }
        public bool HasListDetails { get; set; }
        public System.DateTime CreatedTimestamp { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime LastUpdatedTimestamp { get; set; }
        public System.Guid LastUpdatedBy { get; set; }
        public Nullable<long> ExportListId { get; set; }
        public bool IsIncludeSummaryRow { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ListDisplayColumn> ListDisplayColumns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ListFilterColumn> ListFilterColumns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ListSortColumn> ListSortColumns { get; set; }
    }
}
