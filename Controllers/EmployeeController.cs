using Register.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace Register.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Register()
        {
            return View();
        }
       



        [HttpGet]
        public ActionResult EditEmployee(Guid id)
        {
            using (EmployeeEntities1 db = new EmployeeEntities1())
            {
                var data = db.Edata.SingleOrDefault(x => x.Id == id);
                if (data != null)
                {
                    var model = new EmployeeRegister
                    {
                        Id = data.Id,
                        FirstName = data.FirstName,
                        LastName = data.LastName,
                        Email = data.Email,
                        Password = data.Password
                    };
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }

        }



        [HttpPost]
        public ActionResult Register(EmployeeRegister Reg)
        {
            if (!ModelState.IsValid)// Check Vaidation
            {
                ViewBag.RegisterError = "Input data is  Not valid";
                return View();
            }





            using (EmployeeEntities1 db = new EmployeeEntities1()) // Created Instanse For DataBase To Save A Data


            {

                var IsExist = db.Edata.Any(x => x.Email.ToLower() == Reg.Email.ToLower());

                if (IsExist)
                {
                    ViewBag.RegisterError = $"User Already Exist with email id : {Reg.Email}";
                    return View();
                }
                else
                {
                    ViewBag.RegisterSuccess = "User Added Successfully";

                }

                Edata data = new Edata()    // Object Created From DB Model
                                           // Mapping Data From DataBase Class And Mapping Class
                {
                    Id = Guid.NewGuid(),
                    FirstName = Reg.FirstName,
                    LastName = Reg.LastName,
                    Email = Reg.Email,
                    Password = Reg.Password,
                    CreatedOn = DateTime.Now,
                };

                db.Edata.Add(data);
                db.SaveChanges();


                ModelState.Clear();// Clear The Entered Data
            }

            return View();
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployee(EmployeeRegister reg)
        {
            using (EmployeeEntities1 db = new EmployeeEntities1())
            {
                db.Database.CommandTimeout = 300; // Set timeout to 5 minutes (300 seconds)

                var existingEmployee = db.Edata.FirstOrDefault(x => x.Id == reg.Id);

                if (existingEmployee == null)
                {
                    ViewBag.UpdateError = $"User does not exist with ID : {reg.Id}";
                    return View();
                }
                else
                {

                    existingEmployee.FirstName = reg.FirstName;
                    existingEmployee.LastName = reg.LastName;
                    existingEmployee.Email = reg.Email;   // Assuming you also want to update the email
                    existingEmployee.Password = reg.Password;
                    // You can update other fields as needed

                    ViewBag.UpdateSuccess = "User Updated Successfully";
                }

                db.SaveChanges();
            }

            return RedirectToAction("Register");


        }





        [HttpGet]
        public ActionResult Login()
        {

            return View();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(EmployeeRegister emp)
        {
            if (ModelState.IsValid)
            {
                return View(emp);
            }

            using (EmployeeEntities1 db = new EmployeeEntities1())
            {
                var user = db.Edata.FirstOrDefault(u => u.Email == emp.Email && u.Password == emp.Password);

                if (user != null)
                {
                    Session["ID"] = user.Id.ToString();
                    Session["Name"]=user.FirstName.ToString();
                    
                    Session["Email"] = user.Email.ToString();

                    TempData["Message"] = "Login successful!";
                    return RedirectToAction("User"); 
                }
                else
                {
                    // Use ModelState.AddModelError for business logic errors
                    ModelState.AddModelError("", "Invalid login attempt. Please check your username and password Or please Signup");
                    return View(emp);
                }
            }
        }




        public ActionResult User()
        {
            {
                if (Session["ID"] != null)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }



        }

        public ActionResult Delete(Guid id)
        {
            using (EmployeeEntities1 db = new EmployeeEntities1())
            {
                var employeeToDelete = db.Edata.FirstOrDefault(x => x.Id == id);

                if (employeeToDelete != null)
                {
                    db.Edata.Remove(employeeToDelete);
                    db.SaveChanges();

                    ViewBag.Message = "Employee deleted successfully";

                    // Redirect to the index action or another appropriate page
                    return RedirectToAction("Register");
                }
                else
                {

                    return HttpNotFound();
                }
            }
        }

    }
}
    











