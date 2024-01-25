using EmployeeMgmt.Data;
using EmployeeMgmt.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMgmt.LogicLayer
{
    public class EmployeeDetailLogic
    {
        private readonly EmployeeMgmtDBContext _context;

        public EmployeeDetailLogic(EmployeeMgmtDBContext context)
        {
            _context = context;
        }
        public List<EmployeeDetails> getEmployeeList()
        {
            List<EmployeeDetails> employeeList = new List<EmployeeDetails>();
            try
            {
                employeeList = _context.employees
                   .Join(_context.dept_emp.Distinct(), x => x.emp_no, e => e.emp_no, (x, e) => new { Employee = x, DepartEmp = e })
                   .Join(_context.departments, x => x.DepartEmp.dept_no, d => d.dept_no, (x, d) => new { Employee = x.Employee, DepartEmp = x.DepartEmp, Departments = d })
                   .Join(_context.salaries, x => x.Employee.emp_no, s => s.emp_no, (x, s) => new { Employee = x.Employee, DepartEmp = x.DepartEmp, Departments = x.Departments, Salaries = s })
                   .Join(_context.titles, x => x.Employee.emp_no, t => t.emp_no, (x , t) => new { Employee = x.Employee, DepartEmp = x.DepartEmp, Departments = x.Departments, Salaries = x.Salaries, Titles = t })
                   .Select(x => new EmployeeDetails
                   {
                       EmployeeID = x.Employee.emp_no,
                       First_Name = x.Employee.first_name ?? "",
                       Last_Name = x.Employee.last_name ?? "",
                       DateOfBirth = x.Employee.birth_date,
                       DateOfJoining = x.Employee.hire_date,
                       Department = x.Departments.dept_name, 
                       Salary = x.Salaries.salary, 
                       Title = x.Titles.title,
                       Gender = x.Employee.gender,

                   }).Distinct().ToList();
                return employeeList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
