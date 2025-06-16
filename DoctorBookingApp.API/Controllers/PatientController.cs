using DoctorBookingApp.API.AppResponse;
using DoctorBookingApp.Application.DTOs.PatientDto;
using DoctorBookingApp.Application.DTOs.PaymentDto;
using DoctorBookingApp.Application.Interfaces.Services;
using DoctorBookingApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoctorBookingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }
        [Authorize(Roles ="Patient")]
        [HttpGet("VideoToken")]
        public async Task<IActionResult> getVideoToken(Guid appointmentId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _patientService.GenerateVideoToken(userIdguid, appointmentId);
                if (result is null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "Operation Failed"));
                return Ok(new ApiResponse<string>(200, "Video Token Generated", result));
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [Authorize(Roles = "Patient")]
        [HttpPost("createPaymentintent")]
        public async Task<IActionResult> CreateIntent(CreatePaymentDto payment)
        {
            try
            {
                if(payment.PaymentMethod.ToLower() == "cash")
                {
                    var result = await _patientService.MarkAppointmentAsCash(payment.AppointmentId);
                    if (result is null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "Operation Failed"));
                    return Ok(new ApiResponse<string>(200, "Payment made as cash", result));
                }
                var clientSecret = await _patientService.CreateStripePaymentIntent(payment.AppointmentId);
                return Ok(new ApiResponse<string>(200, "Payment initiated", clientSecret));
            }catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [Authorize(Roles = "Patient")]
        [HttpPost("ConfirmPayment")]
        public async Task<IActionResult> ConfirmPayment(string paymentIntentId, string transactionId)
        {
            try
            {
                var result = await _patientService.MarkAppointmentAsPaid(paymentIntentId, transactionId);
                if (result is null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "Operation Failed"));
                return Ok(new ApiResponse<string>(200, "Payment Confirmed", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [Authorize(Roles ="Patient")]
        [HttpGet("doctors")]
        public async Task<IActionResult> getDoctors()
        {
            try
            {
                var result = await _patientService.GetAvailableDoctors();
                if (result is null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "Doctors not available"));
                return Ok(new ApiResponse<IEnumerable<Doctor>>(200, "All Doctors retrieved", result));
            }catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [Authorize(Roles ="Patient")]
        [HttpGet("GetTimeSlots")]
        public async Task<IActionResult> getTimeSlots(Guid doctorId)
        {
            try
            {
                var result = await _patientService.GetTimeSlots(doctorId);
                if (result is null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "TimeSlots are not available"));
                return Ok(new ApiResponse<IEnumerable<TimeSlot>>(200, "All TimeSlots retrieved", result));
            }catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400,"Failed",null, ex.Message));
            }
        }
        [Authorize(Roles ="Patient")]
        [HttpGet("GetAppointments")]
        public async Task<IActionResult> getappointments()
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _patientService.GetAppointments(userIdguid);
                if (result is null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "Appointments not available"));
                return Ok(new ApiResponse<IEnumerable<Appointment>>(200, "All Appointments retrieved", result));
            }catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [Authorize(Roles ="Patient")]
        [HttpPost("CreateAppointment")]
        public async Task<IActionResult> makeAppointment(Guid slotId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _patientService.MakeAppointment(userIdguid, slotId);
                if (result is null)
                {
                    return BadRequest(new ApiResponse<string>(400, "Failed", null, "Appointment can't be created"));
                }
                return Ok(new ApiResponse<string>(200, "Appointment created successfully", result, null));
            }catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [Authorize(Roles = "Patient")]
        [HttpDelete("CancelAppointment")]
        public async Task<IActionResult> cancelAppointment(Guid appointmentId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _patientService.CancelAppointment(userIdguid, appointmentId);
                if (result is null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "Can't find appointment details"));
                return Ok(new ApiResponse<string>(200, "Appointment Cancelled Successfully", result, null));
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [Authorize(Roles = "Patient")]
        [HttpGet("Profile")]
        public async Task<IActionResult> getUserProfile()
        {
            try
            {

                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _patientService.GetPatientProfile(userIdguid);
                if(result is null)
                {
                    return BadRequest(new ApiResponse<string>(400, "Failed", null, "User Profile deosn't exists"));
                }
                return Ok(new ApiResponse<PatientResDto>(200, "User profile retrieved successfully", result, null));
            }catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [Authorize(Roles = "Patient")]
        [HttpPost("CreateProfile")]
        public async Task<IActionResult> createUserProfile([FromForm] PatientReqDto request)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401,"User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _patientService.CreatePatientProfile(userIdguid, request);
                if(result is null)
                {
                    return BadRequest(new ApiResponse<string>(400, "Failed", null, "User Profile can't be created"));
                }
                return Ok(new ApiResponse<string>(200, result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [Authorize(Roles = "Patient")]
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> updateUserProfile([FromForm] PatientReqDto request)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _patientService.UpdatePatientProfile(userIdguid, request);
                if (result is null)
                {
                    return BadRequest(new ApiResponse<string>(400, "Failed", null, "User Profile can't be updated"));
                }
                return Ok(new ApiResponse<string>(200, result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [Authorize(Roles = "Patient")]
        [HttpPost("DeleteProfile")]
        public async Task<IActionResult> deleteUserProfile()
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _patientService.DeletePatientProfile(userIdguid);
                if (result is null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "User Profile deletion unsuccessfull"));
                return Ok(new ApiResponse<string>(200, result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
    }
}
