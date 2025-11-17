using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                StockId = comment.StockId
            };
        }

        public static Comment ToCommentFromRequestDto(this CreateCommentRequestDto comment, int id)
        {
            return new Comment
            {
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = DateTime.Now,
                StockId = id
            };
        }

        public static Comment ToCommentFromUpdateRequestDto(this UpdateCommentRequestDto comment, int id)
        {
            return new Comment
            {
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = DateTime.Now,
                StockId = id
            };
        }
    }
}