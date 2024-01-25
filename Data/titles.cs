using System.ComponentModel.DataAnnotations;

namespace EmployeeMgmt.Data
{
    public class titles
    {
        [Key]
        public int emp_no { get; set; }
        public string? title { get; set; }
        public DateTime from_date { get; set; }
        public DateTime? to_date { get; set; }
    }
}
