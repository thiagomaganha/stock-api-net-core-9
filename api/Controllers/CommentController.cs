using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [ApiController]
    [Route("api/comment")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }


        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<CommentDto>>> Get()
        {
            var comments = await _commentRepo.GetAllAsync();
            return Ok(comments.Select(c => c.ToCommentDto()));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CommentDto>> Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepo.GetAsync(id);
            if (comment == null)
                return NotFound();
            return Ok(comment.ToCommentDto());
        }


        [HttpPost("{stockId:int}")]
        public async Task<ActionResult<CommentDto>> Create(int stockId, [FromBody] CreateCommentRequestDto comment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exists = await _stockRepo.StockExistsAsync(stockId);
            if (!exists)
                return BadRequest("Stock does not exist");

            var commentModel = comment.ToCommentFromRequestDto(stockId);
            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(Get), new { id = commentModel.Id }, commentModel.ToCommentDto());

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CommentDto>> Update(int id, [FromBody] UpdateCommentRequestDto comment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exists = await _commentRepo.CommentExistsAsync(id);
            if (!exists)
                return BadRequest("Comment does not exist");

            var commentModel = await _commentRepo.UpdateAsync(id, comment.ToCommentFromUpdateRequestDto(id));
            if (commentModel == null)
                return NotFound();

            return Ok(commentModel.ToCommentDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var comment = await _commentRepo.DeleteAsync(id);
            if (comment == null)
                return NotFound("Comment does not exist");
            
            return NoContent();
        }

    }
}