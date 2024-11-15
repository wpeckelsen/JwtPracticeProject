using JwtPracticeProject.Models;
using JwtPracticeProject.Service;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace JwtPracticeProject.Controller
{


    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;


        public UserController(IUserService userService)
        {
            _userService = userService;
        }



        // get method

        [HttpGet("{id}")]

        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var foundUser = await _userService.GetUserByIdAsync(id);

            if (foundUser == null)
            {
                return NotFound();
            }
            else return Ok(foundUser);

        }
        // post
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUser createUser)
        {
            User user = await _userService.CreateUserAsync(createUser.Username, createUser.PlainPassword);

            CreatedUser createdUser = new CreatedUser
            {
                Username = user.Username,
                Role = user.Role,
                Id = user.Id                                    
            };


            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }


        // login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var token = await _userService.Authenticate(model.Email, model.Password);
            if (token == null)
            { return Unauthorized(); }

            // var token = _userService.GenerateJwtToken(user);  // This method will create the JWT

            return Ok(new { Token = token });
        }
    }
}