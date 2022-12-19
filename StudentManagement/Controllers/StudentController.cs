using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentManagement.Controllers
{
    public class StudentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IStudentServices _studentService;
        public StudentController(IConfiguration configuration, IStudentServices studentServices)
        {
            _configuration = configuration;
            _studentService = studentServices;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult StudentRecords()
        {
            AllModel model = new AllModel();

            model.StudentRecords = _studentService.GetStudents().ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult PostnPutStudentRecord(Student formData)
        {
            string studentResponse = string.Empty;
            formData.CreatedBy = 1;
            formData.CreatedOn = DateTime.Now;

            string url = Request.Headers["Referer"].ToString();

            if (formData.StudentId>0)
                studentResponse = _studentService.UpdateStudent(formData);
            else
                studentResponse = _studentService.InsertStudent(formData);
            
            if (studentResponse== "Saved Successfully")
            {
                TempData["SuccessMsg"] = "Saved Successfully";
                return Redirect(url);
            }
            else
            {
                TempData["ErrorMsg"] = studentResponse;
                return Redirect(url);
            }
            
        }
        [HttpPost]
        public JsonResult DeleteStudentRecord(int StudentId)
        {
            string url = Request.Headers["Referer"].ToString();
            string studentResponse = _studentService.DeleteStudent(StudentId);
            if (studentResponse == "Deleted Successfully")
            {
                TempData["SuccessMsg"] = "Deleted Successfully";
                return Json(new { message = TempData["SuccessMsg"] });
                
            }
            else
            {
                TempData["ErrorMsg"] = studentResponse;
                return Json(new { message = TempData["ErrorMsg"] });
            }
        }
    }
}

