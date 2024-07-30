using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapbackend.Dto;
using webapbackend.Interface;
using webapbackend.Models;

namespace webapbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize(Policy ="AdminPolicy")]
    public class UserController : Controller
    {
        private readonly IUser _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserController(IUser userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [AllowAnonymous]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        [HttpGet("{UserId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public IActionResult GetUser(int UserId)
        {
            if (!_userRepository.UserExists(UserId))
                return NotFound();

            var user = _mapper.Map<UserDto>(_userRepository.GetUser(UserId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UserDto userCreate)
        {
            if (userCreate == null)
                return BadRequest(ModelState);

            var users = _userRepository.GetUsers()
                .Where(c => c.Id == userCreate.Id)
                .FirstOrDefault();

            if (users != null)
            {
                ModelState.AddModelError("", "User already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userCreate);

            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{UserId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(int UserId, [FromBody] UserDto updatedUser)
        {
            if (updatedUser == null)
                return BadRequest(ModelState);

            if (UserId != updatedUser.Id)
                return BadRequest(ModelState);

            if (!_userRepository.UserExists(UserId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var UserMap = _mapper.Map<User>(updatedUser);

            if (!_userRepository.UpdateUser(UserMap))
            {
                ModelState.AddModelError("", "Something went wrong updating user");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult<RegisterResponse> Register(UserDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (_userRepository.UserExists(request.Email))
                return BadRequest("This email has already been registered.");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                Name = request.Name,
                Surname = request.Surname,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Password = passwordHash,
                BirthDate = request.BirthDate,
                isAdmin = request.IsAdmin
            };

            _userRepository.CreateUser(newUser);
            return Ok(newUser);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<LoginResponse> Login(LoginDto request)
        {
            var user = _userRepository.GetUserByEmail(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return BadRequest("Wrong password.");

            string token = CreateToken(user);

            LoginResponse modl = new LoginResponse()
            {
                id = user.Id,
                UserEmail = user.Email,
                Token = token,
                IsAdmin = user.isAdmin
            };
            return Ok(modl);
        }

        private string CreateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
    _configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("isAdmin", user.isAdmin.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpDelete("{UserId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int UserId)
        {
            if (!_userRepository.UserExists(UserId))
                return NotFound();

            var userToDelete = _userRepository.GetUser(UserId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting user");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        
    }
}
