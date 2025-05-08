using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication_attw114.Models;

namespace WebApplication_attw114.Controllers
{
    public class LoginController : Controller
    {
            [HttpGet]
            public IActionResult Index()
            {
                return View();
            }

            [HttpPost]
            public IActionResult Index(UserForLogin user)
            {
                Dal.UseLogin dal_uselogin = new Dal.UseLogin();
                string Message = string.Empty;
                if(user.EmpNo != null && dal_uselogin.CheckLogin(user.EmpNo,user.Password).Rows.Count > 0)
                {
                    return RedirectToAction("Index","Home");
                }
            
                return View();
            }
    }
}


