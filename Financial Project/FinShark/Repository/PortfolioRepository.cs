using FinShark.Interfaces;
using FinShark.Models;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Repository
{
	public class PortfolioRepository : IPortfolioRepository
	{
		private readonly ApplicationDBContext _context;

		public PortfolioRepository(ApplicationDBContext context)
		{
			_context = context;
		}

		public async Task<Portfolio> CreateAsync(Portfolio portfolio)
		{
			await _context.AddAsync(portfolio);
			await _context.SaveChangesAsync();
			return portfolio;
		}

		public async Task<Portfolio> Delete(AppUser appUser, string symbol)
		{
			var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(p => p.AppUserId == appUser.Id && p.Stock.Symbol.ToLower() == symbol.ToLower());
			if (portfolioModel == null)
			{
				return null;
			}

			_context.Remove(portfolioModel);
			await _context.SaveChangesAsync();
			return portfolioModel;
		}

		public async Task<List<Stock>> GetUserPortfolios(AppUser user)
		{
			return await _context.Portfolios.Where(p => p.AppUserId == user.Id).Select(portfolio => new Stock
			{
				Id = portfolio.StockId,
				Symbol = portfolio.Stock.Symbol,
				CompanyName = portfolio.Stock.CompanyName,
				Purchase = portfolio.Stock.Purchase,
				LastDiv = portfolio.Stock.LastDiv,
				Industry = portfolio.Stock.Industry,
				MarketCap = portfolio.Stock.MarketCap

			}).ToListAsync();
		}
	}
}
