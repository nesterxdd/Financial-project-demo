﻿using System.ComponentModel.DataAnnotations;

namespace FinShark.Dtos.CommentDto
{
	public class CreateCommentDto
	{
		[Required]
		[MinLength(5,ErrorMessage = "Title must be at least 5 characters")]
		[MaxLength(280, ErrorMessage = "Title must be less than 280 characters")]
		public string Title { get; set; } = string.Empty;
		[Required]
		[MinLength(5, ErrorMessage = "Content must be at least 5 characters")]
		[MaxLength(280, ErrorMessage = "Content must be less than 280 characters")]
		public string Content { get; set; } = string.Empty;
	}
}
