using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task.Services;
using User.Interfaces;
namespace User.Controllers
{
    using Microsoft.Net.Http.Headers;
    using Task.Interfaces;
    using User.Models;
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        
        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] User user)
        {
            var claims = new List<Claim>();
            var getUser = userService.GetAll().FirstOrDefault(c => c.Name == user.Name && c.Password == user.Password);
            if (getUser == null)
                return Unauthorized();
            if (getUser.IsAdmin)
            {
                claims.Add(
                    new Claim("type", "Admin")
                );
            }

            claims.Add(
                new Claim("type", "User")
            );


            claims.Add(new Claim("Id", getUser.Id.ToString()));
            return new OkObjectResult(TokenService.WriteToken(TokenService.GetToken(claims)));
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public ActionResult<List<User>> GetAll() => userService.GetAll();

        [HttpGet("{id}")]
        [Authorize(Policy = "User")]
        public ActionResult<User> GetMyUser()
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var user = userService.Get(TokenService.decode(token));
            if (user == null)
                return NotFound();
            return user;
        }

        [HttpPost("{user}")]
        [Authorize(Policy = "Admin")]
        public ActionResult Post([FromBody] User user)
        {
            userService.Post(user);
            return CreatedAtAction(nameof(Post), new { Id = user.Id }, user);
        }

        [HttpDelete]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            var user = userService.Get(id);
            if (user == null)
                return NotFound();
            userService.Delete(id);
            return NoContent();
        }

    }

}