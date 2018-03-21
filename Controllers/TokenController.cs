using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ArtMapApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ArtMapApi.Models;

namespace ArtMapApi
{

    [Route("/api/token")]
    public class TokenController : Controller
    {
        private ApplicationDbContext _context;
        private readonly SignInManager<User> _signInManager;


        public TokenController(ApplicationDbContext ctx, SignInManager<User> signInManager)
        {
            _context = ctx;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get(){
            return new ObjectResult(new {
                Email = User.Identity.Name
            });
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put(){

            return new ObjectResult(GenerateToken(User.Identity.Name));
        }

        [HttpPost]
        public async Task<IActionResult> Create(string username, string password, string email)
        {
            // // Check simplistic username and password validation rules

            bool isValid = IsValidUserAndPasswordCombination(email, password);

            if (isValid)
            {
                // Does the user already exist?
                User user = _context.User.SingleOrDefault(u => u.Email == email);

                if (user != null)
                {
                    // Found the user, verify credentials
                    var result = await _signInManager.PasswordSignInAsync(user, password, false, lockoutOnFailure: false);

                    // Password is correct, generate token and return it as well as the user id to be stored in local storage
                    if (result.Succeeded)
                    {
                        var tokenKeys = new {

                            Token = GenerateToken(user.Email),
                            UserId = user.Id,
                            email = user.Email


                        };

                        var localStoreObject = Newtonsoft.Json.JsonConvert.SerializeObject(tokenKeys);

                        return new ObjectResult(localStoreObject);
                    };
                } else
                {
                    var userstore = new UserStore<User>(_context);

                    // User does not exist, create one
                    user = new User {
                        UserName = username,
                        NormalizedUserName = username.ToUpper(),
                        Email = email,
                        NormalizedEmail = email.ToUpper(),
                        EmailConfirmed = true,
                        LockoutEnabled = false,
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    };
                    var passwordHash = new PasswordHasher<User>();
                    user.PasswordHash = passwordHash.HashPassword(user, password);
                    await userstore.CreateAsync(user);
                    // await userstore.AddToRoleAsync(user);
                    _context.SaveChanges();
                    return new ObjectResult(GenerateToken(user.Email));
                }
            }
            return BadRequest();
        }

        private bool IsValidUserAndPasswordCombination(string email, string password)
        {
            return !string.IsNullOrEmpty(email) && email != password;
        }

        private string GenerateToken(string email)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
                // new Claim(ClaimTypes.Role),
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("7A735D7B-1A19-4D8A-9CFA-99F55483013F")),
                        SecurityAlgorithms.HmacSha256)
                    ),
                new JwtPayload(claims)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}