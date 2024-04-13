using _535_Assignment.Models;
using _535_Assignment.Repository;
using _535_Assignment.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

namespace _535_Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ContextShopping _contextShopping;
        private readonly IWebHostEnvironment _env;
        private readonly AuthRepository _authRepository;
        private readonly SanitiserService _sanitiserService;

        public HomeController(ILogger<HomeController> logger, ContextShopping context, 
            IWebHostEnvironment env, AuthRepository authRepository, 
            SanitiserService sanitiserService)
        {
            _logger = logger;
            _contextShopping = context;
            _env = env;
            _authRepository = authRepository;
            _sanitiserService = sanitiserService;
        }
        
        public IActionResult Index()
        { 
            return View();
        }
       

        public IActionResult Login([FromQuery] string ReturnUrl)
        {
            LoginDTO login = new LoginDTO()
            {
                RedirectURL = String.IsNullOrEmpty(ReturnUrl) ? "/Home" : ReturnUrl
            };
            return View(login);
        }

        /// <summary>
        /// Signs in a user and allows them to access and manage their shopping lists. Admin's are granted additional features.
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO userLogin)
        {
            var sanitisedUsername = _sanitiserService.Sanitiser.Sanitize(userLogin.user);

            var user = _authRepository.Authenticate(userLogin);

            if (user == null)
            {
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim("ID", user.AppUserId.ToString()),
                new Claim(ClaimTypes.Name, sanitisedUsername),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                RedirectUri = userLogin.RedirectURL
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
                );

            return Redirect(userLogin.RedirectURL ?? "/home/Index");

        }

        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Creates an account for the user in the database.
        /// </summary>
        /// <param name="registerDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Register(RegisterDTO registerDetails)
        {
            var sanitisedUsername = _sanitiserService.Sanitiser.Sanitize(registerDetails.user);
            var sanitisedPassword = _sanitiserService.Sanitiser.Sanitize(registerDetails.password);

            try
            {
                AppUser newUser = new AppUser()
                {
                    UserName = sanitisedUsername,
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword(sanitisedPassword),
                    Role = "User"
                };

                _contextShopping.AppUsers.Add(newUser);
                _contextShopping.SaveChanges();

                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                return View();
            }
        }

        /// <summary>
        /// Deletes the context of the session, signing out the user.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}