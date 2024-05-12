//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Online_Exam_Portal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExamQuestion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExamQuestion()
        {
            this.ExamSubmissions = new HashSet<ExamSubmission>();
        }
    
        public int question_id { get; set; }
        public Nullable<int> content_id { get; set; }
        public Nullable<int> topic_id { get; set; }
        public string question_text { get; set; }
        public string option_1 { get; set; }
        public string option_2 { get; set; }
        public string option_3 { get; set; }
        public string option_4 { get; set; }
        public Nullable<int> correct_option_number { get; set; }
    
        public virtual Content Content { get; set; }
        public virtual Topic Topic { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExamSubmission> ExamSubmissions { get; set; }
    }
}
