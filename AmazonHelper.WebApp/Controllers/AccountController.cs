using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AmazonHelper.Business.Services;
using AmazonHelper.WebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace AmazonHelper.WebApp.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel loginModel)
        {
            var user = await _userService.ValidateUser(loginModel.UserName, loginModel.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Credentials are not valid");
            }
            else
            {
                await AuthenticateUser(user.UserName, user.UserId.ToString());
                return RedirectToDefault();
            }

            return View();
        }

        /// <summary>
        /// Authenticates client
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        private async Task AuthenticateUser(string userId, string userName)
        {
            var genericIdentity = new GenericIdentity(userName);
            genericIdentity.AddClaims(new[]
            {
                        new Claim(ClaimTypes.Name, userName),
                        new Claim("UserId", userId)
                    });

            var identity = new ClaimsIdentity(genericIdentity.Claims, DefaultAuthenticationTypes.ApplicationCookie);

            var claimsPrincipal = System.Web.HttpContext.Current.User as ClaimsPrincipal;
            claimsPrincipal?.AddIdentity(identity);

            HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);
        }
    }
}