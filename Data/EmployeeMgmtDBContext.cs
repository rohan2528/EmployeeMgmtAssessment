using Microsoft.EntityFrameworkCore;

namespace EmployeeMgmt.Data
{
    public class EmployeeMgmtDBContext : DbContext 
    {
        public EmployeeMgmtDBContext(DbContextOptions<EmployeeMgmtDBContext> options) : base(options) { }
        
        public virtual DbSet<employees> employees { get; set; }
        public virtual DbSet<departments> departments { get; set; }
        public virtual DbSet<dept_emp> dept_emp { get; set; }
        public virtual DbSet<titles> titles { get; set; }
        public virtual DbSet<salaries> salaries { get; set; }
    }
}
