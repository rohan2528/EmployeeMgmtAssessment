using System.ComponentModel.DataAnnotations;

namespace EmployeeMgmt.Data
{
    public class departments
    {
        [Key]
        public string? dept_no { get; set; }
        public string? dept_name { get; set; }
    }
}
