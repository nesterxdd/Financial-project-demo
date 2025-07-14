using System.ComponentModel.DataAnnotations;

namespace FinShark.Dtos.AccountDto
{
	public class RegisterDto
	{
		[Required]
		[MinLength(3)]
		[MaxLength(10)]
		public string? UserName { get; set; }
		[EmailAddress]
		[Required]
		public string? Email { get; set; }
		[Required]
		public string? Password { get; set; }
	}
}
