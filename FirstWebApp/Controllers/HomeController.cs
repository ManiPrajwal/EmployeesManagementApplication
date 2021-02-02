using FirstWebApp.Models;
using FirstWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebApp.Controllers
{
    [Authorize]
    //[Route("Home")]
    //[Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository,
            IHostingEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }
        //[Route("~/Home")]
        //[Route("Home")]
        //[Route("Index")]
        //[Route("[action]")]
        //[Route("~/")]
        [AllowAnonymous]
        public ViewResult Index() 
        {
            var model =  _employeeRepository.GetAllEmployees();
            return View(model);
        }
        [HttpGet]
        public ViewResult Create() 
        {
            return View();
        }
        [HttpPost]
        
        public IActionResult Create(EmployeeCreateViewModel model) 
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    ImagePath = uniqueFileName
                };
                _employeeRepository.Add(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }
        //[Route("{id?}")]
        public ViewResult Details(int? id)
        {
            Employee employee = _employeeRepository.GetEmployee(id.Value);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmplyeeNotFound", id.Value);
            }
            HomeDetaisViewModel homeDetaisViewModel = new HomeDetaisViewModel()
            {
                Employee = employee,
                PageTitle = "Employee Details"
            };
            
            return View(homeDetaisViewModel);
        }
        [HttpGet]
        
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditModelView = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.ImagePath
            };
            return View(employeeEditModelView);
        }
        /// <summary name = "code for multiple images" >
        ///                 //string uniqueFileName = null;
        ///if (model.Photos != null && model.Photos.Count > 0)
        ///{
        ///    foreach (IFormFile photo in model.Photos)
        ///   {
        ///       string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
        ///        // To make sure the file name is unique we are appending a new
        ///        // GUID value and and an underscore to the file name
        ///       //var fileName = Path.GetFileName(model.Photo.FileName);
        ///       uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
        ///       string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        ///      // Use CopyTo() method provided by IFormFile interface to
        ///       // copy the file to wwwroot/images folder
        ///       photo.CopyTo(new FileStream(filePath, FileMode.Create));
        ///  }
        ///}
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
       
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Id = model.Id;
                employee.Name = model.Name;
                employee.Department = model.Department;
                employee.Email = model.Email;
                if (model.Photos != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.ImagePath = ProcessUploadedFile(model);
                }
               
                _employeeRepository.Update(employee);
                return RedirectToAction("index");
            }
            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photos != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                // To make sure the file name is unique we are appending a new
                // GUID value and and an underscore to the file name
                //var fileName = Path.GetFileName(model.Photo.FileName);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Photos.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                // Use CopyTo() method provided by IFormFile interface to
                // copy the file to wwwroot/images folder
                using (var resourcePath = new FileStream(filePath, FileMode.Create))
                {
                    model.Photos.CopyTo(resourcePath);
                } 

            }

            return uniqueFileName;
        }
    }
}
