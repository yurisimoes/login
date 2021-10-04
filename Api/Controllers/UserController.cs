using System.Threading.Tasks;
using Api.Data;
using Api.Dto;
using Api.Entities;
using Isopoh.Cryptography.Argon2;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly LoginDbContext _context;

        public UserController(LoginDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Post(CreateUserDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Hash = Argon2.Hash(dto.Password, 1)
            };
            _context.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var user = await _context.Users.ProjectToType<UserResult>().ToListAsync();
            return Ok(user);
        }
    }
}