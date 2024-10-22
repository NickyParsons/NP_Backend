using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestAspNetApplication.Data;
using TestAspNetApplication.DTO;
using TestAspNetApplication.Services;
using TestAspNetApplication.Data.Entities;

namespace TestAspNetApplication.Controllers
{
    public class UsersController : Controller
    {
        private ILogger<UsersController> _logger { get; set; }
        private ProfileService _profileService;
        private UserRepository _userRepository;
        public UsersController(ILogger<UsersController> logger, ProfileService profileService, UserRepository userRepository)
        {
            _logger = logger;
            _profileService = profileService;
            _userRepository = userRepository;
        }
        [Route("/users/{id:guid}")]
        [HttpGet]
        public async Task<IActionResult> GetUserData(Guid id)
        {
            try
            {
                var user = await _profileService.GetProfileData(id);
                return Json(user);
            }
            catch (BadHttpRequestException e)
            {
                _logger.LogDebug(e.Message);
                return BadRequest(e.Message);
            }
        }
        [Route("/users/{id:guid}/edit")]
        [HttpPost]
        public async Task<IActionResult> EditCurrentUser(EditUserRequest form)
        {
            _logger.LogDebug($"Triyng to change profile: {form.Id}");

            var cookieId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "id").Value);
            if (cookieId != form.Id) {
                _logger.LogDebug($"Cookie ID and form ID doesn't match");
                return BadRequest("Something wrong with ID");
            }
            User dbUser;
            if (Request.Form.Files.Count != 0)
            {
                dbUser = await _profileService.EditProfile(form, Request.Form.Files.First());
            }
            else
            {
                dbUser = await _profileService.EditProfile(form, null);
            }
            return Json(dbUser);
        }
    }
}