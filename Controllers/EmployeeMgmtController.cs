using EmployeeMgmt.Data;
using EmployeeMgmt.Model;
using Microsoft.AspNetCore.Mvc;
using EmployeeMgmt.LogicLayer;

namespace EmployeeMgmt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeMgmtController : ControllerBase
    {
        private readonly EmployeeMgmtDBContext _context;

        public EmployeeMgmtController(EmployeeMgmtDBContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult GetEmployeeList()
        {
            EmployeeDetailLogic EmpDetailLogic = new EmployeeDetailLogic(_context);
            var employees = EmpDetailLogic.getEmployeeList();

            return Ok(employees);
        }

        [HttpGet("sort")]
        public IActionResult SortEmployees([FromQuery] string sortBy)
        {
            EmployeeDetailLogic EmpDetailLogic = new EmployeeDetailLogic(_context);
            List<EmployeeDetails> query = EmpDetailLogic.getEmployeeList();

            switch (sortBy.ToLower())
            {
                case "dateofjoining":
                    query = query.OrderBy(e => e.DateOfJoining).ToList();
                    break;
                case "salary":
                    query = query.OrderBy(e => e.Salary).ToList();
                    break;
                default:
                    return BadRequest("Invalid Parameters");
            }

            var sortedEmployees = query.ToList();
            return Ok(sortedEmployees);
        }

        [HttpGet("filter")]
        public IActionResult FilterEmployees([FromQuery] string? title, [FromQuery] string? department
            , [FromQuery] decimal? minSalary, [FromQuery] decimal? maxSalary)
        {
            EmployeeDetailLogic EmpDetailLogic = new EmployeeDetailLogic(_context);
            List<EmployeeDetails> query = EmpDetailLogic.getEmployeeList();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(e => e.Title?.ToLower() == title.ToLower()).ToList();
            }

            if (!string.IsNullOrEmpty(department))
            {
                query = query.Where(e => e.Department?.ToLower() == department.ToLower()).ToList();
            }

            if (minSalary.HasValue)
            {
                query = query.Where(e => e.Salary >= minSalary.Value).ToList();
            }

            if (maxSalary.HasValue)
            {
                query = query.Where(e => e.Salary <= maxSalary.Value).ToList();
            }

            var filteredList = query.ToList();
            return Ok(filteredList);
        }

        [HttpGet("{EmployeeID}")]
        public IActionResult GetEmployeeByID(int EmployeeID)
        {
            EmployeeDetailLogic EmpDetailsLogic = new EmployeeDetailLogic(_context);
            EmployeeDetails EmployeeInfo = EmpDetailsLogic.getEmployeeList()?.FirstOrDefault(x => x.EmployeeID == EmployeeID) ?? new EmployeeDetails();
            
            if (EmployeeInfo == null || EmployeeInfo.EmployeeID == 0)
            {
                return NotFound();
            }

            return Ok(EmployeeInfo);
        }

        [HttpPost]
        public IActionResult AddNewEmployee([FromBody] EmployeeDetails EmpDetails)
        {
            int lastEmpNo = _context.employees.Max(e => (int?)e.emp_no) ?? 0;
            EmpDetails.EmployeeID = lastEmpNo + 1;
            employees newEmployee = new employees();
            newEmployee.emp_no = EmpDetails.EmployeeID;
            newEmployee.first_name = EmpDetails.First_Name;
            newEmployee.last_name = EmpDetails.Last_Name;
            newEmployee.birth_date = EmpDetails.DateOfBirth;
            newEmployee.hire_date = EmpDetails.DateOfJoining;
            newEmployee.gender = EmpDetails.Gender;

            _context.employees.Add(newEmployee);
            _context.SaveChanges();

            DateTime maxDateTimeValue = new DateTime(9999, 1, 1);

            if (EmpDetails.Salary > 0)
            {
                salaries newEmpSalary = new salaries();
                newEmpSalary.emp_no = EmpDetails.EmployeeID;
                newEmpSalary.salary = EmpDetails.Salary;
                newEmpSalary.from_date = EmpDetails.DateOfJoining;
                newEmpSalary.to_date = maxDateTimeValue;

                _context.salaries.Add(newEmpSalary);
                _context.SaveChanges();
            }
            if(!string.IsNullOrEmpty(EmpDetails.Title) && EmpDetails.Title != "string")
            {
                titles newEmployeeTitle = new titles();
                newEmployeeTitle.emp_no = EmpDetails.EmployeeID;
                newEmployeeTitle.title = EmpDetails.Title;
                newEmployeeTitle.from_date = EmpDetails.DateOfJoining;
                newEmployeeTitle.to_date = maxDateTimeValue;

                _context.titles.Add(newEmployeeTitle);
                _context.SaveChanges();
            }
            if (!string.IsNullOrEmpty(EmpDetails.Department) && EmpDetails.Title != "string")
            {
                dept_emp newDeptEmployee = new dept_emp();
                var DepartmentName = _context.departments.Where(x => x.dept_name == EmpDetails.Department).FirstOrDefault();
                if(DepartmentName == null)
                {
                    string nextDeptNo = "d" + (_context.departments.Where(x => x.dept_no == EmpDetails.Department)
                                        .OrderByDescending(x => x.dept_no)
                                        .Select(x => x.dept_no)
                                        .FirstOrDefault()) + 1;

                    departments newDepartment = new departments();
                    newDepartment.dept_no = nextDeptNo;
                    newDepartment.dept_name = EmpDetails.Department;
                    _context.Add(newDepartment);
                    _context.SaveChanges();

                    
                    newDeptEmployee.emp_no = EmpDetails.EmployeeID;
                    newDeptEmployee.dept_no = newDepartment.dept_no;
                    newDeptEmployee.from_date = EmpDetails.DateOfJoining;
                    newDeptEmployee.to_date = maxDateTimeValue;
                    _context.dept_emp.Add(newDeptEmployee);
                    _context.SaveChanges();
                }
                else
                {
                    newDeptEmployee.emp_no = EmpDetails.EmployeeID;
                    newDeptEmployee.dept_no = DepartmentName.dept_no;
                    newDeptEmployee.from_date = EmpDetails.DateOfJoining;
                    newDeptEmployee.to_date = maxDateTimeValue;
                    _context.dept_emp.Add(newDeptEmployee);
                    _context.SaveChanges();
                }
            }

            return CreatedAtAction(nameof(GetEmployeeList), new { id = EmpDetails.EmployeeID }, EmpDetails);
        }

        [HttpPut("{EmployeeID}")]
        public IActionResult Update(int EmployeeID, [FromBody] EmployeeDetails updatedEmployee)
        {
            if(EmployeeID > 0)
            {
                var employee = _context.employees.Where(x => x.emp_no == EmployeeID).FirstOrDefault();
                if(employee != null)
                {
                    if (!string.IsNullOrEmpty(updatedEmployee.First_Name))
                    {
                        employee.first_name = updatedEmployee.First_Name;
                    }

                    if (!string.IsNullOrEmpty(updatedEmployee.Last_Name) && updatedEmployee.Last_Name != "string")
                    {
                        employee.last_name = updatedEmployee.Last_Name;
                    }
                    if (updatedEmployee.DateOfBirth != DateTime.MinValue || updatedEmployee.DateOfBirth != DateTime.Now)
                    {
                        employee.birth_date = updatedEmployee.DateOfBirth;
                    }
                    if (updatedEmployee.DateOfJoining != DateTime.MinValue)
                    {
                        employee.hire_date = updatedEmployee.DateOfJoining;
                    }
                    _context.SaveChanges();
                }
                var salaryInfo = _context.salaries.Where(x => x.emp_no == EmployeeID).FirstOrDefault();
                if(salaryInfo != null)
                {
                    if(updatedEmployee.Salary > 0)
                    {
                        salaryInfo.salary = updatedEmployee.Salary;
                        _context.SaveChanges();
                    }
                }
                var TitleInfo = _context.titles.Single(x => x.emp_no == EmployeeID);
                if(TitleInfo != null)
                {
                    if(!string.IsNullOrEmpty(updatedEmployee.Title) && updatedEmployee.Title != "string")
                    {
                        TitleInfo.title = updatedEmployee.Title;
                        _context.SaveChanges();
                    }
                }
                var departmentInfo = _context.departments.FirstOrDefault(x => x.dept_name == updatedEmployee.Department.ToLower());
                var EmployeeDept = _context.dept_emp.FirstOrDefault(x => x.emp_no == EmployeeID);
                if (departmentInfo != null && EmployeeDept != null)
                {
                    if (!string.IsNullOrEmpty(updatedEmployee.Department) && updatedEmployee.Department != "string")
                    {
                        EmployeeDept.dept_no = departmentInfo.dept_no;
                        _context.SaveChanges();
                    }
                }
            }
            else
            {
                return BadRequest("EmployeeID should be greater than 0.");
            }

            EmployeeDetailLogic EmpDetailsLogic = new EmployeeDetailLogic(_context);
            EmployeeDetails EmployeeInfo = EmpDetailsLogic.getEmployeeList()?.FirstOrDefault(x => x.EmployeeID == EmployeeID) ?? new EmployeeDetails();


            return Ok(EmployeeInfo);
        }

        [HttpDelete]
        public IActionResult Delete(int EmployeeID)
        {
            var employee = _context.employees.Where(x => x.emp_no == EmployeeID).FirstOrDefault();
            if (employee == null)
            {
                return NotFound();
            }
            _context.employees.Remove(employee);
            _context.SaveChanges();

            return Ok();
        }

    }
}
