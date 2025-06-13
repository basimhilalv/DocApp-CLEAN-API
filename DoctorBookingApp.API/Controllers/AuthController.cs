using DoctorBookingApp.API.AppResponse;
using DoctorBookingApp.Application.DTOs.UserDto;
using DoctorBookingApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorBookingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegDto request)
        {
            try
            {
                var result = await _authService.Register(request);
                if(result is null)
                {
                    return BadRequest(new ApiResponse<string>(400,"Registration Failed"));
                }
                return Ok(new ApiResponse<string>(200,result));
            }catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400,"Registration Failed",null, ex.Message));
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            try
            {
                var result = await _authService.Login(request);
                if (result is null) return BadRequest(new ApiResponse<string>(400, "Login Failed"));
                return Ok(new ApiResponse<UserLogResDto>(200, "Login Successfull", result, null));
            }catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Login Failed", null, ex.Message));
            }
        }
    }
}
