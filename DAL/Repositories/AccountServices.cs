using DAL.Interfaces;
using DTO;
using Entities;
using Microsoft.AspNetCore.Identity;

namespace DAL.Repositories
{
    public class AccountServices(UserManager<ApplicationUser> _userManager) : IAccountServices
    {
        public async Task<SignupDTO> SignupUserAsync(SignupDTO model)
        {
            ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email, PasswordHash = model.Password, PhoneNumber = model.PhoneNumber };

            IdentityResult result = await _userManager.CreateAsync(user, password: user.PasswordHash);

            if (!result.Succeeded)
                throw new Exception("User not successfully created.");

            return model;
        }

        public async Task<bool> IsUserExists(SignInDTO signInDTO)
        {

            if (string.IsNullOrEmpty(signInDTO.Email) || string.IsNullOrEmpty(signInDTO.Password))
            {
                throw new Exception("Email or Password is empty.");
            }

            var user = await _userManager.FindByEmailAsync(signInDTO.Email);

            if (user is null)
            {
                throw new Exception("User not found.");
            }

            return await _userManager.CheckPasswordAsync(user, signInDTO.Password);
        }



        private async Task SignIn(SignupDTO model)
        {
            // Validate user
            var user = await _userManager.FindByEmailAsync(model.Email);
            //if (user == null)
            //    return BadRequest(new { Message = "Invalid email or password" });

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            //if (!passwordValid)
            //    return BadRequest(new { Message = "Invalid email or password" });

            // Create a token request (Resource Owner Password flow)
            //var tokenRequest = new Duende.IdentityServer.Models.TokenRequest
            //{
            //    GrantType = "password",
            //    ClientId = "web-client",
            //    ClientSecret = "super-secret",
            //    Parameters =
            //{
            //    { "username", model.Email },
            //    { "password", model.Password },
            //    { "scope", "openid profile myapi offline_access" } // offline_access = refresh token
            //}
            //};

            // Normally, clients call /connect/token directly.
            // For internal call, we can simulate with HttpClient or call IdentityServer's Token endpoint
            // Here, simplest approach: direct client calls /connect/token externally.

            //return Ok(new
            //{
            //    Message = "Login successful",
            //    TokenEndpoint = "https://localhost:5001/connect/token",
            //    Info = "Client should call this endpoint with Resource Owner Password flow to get access & refresh token."
            //});
        }
    }
}
