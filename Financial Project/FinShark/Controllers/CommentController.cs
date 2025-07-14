using FinShark.Dtos.CommentDto;
using FinShark.Extensions;
using FinShark.Interfaces;
using FinShark.Mappers;
using FinShark.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.Controllers
{
	[Route("api/comment")]
	[ApiController]
	public class CommentController : ControllerBase
	{
		private readonly ICommentRepository _commentRepo;
		private readonly IStockRepository _stockRepo;
		private readonly UserManager<AppUser> _userManager;
		public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
		{
			_commentRepo = commentRepo;
			_stockRepo = stockRepo;
			_userManager = userManager;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var comments = await _commentRepo.GetAllAsync();

			var commentDto = comments.Select(c => c.ToCommentDto());

			return Ok(commentDto);
		}

		[HttpGet("{id:int}", Name = "GetCommentById")]
		public async Task<IActionResult> GetById([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var comment = await _commentRepo.GetByIdAsync(id);
			if (comment == null)
			{
				return NotFound();
			}
			return Ok(comment.ToCommentDto());

		}

		[HttpPost("{stockId:int}")]
		public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (!await _stockRepo.StockExists(stockId))
			{
				return BadRequest("Stock does not exist");
			}

			var username = User.GetUsername();
			var appUser = await _userManager.FindByNameAsync(username);

			var commentModel = commentDto.FromCreateCommentDto(stockId);

			commentModel.AppUserId = appUser.Id;
			await _commentRepo.CreateAsync(commentModel);
			Console.WriteLine($"Comment ID after creation: {commentModel.Id}");
			return CreatedAtRoute("GetCommentById", new { id = commentModel.Id }, commentModel.ToCommentDto());

		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto commentDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var comment = await _commentRepo.UpdateAsync(id, commentDto.FromUpdateToCommentDto());
			if(comment == null)
			{
				return NotFound();
			}
			return Ok(comment.ToCommentDto());
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var comment = await _commentRepo.DeleteAsync(id);
			if(comment == null)
			{
				return NotFound("Comment does not exist");
			}

			return NoContent();

		}
	}
}
