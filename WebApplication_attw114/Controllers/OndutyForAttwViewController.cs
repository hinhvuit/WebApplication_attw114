using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using WebApplication_attw114.Models;
using Utility;

namespace WebApplication_attw114.Controllers
{
    public class OndutyForAttwViewController : Controller
    {
        SophyEncrypt se = new SophyEncrypt();
        
        private readonly IWebHostEnvironment _env;
        Dal.OndutyForAttwSign dal_OndutySign = new Dal.OndutyForAttwSign();
        public OndutyForAttwViewController(IWebHostEnvironment env)
        {
            _env = env;
        }
        public IActionResult Index()
        {
            List<Testmodel> listtest = new List<Testmodel>();

            /*if (Request.QueryString["code"] != null)
            {
                string Strcode = Request.QueryString["code"].ToString().Trim();
            }*/

            //OndutyAttwIDint OndutyAttwID = 9;

            
            List<Model_HistorySign> listModelHisSign = new List<Model_HistorySign>();

            int OndutyAttwID = Convert.ToInt32(HttpContext.Request.Query["OndutySignAttwID"].ToString());

            DataTable dt = dal_OndutySign.getInforFormByOndutySignID(OndutyAttwID);

                ViewBag.EmpNoFromIc = dt.Rows[0]["EmpNo"].ToString();
                ViewBag.testNameFromIc = dt.Rows[0]["EmpName"].ToString();
                ViewBag.AreaName = dt.Rows[0]["AreaName"].ToString();
                ViewBag.AreaID = dt.Rows[0]["AreaID"].ToString();
                ViewBag.DateSign = dt.Rows[0]["DateSing"].ToString();
                ViewBag.FrameTimeName = dt.Rows[0]["FrameTimeName"].ToString();
                    

                if (dt.Rows[0]["Type"].ToString() == "1")
                {
                    ViewBag.Type = 1;
                    ViewBag.Title = "Biểu tuần tra ký duyệt của nhân viên cảnh giới ATTW";
                    
                }
                else
                {
                    ViewBag.Type = 2;
                    ViewBag.Title = "Biểu tuần tra ký duyệt của nhân viên phòng cháy ATTW";
                    
                }
                listModelHisSign = dal_OndutySign.getHistorySignByOndutyAttwID(OndutyAttwID);
                ViewData["ListHistorySign"] = listModelHisSign;




            for (int i = 0; i < 6; i++)
            {
                Testmodel model = new Testmodel();
                model.ID = i;
                model.Name = "name" + i.ToString();
                listtest.Add(model);
            }
            ViewData["testViewdata"] = listtest;


            //ViewData["ListRule"] = dal_OndutySign.GetListRulesByLocaIDType("CGQV01", 1);
            return View();
        }
    }
}
