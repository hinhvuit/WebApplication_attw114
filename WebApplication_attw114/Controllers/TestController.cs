using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PDSS.CivetPublic.User;
using Nancy.Json;
using System;

namespace WebApplication_attw114.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["Position"]) && !string.IsNullOrEmpty(HttpContext.Request.Query["Code"]))
            {
                /*
                string Position = HttpContext.Request.Query["Position"].ToString();
                string code = HttpContext.Request.Query["Code"].ToString();

                string appID = "A36PAwlql7vvuGDueqKSMw2";
                OAuth oAuth = new OAuth(appID);

                oAuth.url = @"http://civetinterface.foxconn.com/Open/";

                OAuth.Token token = oAuth.AccessToken(code);
                OAuth.UserInfo useInfor = oAuth.GetUserInfo(token);
                OAuth.PostionInfo OauthpostionInfo = oAuth.GetPostionInfo(token.openid,Position);

                string PossitionInfoStr = new JavaScriptSerializer().Serialize(OauthpostionInfo);

                //ViewBag.testPossition = PossitionInfoStr;
                */
                ViewBag.testPossition = "aa";

                return View();
            }
            else
            {
                ViewBag.testPossition = "Null";
                return View();
            }
            
        }

        [HttpPost]
        public IActionResult PostModel(Models.ModelForTestTable model)
        {
            string ContentResult = string.Empty;
            if(ModelState.IsValid)
            {
                Dal.Dal_TestTb dal_TestTb = new Dal.Dal_TestTb();
                dal_TestTb.Add(model);
                ContentResult = "Them thanh cong: sdt:" + model.sdt + "Ma the:" + model.Mathe + "Diachi:" + model.Diachi;
            }
            else
            {
                ContentResult = "Insert failed!";
            }
            return Content(ContentResult);
        }


    }
}
