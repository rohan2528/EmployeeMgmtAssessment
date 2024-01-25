namespace EmployeeMgmt.Model
{
    public class EmployeeDetails
    {
        public int EmployeeID { get; set; }
        public string? First_Name { get; set; }
        public string? Last_Name { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Salary { get; set; }
        public string? Title { get; set; }
        public string? Department { get; set; }
        public string? Gender { get; set; }
    }
}
