using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using WebApplication_attw114.Models.PatrolFit.Response;

namespace WebApplication_attw114.Controllers
{
    public class OndutyFitController : Controller
    {
        public IActionResult Index()
        {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["LocationID"]))
            { 
                ViewBag.LocationID = HttpContext.Request.Query["LocationID"];
                Models.PatrolFit.Response.PointInfor pointInfor = new Dal.PatrolFit().GetPointInfor(HttpContext.Request.Query["LocationID"]);
                ViewBag.Name = pointInfor.Name;
                ViewBag.FullName = pointInfor.FullName;
                ViewBag.AreaID = pointInfor.AreaID;
                ViewBag.UrlImage = pointInfor.UrlImage;
            }
            return View();
        }
        public IActionResult OndutyFit()
        {
            return View();
        }
        public IActionResult Detail()
        {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["ID"]))
            {
                int id = Convert.ToInt32(HttpContext.Request.Query["ID"]);
                Models.PatrolFit.Response.RecordInfor recordInfor = new Dal.PatrolFit().GetRecordInforById(id);
                ViewBag.ID = id;
                ViewBag.EmpNo = recordInfor.EmpNo;
                ViewBag.EmpName = recordInfor.EmpName;
                ViewBag.AreaName = recordInfor.AreaName;
                ViewBag.FrameName = recordInfor.FrameName;
                ViewBag.AreaID = recordInfor.AreaID;
                ViewBag.TypePatrol = recordInfor.TypePatrol;
                ViewBag.UrlImage = recordInfor.UrlImage;
                if (recordInfor.TypePatrol == 1)
                {
                    ViewBag.TypePatrolName = "Biểu Tuần Tra Của Nhân Viên Bảo Vệ";
                }
                else
                {
                    ViewBag.TypePatrolName = "Biểu Tuần Tra Của Nhân Viên An Ninh";
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

    }
}
