using FinShark.Dtos.CommentDto;
using FinShark.Models;

namespace FinShark.Mappers
{
	public static class CommentMapper
	{
		public static CommentDto ToCommentDto(this Comment commentModel)
		{
			return new CommentDto
			{
				Id = commentModel.Id,
				Title = commentModel.Title,
				Content = commentModel.Content,
				CreatedOn = commentModel.CreatedOn,
				StockId = commentModel.StockId,
				CreatedBy = commentModel.AppUser.UserName
			};
		}

		public static Comment FromCreateCommentDto(this CreateCommentDto createCommentDto, int stockId)
		{
			return new Comment
			{
				StockId = stockId,
				Title = createCommentDto.Title,
				Content = createCommentDto.Content,
			};
		}

		public static Comment FromUpdateToCommentDto(this UpdateCommentDto updateCommentDto)
		{
			return new Comment
			{
				Title = updateCommentDto.Title,
				Content = updateCommentDto.Content,
			};
		}


	}
}
