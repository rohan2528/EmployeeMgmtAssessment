using System.ComponentModel.DataAnnotations;

namespace EmployeeMgmt.Data
{
    public class salaries
    {
        [Key]
        public int emp_no { get; set; }
        public int salary { get; set; }
        public DateTime from_date { get; set; }
        public DateTime? to_date { get; set; }
    }
}
