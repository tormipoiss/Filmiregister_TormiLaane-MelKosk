using Filmiregister.Dto;
using Filmiregister.Models;
using Filmiregister.ServiceInterface;
using Filmiregister.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Filmiregister.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailsServices _emailsServices;

        public AccountsController(UserManager<ApplicationUser> userManager, IEmailsServices emailsServices, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _emailsServices = emailsServices;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Register(Account model)
        {
            if (ModelState.IsValid)
            {
                // Check if username already exists
                var existingUserByName = await _userManager.FindByNameAsync(model.Name);
                if (existingUserByName != null)
                {
                    ModelState.AddModelError("Name", "This username is already taken.");
                    return View(model);
                }

                // Check if email already exists
                var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
                if (existingUserByEmail != null)
                {
                    ModelState.AddModelError("Email", "This email address is already registered.");
                    return View(model);
                }

                var account = new ApplicationUser()
                {
                    UserName = model.Name,
                    Email = model.Email,
                    IsAdmin = false
                };
                var result = await _userManager.CreateAsync(account, model.Password);
                TempData["NewUserID"] = account.Id;

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(account);

                    var confirmationLink = Url.Action("ConfirmEmail", "Accounts", new { accountId = account.Id, token = token }, Request.Scheme);

                    EmailTokenDto newsignup = new();
                    newsignup.Token = token;
                    newsignup.Body = $"Thank you for registering, click here: {confirmationLink}";
                    newsignup.Subject = "Filmiregister sign up";
                    newsignup.To = account.Email;

                    _emailsServices.SendEmailToken(newsignup, token);
                    //if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    //{
                    //    return RedirectToAction("ListUsers", "Administrations");
                    //}

                    return View("CheckEmail");/*RedirectToAction("CheckEmail", "Accounts")*/
                }
                else
                {
                    // Add Identity errors to ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            // If we got this far, something failed, redisplay form with model to preserve user input
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ConfirmEmail(string accountId, string token)
        {
            if (accountId == null || token == null) { return RedirectToAction("Index", "Home"); }
            var account = await _userManager.FindByIdAsync(accountId);
            if (account == null)
            {
                ViewBag.ErrorTitle = "User is not valid";
                ViewBag.ErrorMessage = $"The user with id of {accountId} is not valid";
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(account, token);
            //List<string> errordatas =
            //            [
            //            "Area", "Accounts",
            //            "Issue", "Success",
            //            "StatusMessage", "Registration Success",
            //            "ActedOn", $"{user.Email}",
            //            "CreatedAccountData", $"{user.Email}\n{user.City}\n[password hidden]\n[password hidden]"
            //            ];
            if (result.Succeeded)
            {
                //errordatas =
                //        [
                //        "Area", "Accounts",
                //        "Issue", "Success",
                //        "StatusMessage", "Registration Success",
                //        "ActedOn", $"{user.Email}",
                //        "CreatedAccountData", $"{user.Email}\n{user.City}\n[password hidden]\n[password hidden]"
                //        ];
                //ViewBag.ErrorDatas = errordatas;
                return View("RegisterSuccess");
            }
            //ViewBag.ErrorDatas = errordatas;
            ViewBag.ErrorTitle = "Email cannot be confirmed";
            ViewBag.ErrorMessage = $"The users email, with id of {accountId}, cannot be confirmed";
            return View("Error");
        }

        //public IActionResult CheckEmail()
        //{
        //    return View();
        //}
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null) return RedirectToAction("Index", "Home");
                if (user != null && !user.EmailConfirmed)
                {
                    ViewBag.ErrorTile = "Confirm Email";
                    ViewBag.ErrorMessage = "Your email is not confirmed yet. Please confirm it.";
                    return View("Error");
                }
                var check = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!check)
                {
                    ViewBag.ErrorTile = "Invalid login";
                    ViewBag.ErrorMessage = "Email or password is incorrect";
                    return View("Error");
                }
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.ErrorTile = "Invalid login";
                ViewBag.ErrorMessage = "Email or password is incorrect";
                return View("Error");
            }
            return View(model);
        }


    }
}
