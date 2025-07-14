using FinShark.Dtos.CommentDto;
using FinShark.Interfaces;
using FinShark.Mappers;
using FinShark.Models;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Repository
{
	public class CommentRepository : ICommentRepository
	{
		private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
			_context = context;
        }

		public async Task<Comment> CreateAsync(Comment commentModel)
		{
			await _context.AddAsync(commentModel);
			await _context.SaveChangesAsync();
			return commentModel;
		}

		public async Task<List<Comment>> GetAllAsync()
		{
			//return await _context.Comments.ToListAsync();
			return await _context.Comments.Include(a => a.AppUser).ToListAsync();
		}

		public async Task<Comment?> GetByIdAsync(int id)
		{
			return await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
		{
			var existingComment = await _context.Comments.FindAsync(id);

			if(existingComment == null)
			{
				return null;
			}

			existingComment.Title = commentModel.Title;
			existingComment.Content = commentModel.Content;
			await _context.SaveChangesAsync();
			return existingComment;
		}

		public async Task<Comment?> DeleteAsync(int id)
		{
			var comment = await _context.Comments.FindAsync(id);
			if(comment == null)
			{
				return null;
			}
			_context.Remove(comment);
			await _context.SaveChangesAsync();
			return comment;
		}
	}
}
