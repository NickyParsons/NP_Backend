﻿using Microsoft.AspNetCore.Mvc;
using TestAspNetApplication.DTO;
using TestAspNetApplication.Services;

namespace TestAspNetApplication.Controllers
{
    public class UsersController : Controller
    {
        private ILogger<UsersController> _logger { get; set; }
        private IUserService _userService;
        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        [Route("/register")]
        [HttpGet]
        public async Task ShowRegisterPage()
        {
            Request.Headers["Cache-Control"] = "no-cache";
            Response.Headers["Cache-Control"] = "no-cache";
            Response.ContentType = "text/html; charset=utf-8";
            await Response.SendFileAsync("wwwroot/html/registerPage.html");
        }
        [Route("/register")]
        [HttpPost]
        public async Task<IActionResult> RegisterNewUser(RegisterUserRequest form)
        {
            _logger.LogDebug($"Регистрация нового пользователя {form.Email} {form.Firstname} {form.Lastname}");
            await _userService.Register(form);
            return Redirect("/");
        }
        [Route("/login")]
        [HttpGet]
        public async Task ShowLoginPage()
        {
            Response.ContentType = "text/html; charset=utf-8";
            await Response.SendFileAsync("wwwroot/html/loginPage.html");
        }
        [Route("/login")]
        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginUserRequest form)
        {
            string? token = await _userService.Login(form);
            if (token != null)
            {
                Response.Cookies.Append("nasty-boy", token);
            }
            else 
            {
                //нет никакого сообщения пользователю если логин\пасс не подходят. Надо исправить.
            }
            return Redirect("/");
        }
        [Route("/logout")]
        [HttpGet]
        public IActionResult LogoutUser()
        {
            Response.Cookies.Delete("nasty-boy");
            return Redirect("/");
        }
    }
}
