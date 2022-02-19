using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe.Models;
using Recipe.Models.Dtos;
using Recipe.Repositories.IRepositories;
using System.Collections.Generic;

namespace Recipe.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/comments")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin, Member, Tester")]
    public class CommentController : ControllerBase
    {
        private ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentController(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CommentDto>))]
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
        [ProducesResponseType(200, Type = typeof(CommentDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
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
        [ProducesResponseType(201, Type = typeof(CommentDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        [HttpPut("{commentId:int}", Name = "UpdateComment")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
