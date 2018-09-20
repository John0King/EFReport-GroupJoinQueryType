using GroupJoinQueryType.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GroupJoinQueryType
{
    public class StudentInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public IReadOnlyCollection<StudentExtendField> ExtendInfo { get; set; } = new List<StudentExtendField>();
    }
}
