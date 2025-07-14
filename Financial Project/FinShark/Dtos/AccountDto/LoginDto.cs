using System.ComponentModel.DataAnnotations;

namespace FinShark.Dtos.AccountDto
{
	public class LoginDto
	{
		[Required]
		public string UserName { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
