using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services_Abstraction
{
    public interface IAuthenticationService
    {
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO); 
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO);
        Task<bool> CheckEmailAsync(string email);
        Task<Result<UserDTO>> GetUserByEmailAsync(string email);
    }
}
