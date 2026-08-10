using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_example.DTOs.Comment;
using api_example.Models;
using Npgsql.Replication;

namespace api_example.Mappers
{
    public static class CommentMapper
    {
         public static CommentDTO toCommentDto(this Comment commentModel)
        {
            return new CommentDTO
             {
                 Id = commentModel.Id,
                 Title = commentModel.Title,
                 Content = commentModel.Content,
                 CreatedOn = commentModel.CreatedOn,
                 CreatedBy = commentModel.AppUser.UserName,
                 StockId = commentModel.StockId
             };
        }
         public static Comment toCommentFromCreate(this CreateCommentDto  commentDto, int stockId)
        {
             return new Comment
             {
                 Title = commentDto.Title,
                 Content = commentDto.Content,
                 StockId = stockId
             };
        }
         public static Comment toCommentFromUpdate(this UpdateCommentRequestDto  commentDto)
        {
             return new Comment
             {
                 Title = commentDto.Title,
                 Content = commentDto.Content,
             };
        }
    }
}