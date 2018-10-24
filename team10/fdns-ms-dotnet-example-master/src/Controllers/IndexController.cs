#pragma warning disable 1591 // disables the warnings about missing Xml code comments

using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Foundation.Example.WebUI.Controllers
{
    [Route("api/1.0")]
    [ApiController]
    public sealed class IndexController : ControllerBase
    {
        private readonly string _version = "{ \"version\": \"" +  typeof(Startup).Assembly.GetName().Version.ToString() + "\" }";

        public IndexController()
        {
        }

        // GET api/1.0
        /// <summary>
        /// Gets the version number of this microservice
        /// </summary>
        /// <returns>Version number</returns>
        [Produces("application/json")]
        [HttpGet]
        public IActionResult Index()
        {
            return Content(_version);
        }
    }
}

#pragma warning restore 1591