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
            Response.Cookies.Delete("nasty-boy");
            return Redirect("/");
        }
        [Route("/verify-email")]
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(string token)
        {
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
            _logger.LogDebug($"Change email: {form.Email} to {form.NewEmail}");
            var cookieEmail = HttpContext.User.Claims.First(c => c.Type == "name").Value;
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
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                await _authService.ForgotPassword(email);
                return Ok("Password could be reset now");
            }
            catch (Exception e)
            {
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
                return Ok("Password successfully changed");
            }
            catch (Exception e)
            {
                return BadRequest($"{e.Message}");
            }
        }
    }
}