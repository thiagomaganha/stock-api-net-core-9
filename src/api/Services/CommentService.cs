using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;

namespace api.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<bool> CommentExistsAsync(int id)
        {
            return await _commentRepository.CommentExistsAsync(id);
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            return await _commentRepository.CreateAsync(comment);
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            return await _commentRepository.DeleteAsync(id);
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _commentRepository.GetAllAsync();
        }

        public async Task<Comment?> GetAsync(int id)
        {
            return await _commentRepository.GetAsync(id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment comment)
        {
            return await _commentRepository.UpdateAsync(id, comment);
        }
    }
}