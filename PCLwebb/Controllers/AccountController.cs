﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PCLwebb.Models;

namespace PCLwebb.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private ModelContext context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ModelContext modelContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = modelContext;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                loginViewModel.Email,
                loginViewModel.Password,
                isPersistent: loginViewModel.RememberMe,
                lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Fel användarnam/lösenord.");
                }
            }
            return View(loginViewModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = registerViewModel.Email,
                    Firstname = registerViewModel.Firstname,
                    Lastname = registerViewModel.Lastname,
                    BirthDay = registerViewModel.BirthDay,
                    UserName = registerViewModel.Email,
                    EmailConfirmed = false
                };

                var result = await userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        if (error.Code == "DuplicateUserName")
                        {
                            ModelState.AddModelError("", "E-posten är redan registrerad.");
                            return View(registerViewModel);
                        }

                    }
                    ModelState.AddModelError("", "Lösenordet måste innehålla:");
                    ModelState.AddModelError("", "Minst 6 tecken långt");
                    ModelState.AddModelError("", "Bokstav, stor och liten");
                    ModelState.AddModelError("", "Tecken (ex: !%&=?)");
                }
            }
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            IQueryable<User> userList = from user in context.Users select user;
            userList = userList.Where(user => user.UserName == User.Identity.Name);
            User theUser = userList.FirstOrDefault();
            return View(theUser);
        }

        [HttpPost]
        public IActionResult UpdateUserInfo(User user)
        {
            IQueryable<User> userList = from u in context.Users select u;
            userList = userList.Where(user => user.UserName == User.Identity.Name);
            User theUser = userList.FirstOrDefault();

            theUser.Firstname = user.Firstname;
            theUser.Lastname = user.Lastname;

            
                theUser.CompanyName = user.CompanyName;
            

            context.Users.Update(theUser);
            context.SaveChanges();
            return RedirectToAction("Profile");
        }
    }
}
