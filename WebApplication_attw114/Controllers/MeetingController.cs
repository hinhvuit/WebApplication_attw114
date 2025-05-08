using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication_attw114.Models;
using System.Web;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Text;
using Nancy.Json;
using Newtonsoft.Json;
using static PDSS.CivetPublic.User.Subscriber.SearchResult;

namespace WebApplication_attw114.Controllers
{
    public class MeetingController : Controller
    {
        private readonly IWebHostEnvironment _env;
        Dal.MeetingCivet dal_Meeting = new Dal.MeetingCivet();
        public MeetingController(IWebHostEnvironment env)
        {
            _env = env;
        }
        public IActionResult Index()
        {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["MeetID"]))
            { 
                int id=Convert.ToInt32(HttpContext.Request.Query["MeetID"].ToString().Trim());
                Models.MeetingCivet model = new MeetingCivet();
                Dal.MeetingCivet DMet= new Dal.MeetingCivet();
                model=DMet.GetModelSigle(id);
                if (model != null)
                {
                    ViewBag.Id = id;
                    ViewBag.Name = model.Name;
                    ViewBag.EndTime = model.EndTime;
                    ViewBag.QRImage = @"http://10.224.24.30/Upload/Qrcode/"+model.QRImage;
                    ViewBag.TimeStart = model.TimeStart + " - ";
                    ViewBag.Type = 1;
                }
            }
                return View();
        }
        public IActionResult Sign() {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["MeetID"])&& !String.IsNullOrEmpty(HttpContext.Request.Query["TypeMeet"]))
            {
                int id = Convert.ToInt32(HttpContext.Request.Query["MeetID"].ToString().Trim());
                int type= Convert.ToInt32(HttpContext.Request.Query["TypeMeet"].ToString().Trim());
                Models.MeetingCivet model = new MeetingCivet();
                Dal.MeetingCivet DMet = new Dal.MeetingCivet();
                model = DMet.GetModelSigle(id);
                int checka=DMet.CheckTime(id, type);
                if (checka == 1)
                {
                    if (model != null)
                    {
                        ViewBag.Id = id;
                        ViewBag.Name = model.Name;
                        ViewBag.TimeStart = model.TimeStart;
                        ViewBag.Date = checka;
                        ViewBag.Type = type;
                    }

                    return View();
                }
                else {

                    return View("DateEnd");
                }
            }
            else
            {
                return View("DateEnd");
            }
        }
        public IActionResult Signcivet()
        {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["MeetID"]) && !String.IsNullOrEmpty(HttpContext.Request.Query["TypeMeet"]) && !String.IsNullOrEmpty(HttpContext.Request.Query["empno"]) && !String.IsNullOrEmpty(HttpContext.Request.Query["empname"]))
            {
                int id = Convert.ToInt32(HttpContext.Request.Query["MeetID"].ToString().Trim());
                int type = Convert.ToInt32(HttpContext.Request.Query["TypeMeet"].ToString().Trim());
                string emp_no = HttpContext.Request.Query["empno"].ToString().Trim();
                string emp_name= HttpContext.Request.Query["empname"].ToString().Trim();
                string bg= HttpContext.Request.Query["bg"].ToString().Trim();
                DataTable dt = dal_Meeting.UP_CivetMeeting_Getinfor(emp_no);
                if(dt.Rows.Count > 0)
                {
                    emp_name = dt.Rows[0]["Emp_Name"].ToString();
                    bg = dt.Rows[0]["Dept"].ToString();
                    Models.CivetNo civetNo = new Models.CivetNo();
                    civetNo.emp_name = emp_name;
                    civetNo.emp_no= emp_no;
                    civetNo.dept = dt.Rows[0]["Dept"].ToString();
                }
                Models.MeetingCivet model = new MeetingCivet();
                Dal.MeetingCivet DMet = new Dal.MeetingCivet();
                model = DMet.GetModelSigle(id);
                int checka = DMet.CheckTime(id, type);
                if (checka == 1)
                {
                    if (model != null)
                    {
                        ViewBag.Id = id;
                        ViewBag.Name = model.Name;
                        ViewBag.TimeStart = model.TimeStart;
                        ViewBag.Date = checka;
                        ViewBag.Type = type;
                        ViewBag.Emp_No = emp_no;
                        ViewBag.Emp_Name = emp_name;
                        ViewBag.BG = bg;
                    }

                    return View();
                }
                else
                {

                    return View("DateEnd");
                }
            }
            else
            {
                return View("DateEnd");
            }
        }
        [HttpPost]
        public ActionResult MeetSign(int ID, int Type, string EmpNo, string EmpName, string BGName, string ExtInfor)
        {
            try
            {
                DataTable dt = dal_Meeting.UP_CivetMeeting_Getinfor(EmpNo);

                UserCivetLoginRecord model = new UserCivetLoginRecord();
                model.ID = ID;
                model.Type = Type;
                model.EmpNo = EmpNo;
                model.EmpName = EmpName;
                if (BGName != "" && BGName != null)
                {
                    model.BGName = BGName;
                }
                else
                {
                    if (dt.Rows.Count > 0)
                    {
                        model.BGName = dt.Rows[0]["Dept"].ToString();
                    }
                }
                model.ExtInfor = ExtInfor;
                dal_Meeting.MeetSign(model);

                return Content("Success");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        [HttpPost]
        public string UserCivetLoginRecord_GetList([FromBody] ModelMeet model)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = dal_Meeting.UserCivetLoginRecord_GetList(model.ID, model.Type);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return JsonConvert.SerializeObject(dt);
        }
        [HttpPost]
        public string UserCivetLoginRecord_GetAllCount([FromBody] ModelMeet model)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = dal_Meeting.UserCivetLoginRecord_GetAllCount(model.ID, model.Type);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return JsonConvert.SerializeObject(dt);
        }
    }
}
