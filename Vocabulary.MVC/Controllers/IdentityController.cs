using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vocabulary.MVC.Models;

namespace Vocabulary.MVC.Controllers
{
    public class IdentityController : Controller
    {
        private readonly string _apiUri = "http://localhost:5031/api/identity";
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IdentityController(
            IHttpClientFactory clientFactory, 
            IConfiguration configuration, 
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Login", "Identity");
        }
        
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            var user = new IdentityUser
            {
                UserName = model.Username,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Identity");
            }
            else
            {
                ModelState.AddModelError("Error", "Failed to create account");
                return View(model);
            }
        }
        public async Task<IActionResult> Login(LoginUserViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return View(model);
            }
            
            var user =  await _userManager.FindByEmailAsync(model.Email);
            
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized();
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiUri);

            var userDto = new {Id = user.Id};
            HttpResponseMessage response = client.PostAsJsonAsync($"{client.BaseAddress}/token", userDto).Result;
            if (response.IsSuccessStatusCode)
            {
                var token = response.Content.ReadAsStringAsync().Result;
                HttpContext.Session.SetString("jwt", token);
                return RedirectToAction("Index", "Word");
            }
            else
            {
                ModelState.AddModelError("Error", "Failed to login");
                return View(model);
            }
        }
    }
}
