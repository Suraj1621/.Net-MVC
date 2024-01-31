using Register.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Register.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()

        {
            List<EmployeeRegister>model = new List<EmployeeRegister>();// Created Master Model.

            using (EmployeeEntities1 db = new EmployeeEntities1())
            {
                var users = db.Edata;
                foreach(var user in users)
                {
                    EmployeeRegister employeeRegister = new EmployeeRegister();
                    employeeRegister.Id = user.Id;
                    employeeRegister.FirstName = user.FirstName;
                    employeeRegister.LastName = user.LastName;
                    employeeRegister.Email = user.Email;
                    employeeRegister.CreatedOn=user.CreatedOn.ToString("dd-MM-yyyy");
                    employeeRegister.LastLoggedIn = user.LastLoggedOn?.ToString("dd-MM-yyyy") ?? ("NA");//Used QM For Nullable Object
                    model.Add(employeeRegister);
                }
            }
                return View(model);
        }
    }
}