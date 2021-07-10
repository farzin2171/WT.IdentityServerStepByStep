using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    //https://nahidfa.com/posts/migrating-identityserver4-to-v4/
    public class SecretController : Controller
    {
        [Route("/secret")]
        [Authorize]
        [HttpGet]
        public string Index()
        {
            return "Secret message from ApiOne";
        }
    }
}
