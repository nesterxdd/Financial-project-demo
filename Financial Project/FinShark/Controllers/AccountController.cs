using FinShark.Dtos.AccountDto;
using FinShark.Interfaces;
using FinShark.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Controllers
{
	[Route("api/account")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenService _tokenService;
		public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_tokenService = tokenService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto register)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest();
				}
				var appUser = new AppUser
				{
					UserName = register.UserName,
					Email = register.Email
				};

				var createdUser = await _userManager.CreateAsync(appUser, register.Password);
				if (createdUser.Succeeded)
				{
					var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
					if (roleResult.Succeeded)
					{
						return Ok(
							new NewUserDto
							{
								UserName = appUser.UserName,
								Email = appUser.Email,
								Token = _tokenService.CreateToken(appUser)
							});
					}
					else
					{
						return StatusCode(500, roleResult);
					}
				}
				else
				{
					return StatusCode(500, createdUser);
				}
			}
			catch (Exception e)
			{
				return StatusCode(500, e);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginDto loginDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.UserName.ToLower());

			if (user == null)
			{
				return Unauthorized("No user with this username");
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

			if (!result.Succeeded)
			{
				return Unauthorized("Invalid credentials");
			}

			return Ok
			(
				new NewUserDto
				{
					UserName = user.UserName,
					Email = user.Email,
					Token = _tokenService.CreateToken(user)
				});

		}
	}
}
