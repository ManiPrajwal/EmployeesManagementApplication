using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebApp.Models
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
          {
            modelBuilder.Entity<Employee>().HasData
              (
                  new Employee
                  {
                      Id = 1,
                      Name = "Mani Prajwal Katta",
                      Department = Dept.IT,
                      Email = "maniprajwalkatta@techm.com"
                  },
                  new Employee
                  {
                      Id = 3,
                      Name = "Prajwal",
                      Department = Dept.IT,
                      Email = "prajwal@techm.com"
                  },
                  new Employee
                  {
                      Id = 2,
                      Name = "Mani Prajwal",
                      Department = Dept.HR,
                      Email = "maniprajwal@techm.com"
                  }
          );
        }
    }
}
