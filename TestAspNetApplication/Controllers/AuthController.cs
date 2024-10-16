﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestAspNetApplication.Data;
using TestAspNetApplication.DTO;
using TestAspNetApplication.Services;

namespace TestAspNetApplication.Controllers
{
    public class AuthController : Controller
    {
        private ILogger<AuthController> _logger { get; set; }
        private AuthService _authService;
        public AuthController(ILogger<AuthController> logger, AuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }
        [Route("/register")]
        [HttpPost]
        public async Task<IActionResult> RegisterNewUser(RegisterUserRequest form)
        {
            try
            {
                await _authService.Register(form);
                _logger.LogInformation($"Successfull reistration {form.Email}");
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                return BadRequest(e.Message);
            }
        }
        [Route("/login")]
        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginUserRequest form)
        {
            try
            {
                string token = await _authService.Login(form);
                _logger.LogInformation($"Successfull login {form.Email}");
                return Json(new { token = token });
            }
            catch (Exception e)
            {
                _logger.LogDebug($"{e.Message}");
                return BadRequest("Email or password incorrect");
            }
        }
        [Route("/logout")]
        [HttpGet]
        public IActionResult LogoutUser()
        {
            try { Response.Cookies.Delete("nasty-boy"); }
            catch (Exception) { _logger.LogDebug("Cookie doesn't found"); }
            return Redirect("/");
        }
        [Route("/verify-email")]
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(string? token)
        {
            _logger.LogDebug($"Token: {token}");
            if (token == null) 
            {
                _logger.LogDebug("Token is null");
                return BadRequest("Incorrect token"); 
            }
            try
            {
                await _authService.VerifyEmail(token);
                _logger.LogDebug("Email successfully verified");
                return Ok("Email successfully verified");
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                return BadRequest($"{e.Message}");
            }
        }
        [Authorize]
        [Route("/change-email")]
        [HttpPost]
        public async Task<IActionResult> ChangeEmail(ChangeEmailRequest form)
        {
            _logger.LogDebug($"Triyng to change email: {form.Email} to {form.NewEmail}");
            var cookieEmail = HttpContext.User.Identity?.Name;
            if (cookieEmail == null)
            {
                _logger.LogDebug("Cant get old e-mail from cookie");
                return BadRequest("Something wrong with E-mail");
            }
            if (form.Email != cookieEmail)
            {
                _logger.LogDebug("Old email in request doesn't match user email in cookie");
                return BadRequest("Something wrong with E-mail");
            }
            if (form.NewEmail == null)
            {
                _logger.LogDebug("New e-mail is null");
                return BadRequest("E-mail is null");
            }
            try
            {
                await _authService.ChangeEmail(form);
                _logger.LogDebug("Email successfully changed");
                try { Response.Cookies.Delete("nasty-boy"); }
                catch (Exception) { _logger.LogDebug("Cookie doesn't found"); }
                return Ok("Email successfully changed");
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                return BadRequest($"{e.Message}");
            }
        }
        [Route("/forgot-password")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string? email)
        {
            if (email == null)
            {
                _logger.LogDebug("E-mail is empty");
                return BadRequest($"E-mail is empty");
            }
            try
            {
                await _authService.ForgotPassword(email);
                _logger.LogDebug("Password could be reset now");
                return Ok("Password could be reset now");
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                return BadRequest($"{e.Message}");
            }
        }
        [Route("/reset-password")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest form)
        {
            try
            {
                await _authService.ResetPassword(form);
                _logger.LogDebug("Password successfully changed");
                return Ok("Password successfully changed");
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                return BadRequest($"{e.Message}");
            }
        }
    }
}