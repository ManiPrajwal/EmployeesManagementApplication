using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebApp.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>
            { 
                new Employee(){ Id = 123, Name="One", Department=Dept.HR, Email="abc@gmail.com" },
                new Employee(){ Id = 456, Name="Two", Department=Dept.IT, Email="def@gmail.com"},
                new Employee(){ Id = 789, Name="Three", Department=Dept.Payroll, Email="ghi@gmail.com"}
            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id)+333;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            var empfound = _employeeList.FirstOrDefault(e => e.Id == id);
            if (empfound != null)
            {
                _employeeList.Remove(empfound);
            }
            return empfound;
        }

        public Employee Update(Employee employeechange)
        {
            var empfound = _employeeList.FirstOrDefault(e => e.Id == employeechange.Id);
            if (empfound != null)
            {
                empfound.Name = employeechange.Name;
                empfound.Department = employeechange.Department;
                empfound.Email = employeechange.Email;
            }
            return empfound;
        }
        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(e => e.Id==Id);
        }

       
    }
}
