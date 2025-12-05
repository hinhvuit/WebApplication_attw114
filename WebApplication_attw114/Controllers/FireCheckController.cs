using Microsoft.AspNetCore.Mvc;
using System;
using WebApplication_attw114.Models;
using WebApplication_attw114.Models.PatrolFit.Response;

namespace WebApplication_attw114.Controllers
{
    public class FireCheckController : Controller
    {
        /// <summary>
        /// index page for fire check
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]) && !String.IsNullOrEmpty(HttpContext.Request.Query["dvid"]))
            {
                ViewBag.userId = HttpContext.Request.Query["id"];
                ViewBag.codeDevice = HttpContext.Request.Query["dvid"];
                Dal.FireCheck fireCheck = new Dal.FireCheck();
                Devices devices = fireCheck.GetDevices(HttpContext.Request.Query["dvid"]);
                FcUser fcUser=fireCheck.GetUser(int.Parse(HttpContext.Request.Query["id"]));
                ViewBag.factoryName = devices.FactoryName;
                ViewBag.zoneName = devices.ZoneName;
                ViewBag.typeName = devices.TypeName;
                ViewBag.location = devices.Location;
                ViewBag.type = devices.Type;
                ViewBag.EmpNo = fcUser.EmpNo;
                ViewBag.EmpName = fcUser.EmpName;
                ViewBag.deviceId = devices.Id;
            }
            return View();
        }
        /// <summary>
        /// api to get rule device information by code
        /// </summary>
        /// <param name="codeDevice"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetRules(string codeDevice)
        {
            try
            {
                if (string.IsNullOrEmpty(codeDevice))
                {
                    return BadRequest("CodePoint is required.");
                }
                var firecheck = new Dal.FireCheck();
                var result = firecheck.GetlistRule(codeDevice);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Ghi log để kiểm tra lỗi chi tiết
                Console.WriteLine("Lỗi khi lấy rule: " + ex.ToString());
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        /// <summary>
        /// api to save checked list
        /// </summary>
        /// <param name="devicesSign"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveCheckedList([FromBody] DevicesSign devicesSign)
        {
            if (devicesSign == null || devicesSign.UserID <= 0 || devicesSign.DeviceID <= 0 || devicesSign.ListChecked == null)
            {
                return BadRequest(new { success = false, message = "Invalid data." });
            }

            Dal.FireCheck fireCheck = new Dal.FireCheck();
            try
            {
                Models.CheckedList checkedList = new Models.CheckedList
                {
                    UserID = devicesSign.UserID,
                    DeviceID = devicesSign.DeviceID,
                    Memo = devicesSign.Memo
                };

                int appid = fireCheck.FireCheckSign(checkedList);

                foreach (var rule in devicesSign.ListChecked)
                {
                    RuleCheckedList checkedDTO = new RuleCheckedList
                    {
                        IdChecked = appid,
                        RuleID = rule.RuleID,
                        Memo = rule.Memo,
                        IsOk = rule.IsOk
                    };

                    fireCheck.Add_CheckedRule(checkedDTO);
                }

                return Ok(new { success = true, message = "Data saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Internal server error: {ex.Message}" });
            }
        }

    }
}
