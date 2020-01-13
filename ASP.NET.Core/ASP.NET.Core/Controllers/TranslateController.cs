using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpOperation.QueryParams;
using Microsoft.AspNetCore.Mvc;
using Service.AliyunTranslate;
using static Service.AliyunTranslate.TranslateHelper;

namespace ASP.NET.Core.Controllers
{
    public class TranslateController : Controller
    {
        public TranslateHelper translateHelper = new TranslateHelper();
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Trasnlate(AliyunTranslateParams translateParams)
        {
            var result = "";
            if (translateParams.TranslationEngine == 0)
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