using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GroupJoinQueryType.Models
{
    /// <summary>
    /// Student View
    /// </summary>
    public class Student
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public int? Age { get; set; }
    }
}
