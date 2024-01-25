using System.ComponentModel.DataAnnotations;

namespace EmployeeMgmt.Data
{
    public class dept_emp
    {
        [Key]
        public int emp_no { get; set; }
        public string? dept_no { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
    }
}
