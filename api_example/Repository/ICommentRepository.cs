using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_example.Models;

namespace api_example.Repository
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetCommentsAsync();

        Task<Comment?> GetByIdAsync(int id);

        Task<Comment>CreateAsync(Comment commentModel);

        Task<Comment?>UpdateAsync(int id,Comment commendModel);

        Task<Comment?>DeleteAsync(int id);
        
    }
}