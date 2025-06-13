using AutoMapper;
using BCrypt.Net;
using DoctorBookingApp.Application.DTOs.UserDto;
using DoctorBookingApp.Application.Interfaces.Services;
using DoctorBookingApp.Domain.Entities;
using DoctorBookingApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DoctorBookingApp.Infrastructure.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        public AuthService(IConfiguration config, IMapper mapper, AppDbContext context)
        {
            _config = config;
            _mapper = mapper;
            _context = context;
        }
        public async Task<UserLogResDto> Login(UserLoginDto request)
        {
            try
            {
                var user = await _context.Users.Include(u=>u.Role).FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user is null) throw new InvalidOperationException("Invalid Email");
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password)) throw new InvalidOperationException("Invalid Password");

                var token = CreateToken(user);

                var loginRes = new UserLogResDto
                {
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role.Name,
                    token = token
                };
                return loginRes;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> Register(UserRegDto request)
        {
            try
            {
                if (request is null) throw new ArgumentNullException("Request can not be null");

                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (existingUser != null) throw new Exception("User with same Email already exists");

                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hashpass = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

                var user = _mapper.Map<User>(request);
                user.Password = hashpass;
                user.IsActive = true;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return "User Registered Successfully";

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string CreateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddHours(2)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
