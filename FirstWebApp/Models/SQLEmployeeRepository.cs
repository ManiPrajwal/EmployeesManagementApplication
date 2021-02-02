using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebApp.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private AppDbContext _appDbContext;
        public SQLEmployeeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public Employee Add(Employee employee)
        {
            _appDbContext.Employees.Add(employee);
            _appDbContext.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            var emp =_appDbContext.Employees.Find(id);
            if (emp != null)
            {
                _appDbContext.Employees.Remove(emp);
                _appDbContext.SaveChanges();
                
            }
            return emp;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _appDbContext.Employees;
        }

        public Employee GetEmployee(int Id)
        {
            return _appDbContext.Employees.Find(Id);        
        }

        public Employee Update(Employee employeechange)
        {
            var empchanges = _appDbContext.Employees.Attach(employeechange);
            empchanges.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _appDbContext.SaveChanges();
            return employeechange;
        }
    }
}
