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
        // public async Task<IActionResult<User>> GetUserById(int id)
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
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, null);
        }


        // login
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {

            var user = await _userService.(model.Username, model.Password);

            if(!user){
                return Unauthorized();
            }

            var token = GenerateJwtToken(user);
            return Ok(new{Token = token});



            //   var user = await _userService.Authenticate(model.Username, model.Password);
            // if (user == null)
            //     return Unauthorized();

            // var token = GenerateJwtToken(user);  // This method will create the JWT

            // return Ok(new { Token = token });
        }
    }
}