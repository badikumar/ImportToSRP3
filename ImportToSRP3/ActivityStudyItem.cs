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
    
    public partial class ActivityStudyItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ActivityStudyItem()
        {
            this.ActivityStudyItemIndividuals = new HashSet<ActivityStudyItemIndividual>();
        }
    
        public long Id { get; set; }
        public string DisplayStartDate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string DisplayEndDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool IsCompleted { get; set; }
        public long ActivityId { get; set; }
        public long StudyItemId { get; set; }
        public System.DateTime CreatedTimestamp { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime LastUpdatedTimestamp { get; set; }
        public System.Guid LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> ImportedTimestamp { get; set; }
        public Nullable<System.Guid> ImportedFrom { get; set; }
        public string ImportedFileType { get; set; }
    
        public virtual Activity Activity { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActivityStudyItemIndividual> ActivityStudyItemIndividuals { get; set; }
        public virtual StudyItem StudyItem { get; set; }
    }
}
