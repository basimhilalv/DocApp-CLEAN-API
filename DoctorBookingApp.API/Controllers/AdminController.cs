using DoctorBookingApp.API.AppResponse;
using DoctorBookingApp.Application.Interfaces.Services;
using DoctorBookingApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorBookingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet("GetUsers")]
        public async Task<IActionResult> getUsers()
        {
            try
            {
                var result = await _adminService.GetUsers();
                if (result == null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "Users not found"));
                return Ok(new ApiResponse<IEnumerable<User>>(200,"Users List available",result));
            }catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [HttpGet("GetDoctorList")]
        public async Task<IActionResult> getDoctorsList()
        {
            try
            {
                var result = await _adminService.GetDoctorsList();
                if (result == null) return BadRequest(new ApiResponse<string>(400,"Failed",null,"Doctors not found"));
                return Ok(new ApiResponse<IEnumerable<Doctor>>(200, "Doctor list available", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [HttpGet("GetPatientList")]
        public async Task<IActionResult> getPatientList()
        {
            try
            {
                var result = await _adminService.GetPatientList();
                if (result == null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "Patients not found"));
                return Ok(new ApiResponse<IEnumerable<Patient>>(200, "Doctor list available", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [HttpPost("BlockOrUnblock")]
        public async Task<IActionResult> blockorunblock(Guid userId)
        {
            try
            {
                var result = await _adminService.BlockOrUnblock(userId);
                if (result == null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "Operation Failed"));
                return Ok(new ApiResponse<string>(200, "User is blocked/unblocked", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [HttpPost("VerifyDoctor")]
        public async Task<IActionResult> verifyDoctor(Guid doctorId)
        {
            try
            {
                var result = await _adminService.VerifyDoctor(doctorId);
                if (result == null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "Can not verify doctor"));
                return Ok(new ApiResponse<string>(200, "Doctor is verified", result));
            }
            catch(Exception ex) 
            {
                return BadRequest(new ApiResponse<string>(400,"Failed",null, ex.Message));
            }
        }
        [HttpPost("BlockDoctor")]
        public async Task<IActionResult> blockDoctor(Guid doctorId)
        {
            try
            {
                var result = await _adminService.BlockDoctor(doctorId);
                if (result == null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "Can not block doctor"));
                return Ok(new ApiResponse<string>(200, "Doctor is verified", result));
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400,"Failed",null,ex.Message));
            }
        }
    }
}
