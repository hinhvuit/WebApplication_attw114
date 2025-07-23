using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using WebApplication_attw114.Models.PatrolFit.DTO;
using WebApplication_attw114.Models.PatrolFit.Response;

namespace WebApplication_attw114.Controllers
{
    public class OndutyFitController : Controller
    {
        public RedirectResult Index()
        {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["LocationID"]))
            {
                string lat = HttpContext.Request.Query["LocationID"];
                string uria = @"https://attw.foxconn.com.vn/Ondutyfit.html?Loca=" + lat;
                string lataaa=lat.ToLower();
                if (lataaa.Contains("qci") || lataaa.Contains("qcj"))
                {
                    uria = @"https://attw.foxconn.com.vn/Ondutyfit.html?Loca=" + lat + "&type=atvn";
                }
                return Redirect(uria);
            }
            else
            {
                return Redirect("#");
            }
        }
        public IActionResult OndutyFit()
        {
            return View();
        }
        public IActionResult ErrorLocation()
        {
            ViewBag.Error = "1";
            ViewBag.Message = "Vui lòng chọn vị trí tuần tra trước khi thực hiện thao tác này.";
            return View();
        }
        public IActionResult ErorrSign()
        {
            ViewBag.Error = "1";
            ViewBag.Message = "Vui lòng chọn vị trí tuần tra trước khi thực hiện thao tác này.";
            return View();
        }
        public IActionResult Tuantra()
        {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["LocationID"]) && !String.IsNullOrEmpty(HttpContext.Request.Query["lati"]) && !String.IsNullOrEmpty(HttpContext.Request.Query["longti"]))
            {
                ViewBag.LocationID = HttpContext.Request.Query["LocationID"];
                PointInfor pointInfor = new Dal.PatrolFit().GetPointInfor(HttpContext.Request.Query["LocationID"]);
                ViewBag.Name = pointInfor.Name;
                ViewBag.FullName = pointInfor.FullName;
                ViewBag.AreaID = pointInfor.AreaID;
                ViewBag.UrlImage = pointInfor.UrlImage;
                ViewBag.LongtiNew = HttpContext.Request.Query["longti"];
                ViewBag.LatiNew = HttpContext.Request.Query["lati"];

                if (pointInfor.Lati != null && pointInfor.Lati != 0)
                {
                    ViewBag.Lati = pointInfor.Lati;
                }
                else
                {
                    ViewBag.Lati = "0";
                }
                if (pointInfor.Longti != null && pointInfor.Longti != 0)
                {
                    ViewBag.Longti = pointInfor.Longti;
                }
                else
                {
                    ViewBag.Longti = "0";
                }
                if (pointInfor.Lati != null && pointInfor.Lati != 0 && pointInfor.Longti != null && pointInfor.Longti != 0)
                {
                    Helper.GeoHelper geoHelper = new Helper.GeoHelper();
                    double lati = Convert.ToDouble(pointInfor.Lati);
                    double longti = Convert.ToDouble(pointInfor.Longti);
                    double latiNew = Convert.ToDouble(HttpContext.Request.Query["lati"]);
                    double longtiNew = Convert.ToDouble(HttpContext.Request.Query["longti"]);
                    double distance = geoHelper.CalculateDistance(lati, longti, latiNew, longtiNew);
                    if (distance > 150)
                    {
                        ViewBag.Distance = "Vượt quá phạm vi tuần tra, bạn không thể tuần tra tại đây";
                        ViewBag.Error = "1";
                    }
                    else
                    {
                        ViewBag.Distance = "Khoảng cách từ vị trí tuần tra đến điểm tuần tra là: " + distance.ToString("0.00") + "m";
                        ViewBag.Error = "0";
                    }
                }
                else
                {
                    ViewBag.Distance = "";
                    ViewBag.Error = "";

                }
            }
            else
            {
                ViewBag.LocationID = "0";
            }
            return View();
        }
        public IActionResult Securety()
        {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["LocationID"]) && !String.IsNullOrEmpty(HttpContext.Request.Query["lati"]) && !String.IsNullOrEmpty(HttpContext.Request.Query["longti"])&& !String.IsNullOrEmpty(HttpContext.Request.Query["userid"]))
            {
                ViewBag.LocationID = HttpContext.Request.Query["LocationID"];
                PointInfor pointInfor = new Dal.PatrolFit().GetPointInfor(HttpContext.Request.Query["LocationID"]);
                PatrolSignInforDTO signInforDTO = new Dal.PatrolFit().GetInforByUserID(HttpContext.Request.Query["userid"]);
                ViewBag.Name = pointInfor.Name;
                ViewBag.FullName = pointInfor.FullName;
                ViewBag.AreaID = pointInfor.AreaID;
                ViewBag.UrlImage = pointInfor.UrlImage;
                ViewBag.LongtiNew = HttpContext.Request.Query["longti"];
                ViewBag.LatiNew = HttpContext.Request.Query["lati"];
                ViewBag.UserID = HttpContext.Request.Query["userid"];
                if (signInforDTO.EmpNo != null && signInforDTO.EmpName != null)
                {
                    ViewBag.EmpNo = signInforDTO.EmpNo;
                    ViewBag.EmpName = signInforDTO.EmpName;
                }
                else
                {
                    ViewBag.EmpNo = "Na";
                    ViewBag.EmpName = "Na";
                }
                if (pointInfor.Lati != null && pointInfor.Lati != 0)
                {
                    ViewBag.Lati = pointInfor.Lati;
                }
                else
                {
                    ViewBag.Lati = "0";
                }
                if (pointInfor.Longti != null && pointInfor.Longti != 0)
                {
                    ViewBag.Longti = pointInfor.Longti;
                }
                else
                {
                    ViewBag.Longti = "0";
                }
                if (pointInfor.Lati != null && pointInfor.Lati != 0 && pointInfor.Longti != null && pointInfor.Longti != 0)
                {
                    Helper.GeoHelper geoHelper = new Helper.GeoHelper();
                    double lati = Convert.ToDouble(pointInfor.Lati);
                    double longti = Convert.ToDouble(pointInfor.Longti);
                    double latiNew = Convert.ToDouble(HttpContext.Request.Query["lati"]);
                    double longtiNew = Convert.ToDouble(HttpContext.Request.Query["longti"]);
                    double distance = geoHelper.CalculateDistance(lati, longti, latiNew, longtiNew);
                    if (distance > 150)
                    {
                        ViewBag.Distance = "Vượt quá phạm vi tuần tra, bạn không thể tuần tra tại đây";
                        ViewBag.Error = "1";
                    }
                    else
                    {
                        ViewBag.Distance = "Khoảng cách từ vị trí tuần tra đến điểm tuần tra là: " + distance.ToString("0.00") + "m";
                        ViewBag.Error = "0";
                    }
                }
                else
                {
                    ViewBag.Distance = "";
                    ViewBag.Error = "";

                }
            }
            else
            {
                ViewBag.LocationID = "0";
            }
            return View();
        }
        public IActionResult Detail()
        {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["ID"]))
            {
                int id = Convert.ToInt32(HttpContext.Request.Query["ID"]);
                RecordInfor recordInfor = new Dal.PatrolFit().GetRecordInforById(id);
                ViewBag.ID = id;
                ViewBag.EmpNo = recordInfor.EmpNo;
                ViewBag.EmpName = recordInfor.EmpName;
                ViewBag.AreaName = recordInfor.AreaName;
                ViewBag.FrameName = recordInfor.FrameName;
                ViewBag.AreaID = recordInfor.AreaID;
                ViewBag.TypePatrol = recordInfor.TypePatrol;
                ViewBag.UrlImage = recordInfor.UrlImage;
                if (recordInfor.TypePatrol == 2)
                {
                    ViewBag.TypePatrolName = "Biểu Tuần Tra Của Nhân Viên An Ninh";
                }
                else
                {
                    ViewBag.TypePatrolName = "Biểu Tuần Tra Của Nhân Viên Bảo Vệ";
                }
                    ViewBag.DatePatrol = recordInfor.DatePatrol.ToString("yyyy-MM-dd HH:mm:ss");
                ViewData["ListPoint"] = new Dal.PatrolFit().PatrolRecord_GetListChecked(id);
            }
            return View();
        }
        public IActionResult Point(int idrecord,int pointid)
        {
            var view_model = new Dal.PatrolFit().GetPointDetailViewModel(idrecord, pointid);
            return PartialView("_PointDetailPartial", view_model);
        }
        [HttpGet]
        public JsonResult getrulechecked(int idrecord, int pointid)
        {
            var result = new Dal.PatrolFit().GetPointDetailViewModel(idrecord, pointid);
            return Json(result);
        }
        [HttpPost]
        public IActionResult GetList([FromBody] Models.PatrolFit.DTO.PatrolGetNowDTO nowDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var patrol = new Dal.PatrolFit();
            var result = patrol.PatrolPoint_GetHistory(nowDTO);
            return Ok(result); // hoặc return Json(result);

        }
        [HttpGet]
        public IActionResult GetRules(string codePoint,int typePatrol)
        {
            if (string.IsNullOrEmpty(codePoint))
            {
                return BadRequest("CodePoint is required.");
            }
            var patrol = new Dal.PatrolFit();
            var result = patrol.GetRuleInfors(codePoint,typePatrol);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult SignPatrol([FromBody] Models.PatrolFit.DTO.PatrolSignListDTO saveDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var patrol = new Dal.PatrolFit();
            Models.PatrolFit.DTO.PatrolSignDTO signDTO = new Models.PatrolFit.DTO.PatrolSignDTO();
            signDTO.TypePatrol = saveDTO.TypePatrol;
            signDTO.code_Point = saveDTO.CodePoint;
            signDTO.EmpNo = saveDTO.EmpNo;
            signDTO.EmpName = saveDTO.EmpName;
            signDTO.Lati = saveDTO.Lati;
            signDTO.Longti = saveDTO.Longti;
            int idapp=patrol.PatrolSign(signDTO);
            foreach (var rule in saveDTO.ListChecked)
            {
                Models.PatrolFit.DTO.PatrolCheckedDTO checkedDTO = new Models.PatrolFit.DTO.PatrolCheckedDTO();
                checkedDTO.PointCheckedID = idapp;
                checkedDTO.RuleID = rule.RuleID;
                checkedDTO.ImageName = rule.ImageName;
                checkedDTO.Memo = rule.Memo;
                checkedDTO.IsOk = rule.IsOk;
                patrol.Add_CheckedRule(checkedDTO);
            }
            return Ok(new { Message = "Rules processed successfully" });
        }
        [HttpPost]
        public IActionResult UpdateLocation([FromBody] Models.PatrolFit.DTO.PointUpdateDTO updateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patrol = new Dal.PatrolFit();
            patrol.Update_PatrolPointlatilongti(updateDTO);

            return Ok(new { Message = "Location updated successfully" });
        }


    }
}
