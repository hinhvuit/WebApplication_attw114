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
using System.Reflection.Metadata;
using System.Web.Helpers;
using Nancy;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Utility;

namespace WebApplication_attw114.Controllers
{

    public class OndutySignEmpAttw114Controller : Controller
    {
        private readonly IWebHostEnvironment _env;
        Dal.OndutyForAttwSign dal_OndutySign = new Dal.OndutyForAttwSign();
        public Models.Model_IcvetOut McivetOut;
        public OndutySignEmpAttw114Controller(IWebHostEnvironment env)
        {
            _env = env;
        }
        public RedirectResult Index()
        {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["code"]) && !String.IsNullOrEmpty(HttpContext.Request.Query["LocationID"]))
            {
                string code = HttpContext.Request.Query["code"].ToString();
                string lat = HttpContext.Request.Query["LocationID"];
                _ = BindDataAsync(code);
                string uria = @"https://attw.foxconn.com.vn/attwemponduty.html?code=" + code + "&Loca=" + lat;
                return Redirect(uria);
            }
            else
            {
                return Redirect("#");
            }
        }
        public IActionResult Onduty()
        {
            List<Testmodel> listtest = new List<Testmodel>();

            /*if (Request.QueryString["code"] != null)
            {
                string Strcode = Request.QueryString["code"].ToString().Trim();
            }*/

            string LocationID = "CGQV01";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["LocationID"]))
            {
                LocationID = HttpContext.Request.Query["LocationID"].ToString();

                DataTable dt = dal_OndutySign.getInforFormByLocationID(LocationID);
                ViewBag.AreaName = dt.Rows[0]["AreaName"].ToString();
                ViewBag.AreaID = dt.Rows[0]["AreaID"].ToString();
                if (dt.Rows[0]["latitude"] != null)
                {
                    ViewBag.latitude = dt.Rows[0]["latitude"].ToString();
                    ViewBag.longitude = dt.Rows[0]["longitude"].ToString();

                }
                else
                {
                    ViewBag.latitude = "0";
                    ViewBag.longitude = "0";
                }
                if (dt.Rows[0]["Type"].ToString() == "1")
                {
                    ViewBag.Type = 1;
                    ViewBag.Title = "Biểu tuần tra ký duyệt của nhân viên cảnh giới ATTW";
                    ViewData["ListRule"] = dal_OndutySign.GetListRulesByLocaIDType(LocationID, 1);
                }
                else
                {
                    ViewBag.Type = 2;
                    ViewBag.Title = "Biểu tuần tra ký duyệt của nhân viên phòng cháy ATTW";
                    ViewData["ListRule"] = dal_OndutySign.GetListRulesByLocaIDType(LocationID, 2);
                }
                ViewBag.LocationDetail = dt.Rows[0]["LocationDetailName"].ToString();

            }
            else
            {
                ViewData["ListRule"] = dal_OndutySign.GetListRulesByLocaIDType(LocationID, 1);
                DataTable dt = dal_OndutySign.getInforFormByLocationID(LocationID);
                ViewBag.AreaName = dt.Rows[0]["AreaName"].ToString();
                ViewBag.AreaID = dt.Rows[0]["AreaID"].ToString();
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
                ViewBag.LocationDetail = dt.Rows[0]["LocationDetailName"].ToString();
            }
            ViewBag.LocaiontID = LocationID;
            Model_IcvetOut model_Icvet = new Model_IcvetOut();

            List<Model_HistorySign> listModelHisSign = new List<Model_HistorySign>();
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["code"]) && !String.IsNullOrEmpty(HttpContext.Request.Query["empno"]) && !String.IsNullOrEmpty(HttpContext.Request.Query["empname"]))
            {
                string code = HttpContext.Request.Query["code"].ToString();
                string empno = HttpContext.Request.Query["empno"].ToString();
                string empname = HttpContext.Request.Query["empname"].ToString();
                ViewBag.testStr = HttpContext.Request.Query["code"].ToString();
                //_ = BindDataAsync(code);
                //model_Icvet = PopulateGetUserCvet(code);
                ViewBag.testNameFromIc = empname;
                ViewBag.EmpNoFromIc = empno;
                listModelHisSign = dal_OndutySign.getHistorySignByLocationIDNow(LocationID, empno);
                ViewData["ListHistorySign"] = listModelHisSign;
            }
            else
            {
                ViewBag.testNameFromIc = "Đinh Đức Mạnh";
                ViewBag.EmpNoFromIc = "V0510589";
                ViewBag.testStr = "this is data of ViewBag";
                listModelHisSign = dal_OndutySign.getHistorySignByLocationIDNow(LocationID, "V0510589");
                ViewData["ListHistorySign"] = listModelHisSign;
            }


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
        /* comment old code
        public IActionResult Index()
        {
            List<Testmodel> listtest = new List<Testmodel>();

            

            string LocationID = "CGQV01";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["LocationID"]))
            {
                LocationID = HttpContext.Request.Query["LocationID"].ToString();

                DataTable dt = dal_OndutySign.getInforFormByLocationID(LocationID);
                ViewBag.AreaName = dt.Rows[0]["AreaName"].ToString();
                ViewBag.AreaID = dt.Rows[0]["AreaID"].ToString();
                if (dt.Rows[0]["latitude"] != null)
                {
                    ViewBag.latitude = dt.Rows[0]["latitude"].ToString();
                    ViewBag.longitude = dt.Rows[0]["longitude"].ToString();

                }
                else
                {
                    ViewBag.latitude = "0";
                    ViewBag.longitude = "0";
                }
                if (dt.Rows[0]["Type"].ToString() == "1")
                {
                    ViewBag.Type = 1;
                    ViewBag.Title = "Biểu tuần tra ký duyệt của nhân viên cảnh giới ATTW";
                    ViewData["ListRule"] = dal_OndutySign.GetListRulesByLocaIDType(LocationID, 1);
                }
                else
                {
                    ViewBag.Type = 2;
                    ViewBag.Title = "Biểu tuần tra ký duyệt của nhân viên phòng cháy ATTW";
                    ViewData["ListRule"] = dal_OndutySign.GetListRulesByLocaIDType(LocationID, 2);
                }
                ViewBag.LocationDetail = dt.Rows[0]["LocationDetailName"].ToString();
                
            }
            else
            {
                ViewData["ListRule"] = dal_OndutySign.GetListRulesByLocaIDType(LocationID, 1);
                DataTable dt = dal_OndutySign.getInforFormByLocationID(LocationID);
                ViewBag.AreaName = dt.Rows[0]["AreaName"].ToString();
                ViewBag.AreaID = dt.Rows[0]["AreaID"].ToString();
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
                ViewBag.LocationDetail = dt.Rows[0]["LocationDetailName"].ToString();
            }
            ViewBag.LocaiontID = LocationID;
            Model_IcvetOut model_Icvet = new Model_IcvetOut();

            List<Model_HistorySign> listModelHisSign = new List<Model_HistorySign>();
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["code"]))
            {
                string code = HttpContext.Request.Query["code"].ToString();
                ViewBag.testStr = HttpContext.Request.Query["code"].ToString();
                _ = BindDataAsync(code);
                //model_Icvet = PopulateGetUserCvet(code);
                ViewBag.testNameFromIc = McivetOut.realname;
                ViewBag.EmpNoFromIc = McivetOut.civetno;
                listModelHisSign = dal_OndutySign.getHistorySignByLocationIDNow(LocationID, McivetOut.civetno);
                ViewData["ListHistorySign"] = listModelHisSign;
            }
            else
            {
                ViewBag.testNameFromIc = "Đinh Đức Mạnh";
                ViewBag.EmpNoFromIc = "V0510589";
                ViewBag.testStr = "this is data of ViewBag";
                listModelHisSign = dal_OndutySign.getHistorySignByLocationIDNow(LocationID, "V0510589");
                ViewData["ListHistorySign"] = listModelHisSign;
            }
            

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

        public IActionResult Sign()
        {
            List<Testmodel> listtest = new List<Testmodel>();

            /*if (Request.QueryString["code"] != null)
            {
                string Strcode = Request.QueryString["code"].ToString().Trim();
            }

            string LocationID = "CGQV01";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["LocationID"]))
            {
                LocationID = HttpContext.Request.Query["LocationID"].ToString();

                DataTable dt = dal_OndutySign.getInforFormByLocationID(LocationID);
                ViewBag.AreaName = dt.Rows[0]["AreaName"].ToString();
                ViewBag.AreaID = dt.Rows[0]["AreaID"].ToString();
                if (dt.Rows[0]["latitude"] != null)
                {
                    ViewBag.latitude = dt.Rows[0]["latitude"].ToString();
                    ViewBag.longitude = dt.Rows[0]["longitude"].ToString();

                }
                else
                {
                    ViewBag.latitude = "0";
                    ViewBag.longitude = "0";
                }
                if (dt.Rows[0]["Type"].ToString() == "1")
                {
                    ViewBag.Type = 1;
                    ViewBag.Title = "Biểu tuần tra ký duyệt của nhân viên cảnh giới ATTW";
                    ViewData["ListRule"] = dal_OndutySign.GetListRulesByLocaIDType(LocationID, 1);
                }
                else
                {
                    ViewBag.Type = 2;
                    ViewBag.Title = "Biểu tuần tra ký duyệt của nhân viên phòng cháy ATTW";
                    ViewData["ListRule"] = dal_OndutySign.GetListRulesByLocaIDType(LocationID, 2);
                }
                ViewBag.LocationDetail = dt.Rows[0]["LocationDetailName"].ToString();

            }
            else
            {
                ViewData["ListRule"] = dal_OndutySign.GetListRulesByLocaIDType(LocationID, 1);
                DataTable dt = dal_OndutySign.getInforFormByLocationID(LocationID);
                ViewBag.AreaName = dt.Rows[0]["AreaName"].ToString();
                ViewBag.AreaID = dt.Rows[0]["AreaID"].ToString();
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
                ViewBag.LocationDetail = dt.Rows[0]["LocationDetailName"].ToString();
            }
            ViewBag.LocaiontID = LocationID;
            Model_IcvetOut model_Icvet = new Model_IcvetOut();

            List<Model_HistorySign> listModelHisSign = new List<Model_HistorySign>();
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["code"]))
            {
                string code = HttpContext.Request.Query["code"].ToString();
                ViewBag.testStr = HttpContext.Request.Query["code"].ToString();
                _ = BindDataAsync(code);
                //model_Icvet = PopulateGetUserCvet(code);
                ViewBag.testNameFromIc = McivetOut.realname;
                ViewBag.EmpNoFromIc = McivetOut.civetno;
                listModelHisSign = dal_OndutySign.getHistorySignByLocationIDNow(LocationID, McivetOut.civetno);
                ViewData["ListHistorySign"] = listModelHisSign;
            }
            else
            {
                ViewBag.testNameFromIc = "Đinh Đức Mạnh";
                ViewBag.EmpNoFromIc = "V0510589";
                ViewBag.testStr = "this is data of ViewBag";
                listModelHisSign = dal_OndutySign.getHistorySignByLocationIDNow(LocationID, "V0510589");
                ViewData["ListHistorySign"] = listModelHisSign;
            }


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
        }*/

        [HttpPost]
        public string GetInforAreaByLocationID()
        {
            DataTable dt = new DataTable();

            return JsonConvert.SerializeObject(dt);
        }


        [HttpPost]
        public ActionResult SignAttw(int Type, string EmpNo, string EmpName, string LocationID, int Isoke, string ImageName, string Notes)
        {
            try
            {
                ModelForOndutyAttwSign model = new ModelForOndutyAttwSign();
                model.Type = Type;
                model.EmpNo = EmpNo;
                model.EmpName = EmpName;
                model.LocationID = LocationID;
                model.Isoke = Isoke;
                model.ImageName = ImageName;
                model.Notes = Notes;
                dal_OndutySign.OndutySign_Sign(model);
                return Content("Success");
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }

        }

        [HttpPost]
        public ActionResult UpdateLocation(string LocationID,float Latitude, float Longitude)
        {
            try
            {
                dal_OndutySign.OndutySign_UpdateCoordinate(LocationID, Latitude, Longitude);
                return Content("Success");
            }
            catch(Exception ex)
            {
                return Content(ex.Message.ToString());
            }
        }

        [HttpPost]
        public JsonResult TestController()
        {
            List<Testmodel> list1 = new List<Testmodel>();
            Testmodel test1 = new Testmodel();
            test1.ID = 1;
            test1.Name = "Dinh manh";
            list1.Add(test1);
            var returnedData = new
            {
                id = 1,
                age = 23,
                name = "John Smith"
            };
            return Json(list1);
        }
        [HttpPost]
        public JsonResult TestSaveCode(string code)
        {

            dal_OndutySign.UpdateCodeTest(code);
            var returnedData = new
            {
                id = 1,
                age = 23,
                name = "Oke Have saved code"
            };
            return Json(returnedData);
        }


        public EmptyResult SaveCode()
        {
            dal_OndutySign.UpdateCodeTest("mmmmmmmmmmmmmmmmmmm");

            return new EmptyResult();
        }

        [HttpPost]
        public string getListTest()
        {
            /*Ham nay dung het*/
            DataTable dt = new DataTable();
            dt = dal_OndutySign.getListTest();
            var returnedData = new
            {
                id = 1,
                age = 23,
                name = dt.Rows[1]["LocationDetailName"].ToString()
            };
            return JsonConvert.SerializeObject(dt);

        }

        [HttpPost]
        public string GetHistorySignNow([FromBody]ModelInput_GetHistorySign model)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = dal_OndutySign.getHistorySignByLocationIDNowDt(model.LocationID, model.EmpNo);
                return JsonConvert.SerializeObject(dt);
            }
            catch(Exception ex)
            {
                Console.Write(ex);
            }
            return JsonConvert.SerializeObject(dt);


        }
        [HttpPost]
        public string GetHistorySignNow1(string LocationID, string EmpNo)
        {
            //DataTable dt = new DataTable();
            //dt = dal_OndutySign.getHistorySignByLocationIDNowDt("CGQV01", "V0510589");
            try
            {
                return JsonConvert.SerializeObject(dal_OndutySign.getHistorySignByLocationIDNowDt("CGQV01", "V0510589"));
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(ex);
            }
        }


        [HttpPost]
        public ActionResult UploadImage()
        {
            try
            {
                foreach (var formFile in Request.Form.Files)
                {
                    string fileName = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + formFile.FileName.Substring((formFile.FileName.LastIndexOf(".")), (formFile.FileName.Length - formFile.FileName.LastIndexOf(".")));

                    //var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\Upload", formFile.FileName);
                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\Upload", fileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }
                    return Json(fileName);
                }
                return Json("Please Try Again !!");
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Testprint(string mmm,string bbb)
        {   
            try {
                var retval = new
                {
                    a = 1,
                    str = "success"
                };
                return Content("success");
            }
            catch(Exception ex)
            {
                var retval = new
                {
                    a = 1,
                    str = "error"
                };
                return Content("error");
            }
        }

        
        [HttpPost]
        public string UploadImageNew()
        {
            
            foreach (var formFile in Request.Form.Files)
            {

                //string fileName = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + formFile.FileName.Substring((formFile.FileName.LastIndexOf(".")), (formFile.FileName.Length - formFile.FileName.LastIndexOf(".")));

                string fileName = Request.Form["ImageName"].ToString() + formFile.FileName.Substring((formFile.FileName.LastIndexOf(".")), (formFile.FileName.Length - formFile.FileName.LastIndexOf(".")));

                //var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\Upload", formFile.FileName);
                var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\Upload", fileName);
                using (FileStream fs = System.IO.File.Create(fulPath))
                {
                    formFile.CopyTo(fs);
                    fs.Flush();
                }
                var returnData = new
                {
                    ImageName = fileName
                };
                return JsonConvert.SerializeObject(returnData);
            }
            var returnedData = new
            {
                ImageName = "error"
            };
            return JsonConvert.SerializeObject(returnedData);
        }
        public static string CreateFileName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(10, 99).ToString();
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was uploaded.");
            }

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Get the file extension
            var fileExtension = Path.GetExtension(file.FileName);

            // Generate a new filename (e.g., using a GUID)
            var newFileName = $"{CreateFileName()}{fileExtension}";

            var filePath = Path.Combine(uploadPath, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { filePath = $"/Upload/{newFileName}" });
        }
        public async Task<ActionResult> UploadImageNew1()
        {

            foreach (var formFile in Request.Form.Files)
            {
                //string fileName = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + formFile.FileName.Substring((formFile.FileName.LastIndexOf(".")), (formFile.FileName.Length - formFile.FileName.LastIndexOf(".")));

                string fileName = Request.Form["ImageName"].ToString() + formFile.FileName.Substring((formFile.FileName.LastIndexOf(".")), (formFile.FileName.Length - formFile.FileName.LastIndexOf(".")));

                //var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\Upload", formFile.FileName);
                var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\Upload", fileName);
                using (FileStream fs = System.IO.File.Create(fulPath))
                {
                    await formFile.CopyToAsync(fs);
                    fs.Flush();
                }
                var returnData1 = "Oke";
                return Content(returnData1);
            }
            var returnData = "error";
            return Content(returnData);
        }
        public async Task BindDataAsync(string code)
        {
            string URL = @"http://civetInterface.foxconn.com/open/get_user_info_bycode";
            string urlParameters = "?code=" + code+ "&appid=A36PAwlql7vvuGDueqKSMw2";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                McivetOut = (new JavaScriptSerializer()).Deserialize<Models.Model_IcvetOut>(result);
            }
        }
        /*private Models.Model_IcvetOut PopulateGetUserCvet(string Strcode)
        {
            /*string apiUrl = "http://civetInterface.foxconn.com/open/get_user_info_bycode";
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
            var baseAddress = "http://civetInterface.foxconn.com/open/get_user_info_bycode";

            var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
            http.Accept = "application/json";
            http.ContentType = "application/json";
            //http.Headers.Add("Access-Control-Allow-Origin", "*");
            //http.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            http.Method = "POST";
            object input = new
            {
                code = Strcode,
                appid = "A36PAwlql7vvuGDueqKSMw2"
            };
            string inputJson = (new JavaScriptSerializer()).Serialize(input);
            string parsedContent = inputJson;
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(parsedContent);

            Stream newStream = http.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();

            var response = http.GetResponse();

            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();
            Models.Model_IcvetOut model1 = new Models.Model_IcvetOut();
            model1 = (new JavaScriptSerializer()).Deserialize<Models.Model_IcvetOut>(content);
            return model1;
        }*/


    }
}
