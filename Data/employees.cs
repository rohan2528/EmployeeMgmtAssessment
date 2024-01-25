using System.ComponentModel.DataAnnotations;

namespace EmployeeMgmt.Data
{
    public class employees
    {
        [Key]
        public int emp_no { get; set; }
        public DateTime birth_date { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public DateTime hire_date { get; set; }
        // public DateTime DateOfBirth { get; set; }
        public string? gender { get; set; }
        //public string? Title { get; set; }
        //public string? Department { get; set; }
    }
}
