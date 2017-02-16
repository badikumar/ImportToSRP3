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
    
    public partial class ListColumn
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ListColumn()
        {
            this.ListDisplayColumns = new HashSet<ListDisplayColumn>();
            this.ListFilterColumns = new HashSet<ListFilterColumn>();
            this.ListSortColumns = new HashSet<ListSortColumn>();
        }
    
        public long Id { get; set; }
        public string EntityType { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string SortColumnName { get; set; }
        public string FilterColumnName { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsCalculated { get; set; }
        public string Expression { get; set; }
        public bool IsAvailableListColumn { get; set; }
        public bool IsRequiredListColumn { get; set; }
        public bool IsSelectableListColumn { get; set; }
        public bool IsOrderableListColumn { get; set; }
        public bool IsFilterableListColumn { get; set; }
        public bool IsAvailableReportColumn { get; set; }
        public bool IsRequiredReportColumn { get; set; }
        public bool IsSelectableReportColumn { get; set; }
        public bool IsOrderableReportColumn { get; set; }
        public bool IsFilterableReportColumn { get; set; }
        public string ColumnType { get; set; }
        public short Order { get; set; }
        public System.DateTime CreatedTimestamp { get; set; }
        public System.DateTime LastUpdatedTimestamp { get; set; }
        public bool IsAvailableExportColumn { get; set; }
        public Nullable<long> ListColumnGroupId { get; set; }
        public string ColumnCategory { get; set; }
        public string DBSortColumnName { get; set; }
        public string DBFilterColumnName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ListDisplayColumn> ListDisplayColumns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ListFilterColumn> ListFilterColumns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ListSortColumn> ListSortColumns { get; set; }
    }
}
