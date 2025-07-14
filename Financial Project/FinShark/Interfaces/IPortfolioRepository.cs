using FinShark.Models;

namespace FinShark.Interfaces
{
	public interface IPortfolioRepository
	{
		Task<List<Stock>> GetUserPortfolios(AppUser user);
		Task<Portfolio> CreateAsync(Portfolio portfolio);
		Task<Portfolio> Delete(AppUser appUser, string symbol);
	}
}
