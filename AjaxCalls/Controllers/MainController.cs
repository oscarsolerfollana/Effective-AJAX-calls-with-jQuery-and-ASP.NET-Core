using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AjaxCalls.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MainController : Controller
    {
        List<string> names = new List<string>
        {
            "Ana", "Alberto", "Alba", "Luís", "Laura", "Luisa"
        };
        [Route("index")]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View("~/Views/Main.cshtml");
        }

        [HttpPost("getnames")]
        public JsonResult GetNames([FromBody] Pattern pattern)
        {
            lock("AjaxCall")
            {
                System.IO.File.AppendAllText(@"c:\prueba\log.txt", pattern.text + Environment.NewLine);
            }
            if(pattern.text=="")
            {
                return Json(names);
            }
            return Json(names.Where(str => str.ToUpper().StartsWith(pattern.text.ToUpper())));
        }
    }

    public class Pattern
    {
        public string text { get; set; }
    }
}