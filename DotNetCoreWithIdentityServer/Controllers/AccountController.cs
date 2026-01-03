using DAL.Interfaces;
using DotNetCoreWithIdentityServer.Models;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreWithIdentityServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices _accountService;

        public AccountController(IAccountServices accountServices)
        {
            _accountService = accountServices;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isAuthenticateUser = await _accountService.IsUserExists(new SignInDTO() { Email = model.Email, Password = model.Password });

            if (isAuthenticateUser)
            {
                return Ok(new { Success = true, Message = "Login successful" });

            }
            return Ok(new { Success = false, Message = "Invalid Username or email" });
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            SignupDTO signupDTO = new SignupDTO { UserName = model.UserName, Email = model.Email, Password = model.Password, PhoneNumber = model.PhoneNumber };
            signupDTO = await _accountService.SignupUserAsync(signupDTO);

            return signupDTO != null ? Ok(new { Success = true, Message = "SignUp successful." }) : BadRequest("Please try agian. User not successfully signup");
        }
    }
}