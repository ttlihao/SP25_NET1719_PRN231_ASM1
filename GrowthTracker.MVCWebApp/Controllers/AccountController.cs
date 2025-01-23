using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Net.Http;
using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;
using GrowthTracker.MVCWebApp.Models;

namespace GrowthTracker.MVCWebApp.Controllers
{
    public class AccountController : Controller
    {
        private string APIEndPoint = "https://localhost:7066/api/";

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest login)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsJsonAsync(APIEndPoint + "Account/Login", login);
                    if (response.IsSuccessStatusCode)
                    {
                        var tokenString = await response.Content.ReadAsStringAsync();
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var jwtToken = tokenHandler.ReadToken(tokenString) as JwtSecurityToken;

                        if (jwtToken != null)
                        {
                            var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, userName),
                                new Claim(ClaimTypes.Role, role),
                            };

                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                            Response.Cookies.Append("UserName", userName);
                            Response.Cookies.Append("Role", role);

                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                ModelState.AddModelError("", "Login failure: " + ex.Message);
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ModelState.AddModelError("", "Login failure");
            return View();
        }

        [HttpPost()]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Forbidden()
        {
            return View();
        }
    }
}
