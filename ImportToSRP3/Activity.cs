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
    
    public partial class Activity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Activity()
        {
            this.ActivityStudyItems = new HashSet<ActivityStudyItem>();
            this.ActivityStudyItemIndividuals = new HashSet<ActivityStudyItemIndividual>();
        }
    
        public long Id { get; set; }
        public byte ActivityType { get; set; }
        public string DisplayStartDate { get; set; }
        public System.DateTime StartDate { get; set; }
        public string DisplayEndDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Comments { get; set; }
        public bool IsCompleted { get; set; }
        public Nullable<bool> HasServiceProjects { get; set; }
        public Nullable<int> Participants { get; set; }
        public Nullable<int> BahaiParticipants { get; set; }
        public long LocalityId { get; set; }
        public Nullable<long> SubdivisionId { get; set; }
        public bool IsOverrideParticipantCounts { get; set; }
        public System.DateTime CreatedTimestamp { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime LastUpdatedTimestamp { get; set; }
        public System.Guid LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> ImportedTimestamp { get; set; }
        public Nullable<System.Guid> ImportedFrom { get; set; }
        public string ImportedFileType { get; set; }
        public System.Guid GUID { get; set; }
        public string LegacyId { get; set; }
        public string InstituteId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActivityStudyItem> ActivityStudyItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActivityStudyItemIndividual> ActivityStudyItemIndividuals { get; set; }
        public virtual Locality Locality { get; set; }
        public virtual Subdivision Subdivision { get; set; }
    }
}
