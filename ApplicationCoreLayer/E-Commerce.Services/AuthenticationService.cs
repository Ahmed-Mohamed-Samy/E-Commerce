using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<ApplicationUser> userManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
             var User = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (User is null)
               return Error.InvalidCredentials("User Invalid Cerdentials");

            var IsPasswordValid = await _userManager.CheckPasswordAsync(User, loginDTO.Password);

            if (!IsPasswordValid)
                return Error.InvalidCredentials("User Invalid Cerdentials");

            var Token = await CreateTokenAsync(User);
            return new UserDTO(User.Email!, User.DisplayName, Token);





        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var User = new ApplicationUser()
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.UserName,
            };

            var IdentityResult = await _userManager.CreateAsync(User,registerDTO.Password);

            if (IdentityResult.Succeeded)
            {
                var Token = await CreateTokenAsync(User);
                return new UserDTO(User.Email, User.DisplayName,Token);
            }

            return IdentityResult.Errors.Select(E => Error.Validation(E.Code,E.Description)).ToList();

        }
        public async Task<bool> CheckEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            return User is not null;
        }

        public async Task<Result<UserDTO>> GetUserByEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User is null)
                return Error.NotFound("User Not Found", $"No User With Email {email} Was Found");

            return new UserDTO(User.Email!, User.DisplayName, await CreateTokenAsync(User));
        }


        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var Claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                new Claim(JwtRegisteredClaimNames.Name,user.UserName!),
            };

            var Roles = await _userManager.GetRolesAsync(user);
            
            foreach(var role in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var SecretKey = _configuration["JWTOptions:SecretKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey!));
            var Cerdentials = new SigningCredentials(Key,SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken
                (
                    issuer: _configuration["JWTOptions:Issuer"],
                    audience: _configuration["JWTOptions:Audience"],
                    expires: DateTime.UtcNow.AddHours(1),
                    claims: Claims,
                    signingCredentials: Cerdentials
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
