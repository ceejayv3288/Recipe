using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetNationalParks()
        {
            var objList = _commentRepository.GetComments();

            var objDto = new List<CommentDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<CommentDto>(obj));
            }

            return Ok(objDto);
        }

        [HttpGet("{commentId:int}")]
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
    }
}
