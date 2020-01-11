using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ASP.NET.Core.Models;
using HttpOperation.QueryParams;
using Service.AliyunTranslate;
using static Service.AliyunTranslate.TranslateHelper;

namespace ASP.NET.Core.Controllers
{
    public class HomeController : Controller
    {
        public TranslateHelper translateHelper=new TranslateHelper();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public IActionResult Trasnlate(AliyunTranslateParams translateParams)
        {
            var result = "";
            if (translateParams.TranslationEngine==0)
            {
               result = translateHelper.HttpAliyunECommerceTranslate(translateParams.Content,
               (Language)translateParams.SourceLanguage,
               (Language)translateParams.TargetLanguage,
               (Scene)translateParams.Scene, translateParams.FormatType);
               
            }
            else
            {
               result = translateHelper.HttpAliyunTranslate(translateParams.Content,
              (Language)translateParams.SourceLanguage,
               (Language)translateParams.TargetLanguage,
               (Scene)translateParams.Scene, translateParams.FormatType);
            }
            return new JsonResult(result);
        }
    }
}
