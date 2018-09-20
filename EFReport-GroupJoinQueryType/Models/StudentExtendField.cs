using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GroupJoinQueryType.Models
{
    public class StudentExtendField
    {
        public int Id { get; set; }
        public int StudentId { get; set; }

        [Required]
        [StringLength(50)]
        public string FieldName { get; set; }
        public string FieldValue { get; set; }

    }
}
