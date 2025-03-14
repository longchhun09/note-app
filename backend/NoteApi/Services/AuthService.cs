using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NoteApi.DTOs.Auth;
using NoteApi.Models;
using NoteApi.Repositories.Interfaces;
using NoteApi.Services.Interfaces;

namespace NoteApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IConfiguration configuration
        )
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _configuration = configuration;
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDto)
        {
            var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
            if (
                user == null
                || !_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash)
            )
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            var token = GenerateJwtToken(user);
            return new AuthResponseDTO { Token = token, Username = user.Username };
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDto)
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                throw new ArgumentException("Passwords do not match");
            }

            var existingUser = await _userRepository.GetByUsernameAsync(registerDto.Username);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Username already exists");
            }

            // Generate a random salt
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = _passwordService.HashPassword(registerDto.Password),
                PasswordSalt = saltBytes,
                CreatedAt = DateTime.UtcNow,
                RefreshToken = Guid.NewGuid().ToString(),
            };

            user.Id = await _userRepository.CreateAsync(user);
            var token = GenerateJwtToken(user);

            return new AuthResponseDTO { Token = token, Username = user.Username };
        }

        private string GenerateJwtToken(User user)
        {
            string jwtKey =
                _configuration["AppSettings:Secret"]
                ?? throw new InvalidOperationException("AppSettings Secret is not configured");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["AppSettings:Issuer"],
                audience: _configuration["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
