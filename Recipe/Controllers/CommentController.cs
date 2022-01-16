using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Recipe.Models;
using Recipe.Models.Dtos;
using Recipe.Repositories.IRepositories;
using System.Collections.Generic;

namespace Recipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : Controller
    {
        private ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentController(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetComments()
        {
            var objList = _commentRepository.GetComments();

            var objDto = new List<CommentDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<CommentDto>(obj));
            }

            return Ok(objDto);
        }

        [HttpGet("{commentId:int}", Name = "GetComment")]
        public IActionResult GetComment(int commentId)
        {
            var obj = _commentRepository.GetComment(commentId);

            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<CommentDto>(obj);

            return Ok(objDto);
        }

        [HttpPost]
        public IActionResult CreateComment([FromBody] CommentDto commentDto)
        {
            if (commentDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_commentRepository.CommentExists(commentDto.Id))
            {
                ModelState.AddModelError("", "The comment already exist!");
                return StatusCode(404, ModelState);
            }

            var commentObj = _mapper.Map<Comment>(commentDto);
            if (!_commentRepository.CreateComment(commentObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {commentObj.Description}");
                return StatusCode(500, ModelState);
            }

            //return Ok();
            return CreatedAtRoute("GetComment", new { commentId = commentObj.Id }, commentObj);
        }

        [HttpPatch("{commentId:int}", Name = "UpdateComment")]
        public IActionResult UpdateComment(int commentId, [FromBody] CommentDto commentDto)
        {
            if (commentDto == null || commentId != commentDto.Id)
            {
                return BadRequest(ModelState);
            }

            var commentObj = _mapper.Map<Comment>(commentDto);
            if (!_commentRepository.UpdateComment(commentObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {commentObj.Description}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{commentId:int}", Name = "DeleteComment")]
        public IActionResult DeleteComment(int commentId)
        {
            if (!_commentRepository.CommentExists(commentId))
            {
                return NotFound();
            }

            var commentObj = _commentRepository.GetComment(commentId);
            if (!_commentRepository.DeleteComment(commentObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {commentObj.Description}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
