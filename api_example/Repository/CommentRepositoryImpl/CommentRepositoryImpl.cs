using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_example.Data;
using api_example.Helpers;
using api_example.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api_example.Repository.CommentRepositoryImpl
{
    public class CommentRepositoryImpl : ICommentRepository
    {
        public readonly ApplicationDBContext _applicationDbContext;
        public CommentRepositoryImpl(ApplicationDBContext applicationDBContext)
        {
            _applicationDbContext = applicationDBContext;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _applicationDbContext.Comments.AddAsync(commentModel);
            await _applicationDbContext.SaveChangesAsync();

            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var exist = await _applicationDbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if(exist == null)
            {
                return null;
            }
            _applicationDbContext.Comments.Remove(exist);
            await _applicationDbContext.SaveChangesAsync();
            return exist;
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
                return await _applicationDbContext.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(c => c.Id == id );
        }
        public async Task<List<Comment>> GetCommentsAsync(CommentQueryObject queryObject)
        {
            var comments = _applicationDbContext.Comments.Include(a => a.AppUser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                comments = comments.Where(s=>s.Stock.Symbol == queryObject.Symbol);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            ;
            if(queryObject.IsDescending == true)
            {
                comments = comments.OrderByDescending(c=>c.CreatedOn);
            }
            return await comments.ToListAsync();
        }
        async Task<Comment?> ICommentRepository.UpdateAsync(int id, Comment commendModel)
        {
            var existingStock = await _applicationDbContext.Comments.FindAsync(id);
            if(existingStock == null)
            {
                return null;
            }
            existingStock.Title = commendModel.Title;
            existingStock.Content = commendModel.Content;

            await _applicationDbContext.Comments.Include(a=>a.AppUser).FirstOrDefaultAsync(c => c.Id == id);
            await _applicationDbContext.SaveChangesAsync();
            return existingStock;
        }
    }
}