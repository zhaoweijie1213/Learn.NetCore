using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeetCodeWeb.Controllers
{
    /// <summary>
    /// leetCode
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LeetCodeController : ControllerBase
    {
        /// <summary>
        /// test方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetName")]
        public string GetName()
        {
            return "kizuna_ai";
        }
    }
}