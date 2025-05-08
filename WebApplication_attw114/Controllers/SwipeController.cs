using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utility;
using WebApplication_attw114.Dal;
using WebApplication_attw114.Models;

namespace WebApplication_attw114.Controllers
{
    public class SwipeController : Controller
    {
        public Models.Model_IcvetOut McivetOut;
        Models.UserMember MUser = new Models.UserMember();
        Dal.UserInfor DUser = new Dal.UserInfor();
        Dal.UserRecord DRecord = new Dal.UserRecord();
        Models.UserRecord MRecord = new Models.UserRecord();
        Dal.UserFromECard DECard = new Dal.UserFromECard();
        Models.UserFromECard MECard = new Models.UserFromECard();
        SophyEncrypt SE = new SophyEncrypt();
        public RedirectResult Onlocal()
        {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["code"]) && !String.IsNullOrEmpty(HttpContext.Request.Query["lati"]) && !String.IsNullOrEmpty(HttpContext.Request.Query["longti"]))
            {
                string code = HttpContext.Request.Query["code"].ToString();
                string lat = HttpContext.Request.Query["lati"];
                string lot = HttpContext.Request.Query["longti"];
                _ = BindDataAsync(code);
                string uria = @"http://114.foxconn.com.vn:8186/SwipesRegister?code=" + code + "&lati=" + lat + "&longti=" + lot + "&empno=" + McivetOut.civetno + "&empname=" + McivetOut.realname + "&bgname=" + McivetOut.bg;
                return Redirect(uria);
            }
            else
            {
                return Redirect("#");
            }
        }
        public RedirectResult Onduty()
        {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["code"]) && !String.IsNullOrEmpty(HttpContext.Request.Query["Loca"]))
            {
                string code = HttpContext.Request.Query["code"].ToString();
                string lat = HttpContext.Request.Query["Loca"];
                _ = BindDataAsync(code);
                string uria = @"http://114.foxconn.com.vn:8088/OndutySignCivet.aspx?code=" + code + "&Loca=" + lat + "&empno=" + McivetOut.civetno + "&empname=" + McivetOut.realname + "&sty=2" + "&bgname=" + McivetOut.bg;
                return Redirect(uria);
            }
            else
            {
                return Redirect("#");
            }
        }
        public RedirectResult Approve()
        {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["code"]))
            {
                string code = HttpContext.Request.Query["code"].ToString();
                string lat = HttpContext.Request.Query["Loca"];
                string aass = "Smart";
                _ = BindDataAsync(code);
                string Emp = SE.Add(McivetOut.civetno.Trim());
                //string uria = @"http://114.foxconn.com.vn/OndutySign.aspx?code=" + code + "&Loca=" + lat + "&empno=" + McivetOut.civetno + "&empname=" + McivetOut.realname + "&sty=2" + "&bgname=" + McivetOut.bg;
                string Urlto = "http://114.foxconn.com.vn/Zhengjian/WaitListAppr.aspx?Paracode=" + Emp + "&uid=" + SE.Add(aass);
                return Redirect(Urlto);
            }
            else
            {
                return Redirect("#");
            }
        }
        public RedirectResult SafeCheck()
        {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["code"]) && !String.IsNullOrEmpty(HttpContext.Request.Query["LocationID"]))
            {
                string code = HttpContext.Request.Query["code"].ToString();
                string lat = HttpContext.Request.Query["lati"];
                string lot = HttpContext.Request.Query["longti"];
                string locatiodid = HttpContext.Request.Query["LocationID"].ToString();
                _ = BindDataAsync(code);
                string uria = @"http://114.foxconn.com.vn/Zhengjian/SafetycheckingLocation.aspx?code=" + code + "&lati=" + lat + "&longti=" + lot + "&empno=" + McivetOut.civetno + "&empname=" + McivetOut.realname+ "&sty=3&LocationID="+locatiodid;
                return Redirect(uria);
            }
            else
            {
                return Redirect("#");
            }
        }
        public RedirectResult SafeCheckOld()
        {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["code"]))
            {
                string code = HttpContext.Request.Query["code"].ToString();
                _ = BindDataAsync(code);
                string uria = @"http://114.foxconn.com.vn/Zhengjian/Safetychecking.aspx?code=" + code + "&empno=" + McivetOut.civetno + "&empname=" + McivetOut.realname + "&sty=4";
                return Redirect(uria);
            }
            else
            {
                return Redirect("#");
            }
        }
        public IActionResult Index()
        {
            string LocationID = "CGQV01";
            Model_IcvetOut model_Icvet = new Model_IcvetOut();
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["code"]) && !String.IsNullOrEmpty(HttpContext.Request.Query["lati"]) && !String.IsNullOrEmpty(HttpContext.Request.Query["longti"]))
            {
                string code = HttpContext.Request.Query["code"].ToString();
                string lat = HttpContext.Request.Query["lati"];
                string lot = HttpContext.Request.Query["longti"];
                //model_Icvet = PopulateGetUserCvet(code);

                _ = BindDataAsync(code);
                if (DUser.CheckExists(McivetOut.civetno) == 0)
                {

                }
                else
                {
                    DataSet ds;
                    ds = DRecord.GetListAreaDetail(1);
                    MUser = DUser.GetModel(McivetOut.civetno);
                    MRecord.EmpNo = MUser.EmpNo;
                    MRecord.EmpName = MUser.EmpName;
                    string loname = "N/A";
                    int areaid = 0;
                    double lati = 0, longti = 0;
                    if (!string.IsNullOrEmpty(lat) && !string.IsNullOrEmpty(lot))
                    {
                        lati = Convert.ToDouble(lat);
                        longti = Convert.ToDouble(lot);
                    }
                    else
                    {
                        lati = 0;
                        longti = 0;
                    }
                    int apid = 0;
                    DataSet dscheck;
                    dscheck = DRecord.CheckAreaByLaLong((float)lati, (float)longti);
                    if (dscheck.Tables[0].Rows.Count > 0)
                    {
                        loname = dscheck.Tables[0].Rows[0]["LocationName"].ToString();
                        areaid = Convert.ToInt32(dscheck.Tables[0].Rows[0]["AreaID"].ToString().Trim());
                    }
                    MRecord.Location = loname;
                    MRecord.Latitude = (float)lati;
                    MRecord.Longitude = (float)longti;
                    int icheck = DECard.CheckLocationECard(McivetOut.civetno.Trim(), areaid);
                    if (icheck == 0)
                    {
                        MRecord.Type = 1;
                    }
                    else
                    {
                        if (icheck == -2)
                        {
                            MRecord.Type = 4;
                        }
                        else
                        {
                            MRecord.Type = 3;
                        }
                    }
                    apid = DRecord.Add(MRecord);
                    Models.ParaLocation prads= new Models.ParaLocation();
                    prads.code = code;
                    prads.appid=apid.ToString();
                    return RedirectToAction("SwipeCard", "Swipe", prads);
                }
            }
            return View();
        }
        public IActionResult SwipeCard()
        {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["appid"]))
            {
                int id = Convert.ToInt32(HttpContext.Request.Query["appid"]);
                MRecord = DRecord.GetModelByID(id);
                ViewData["Record"] = MRecord;
                ViewData["AreaPower"] = DRecord.GetAreaPowers(MRecord.EmpNo);
                ViewBag.EmpNo=MRecord.EmpNo;
                ViewBag.EmpName=MRecord.EmpName;
                ViewBag.Lacation = MRecord.Location;
                if (MRecord.Avarta.Contains("swipe")) {
                    ViewBag.Avartar=@"~/upload/avarta/"+MRecord.Avarta;
                }
                else
                {
                    ViewBag.Avartar = MRecord.Avarta;
                }
                ViewBag.Time = MRecord.TimeRecord.ToString("yyyy-MM-dd HH:mm:ss");

                return View();
            }
            else
            {
                return View("Error");
            }
        }
        public IActionResult Sign() { 
            return View(); 
        }
        public IActionResult Detail()
        {
            return View();
        }
        public IActionResult UpdateInfor()
        {
            return View();
        }
        private Models.Model_IcvetOut PopulateGetUserCvet(string Strcode)
        {
            string apiUrl = "http://civetInterface.foxconn.com/open/get_user_info_bycode";
            object input = new
            {
                code = Strcode,
                appid = "A36PAwlql7vvuGDueqKSMw2"
            };
            string inputJson = (new JavaScriptSerializer()).Serialize(input);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string json = client.UploadString(apiUrl, inputJson);
            Models.Model_IcvetOut model1 = new Models.Model_IcvetOut();
            model1 = (new JavaScriptSerializer()).Deserialize<Models.Model_IcvetOut>(json);
            return model1;
        }

        public async Task BindDataAsync(string code)
        {
            string URL = @"http://civetInterface.foxconn.com/open/get_user_info_bycode";
            string urlParameters = "?code=" + code + "&appid=A36PAwlql7vvuGDueqKSMw2";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            dynamic jssdado;
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                McivetOut = (new JavaScriptSerializer()).Deserialize<Models.Model_IcvetOut>(result);
            }
        }
    }
}
