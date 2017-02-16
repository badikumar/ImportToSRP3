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
    
    public partial class Individual
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Individual()
        {
            this.ActivityStudyItemIndividuals = new HashSet<ActivityStudyItemIndividual>();
            this.IndividualEmails = new HashSet<IndividualEmail>();
            this.IndividualPhones = new HashSet<IndividualPhone>();
        }
    
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public Nullable<byte> Gender { get; set; }
        public Nullable<short> EstimatedYearOfBirthDate { get; set; }
        public bool IsSelectedEstimatedYearOfBirthDate { get; set; }
        public string DisplayBirthDate { get; set; }
        public System.DateTime BirthDate { get; set; }
        public bool IsRegisteredBahai { get; set; }
        public string DisplayRegistrationDate { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public Nullable<System.DateTime> UnRegisteredTimestamp { get; set; }
        public string Address { get; set; }
        public bool IsArchived { get; set; }
        public Nullable<bool> IsNonDuplicate { get; set; }
        public bool LegacyDataHadCurrentlyAttendingChildrensClass { get; set; }
        public bool LegacyDataHadCurrentlyParticipatingInAJuniorYouthGroup { get; set; }
        public string Comments { get; set; }
        public long LocalityId { get; set; }
        public Nullable<long> SubdivisionId { get; set; }
        public System.DateTime CreatedTimestamp { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime LastUpdatedTimestamp { get; set; }
        public System.Guid LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> ArchivedTimestamp { get; set; }
        public Nullable<System.DateTime> ImportedTimestamp { get; set; }
        public Nullable<System.Guid> ImportedFrom { get; set; }
        public string ImportedFileType { get; set; }
        public System.Guid GUID { get; set; }
        public string LegacyId { get; set; }
        public string InstituteId { get; set; }
        public bool WasLegacyRecord { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActivityStudyItemIndividual> ActivityStudyItemIndividuals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IndividualEmail> IndividualEmails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IndividualPhone> IndividualPhones { get; set; }
        public virtual Locality Locality { get; set; }
        public virtual Subdivision Subdivision { get; set; }
    }
}
