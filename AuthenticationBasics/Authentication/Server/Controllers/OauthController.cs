using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controllers
{
    public class OauthController : Controller
    {
        public IActionResult Authorize(string response_type,  //authorization flow type
                                       string client_id,
                                       string redirect_uri,
                                       string scope, //what information i want = email,webtech,tel
                                       string state) //random string generated to clonfirm that we are going to back the same clinet
        {
            var query = new QueryBuilder();
            query.Add("redireUri", redirect_uri);
            query.Add("state", state);
            return View(model: query.ToString());
        }

        [HttpPost]
        public IActionResult Authorize(string userName,
                                       string redireUri,
                                       string state)
        {
            const string code = "BABABA";
            var query = new QueryBuilder();
            query.Add("code", code);
            query.Add("state", state);
            return Redirect($"{redireUri}{query.ToString()}");
        }

        public async Task<IActionResult> Token(string grant_type, //flow of accec_token request
                                   string code, //confiramtion of the authenticateion 
                                   string redirect_uri,
                                   string client_id)
        {
            //Some mechanism for validating the code 
            var claims = new[]
           {
                new Claim(JwtRegisteredClaimNames.Sub,"Some_i"),
                new Claim("WebTechId","id")
            };
            var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;
            var signingCredentials = new SigningCredentials(key, algorithm);
            var token = new JwtSecurityToken(Constants.Audiance,
                                           Constants.Issuer,
                                           claims,
                                           DateTime.Now,
                                           DateTime.Now.AddDays(1),
                                           signingCredentials);

            var access_token = new JwtSecurityTokenHandler().WriteToken(token);

            var responseObject = new
            {
                access_token,
                token_type = "Bearer",
                raw_claim = "oauthTutorial"
            };
            var responseJson = JsonConvert.SerializeObject(responseObject);
            var responseBytes = Encoding.UTF8.GetBytes(responseJson);
            await Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);

            return Redirect(redirect_uri);

        }

        [Authorize]
        public IActionResult Validate()
        {
            if (HttpContext.Request.Query.TryGetValue("access_token", out var acessToken))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
