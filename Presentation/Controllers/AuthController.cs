using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ModsenAPI.Application.CustomExceptions;
using ModsenAPI.Application.UnitOfWorks;
using ModsenAPI.Application.Validators;
using ModsenAPI.Domain.Models;
using ModsenAPI.Domain.ModelsDTO.EventDTO;
using ModsenAPI.Domain.ModelsDTO.UserDTO;
using ModsenAPI.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ModsenAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<UserRegisterRequestDto> _userRegisterValidator;
        private readonly IValidator<UserLoginRequestDto> _userLoginValidator;

        public AuthController(ApplicationDbContext context, 
            IMapper mapper, IValidator<UserRegisterRequestDto> userValidator,
            IValidator<UserLoginRequestDto> userLoginValidator,
            IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _userRegisterValidator = userValidator;
            _userLoginValidator=userLoginValidator;
            _configuration=configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequestDto userRegisterDto)
        {
            var validationResult = _userRegisterValidator.Validate(userRegisterDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userRegisterDto.UserName);
            if (existingUser != null)
            {
                throw new AlreadyExistsException("Пользователь с таким логином уже существует");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.Password);

            var newUser = _mapper.Map<User>(userRegisterDto);

            newUser.PasswordHash = passwordHash;
            newUser.Role = userRegisterDto.Role;


            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var newUserResponse = _mapper.Map<UserResponseDto>(newUser);

            return Ok(newUserResponse);
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto userLoginDto)
        {
            var validationResult = _userLoginValidator.Validate(userLoginDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userLoginDto.UserName);
            if (existingUser == null)
            {
                throw new NotFoundException("Пользователь с таким логином не существует");
            }

            if (!BCrypt.Net.BCrypt.Verify(userLoginDto.Password, existingUser.PasswordHash))
            {
                throw new BadRequestException("Неверный пароль");
            }

            var accessToken = CreateToken(existingUser);
            var refreshToken = GenerateRefreshToken();

            existingUser.RefreshToken = refreshToken;

            await _context.SaveChangesAsync();

            return Ok(new { accessToken, refreshToken, existingUser.Role });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            
            // Проверка refresh токена
            var user = await _context.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user == null)
            {
                throw new BadRequestException("Неверный refresh токен");
            }

            var accessToken = CreateToken(user);
            var _refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            await _context.SaveChangesAsync();

            return Ok(new { accessToken, refreshToken });
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Secret").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }
    }
}
