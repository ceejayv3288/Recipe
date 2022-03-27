using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeAPI.Models;
using RecipeAPI.Models.Dtos;
using RecipeAPI.Repositories.IRepositories;
using System.Collections.Generic;

namespace RecipeAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/likes")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin, Member, Tester")]
    public class LikeController : ControllerBase
    {
        private ILikeRepository _likeRepository;
        private readonly IMapper _mapper;

        public LikeController(ILikeRepository likeRepository, IMapper mapper)
        {
            _likeRepository = likeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<LikeDto>))]
        public IActionResult GetLikes()
        {
            var objList = _likeRepository.GetLikes();

            var objDto = new List<LikeDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<LikeDto>(obj));
            }

            return Ok(objDto);
        }

        [HttpGet("{likeId:int}", Name = "GetLike")]
        [ProducesResponseType(200, Type = typeof(LikeDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetLike(int likeId)
        {
            var obj = _likeRepository.GetLike(likeId);

            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<LikeDto>(obj);

            return Ok(objDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(LikeDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateLike([FromBody] LikeDto likeDto)
        {
            if (likeDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_likeRepository.LikeExists(likeDto.Id))
            {
                ModelState.AddModelError("", "The like already exist!");
                return StatusCode(404, ModelState);
            }

            var likeObj = _mapper.Map<LikeModel>(likeDto);
            if (!_likeRepository.CreateLike(likeObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {likeObj.RecipeId}");
                return StatusCode(500, ModelState);
            }

            //return Ok();
            return CreatedAtRoute("GetLike", new { likeId = likeObj.Id }, likeObj);
        }

        [HttpGet("recipeId/{recipeId:int}", Name = "GetLikesByRecipeId")]
        [ProducesResponseType(200, Type = typeof(List<LikeDto>))]
        public IActionResult GetLikesByRecipeId(int recipeId)
        {
            var objList = _likeRepository.GetLikesByRecipeId(recipeId);

            var objDto = new List<LikeDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<LikeDto>(obj));
            }

            return Ok(objDto);
        }

        [HttpGet("userId/{userId}", Name = "GetLikesByUserId")]
        [ProducesResponseType(200, Type = typeof(List<LikeDto>))]
        public IActionResult GetCommentsByUserId(string userId)
        {
            var objList = _likeRepository.GetLikesByUserId(userId);

            var objDto = new List<LikeDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<LikeDto>(obj));
            }

            return Ok(objDto);
        }

        [HttpGet("{recipeId:int}/{userId}", Name = "GetLikesByRecipeAndUserId")]
        [ProducesResponseType(200, Type = typeof(List<LikeDto>))]
        public IActionResult GetCommentsByRecipeAndUserId(int recipeId, string userId)
        {
            var obj = _likeRepository.GetLikesByRecipeAndUserId(recipeId, userId);

            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<LikeDto>(obj);

            return Ok(objDto);
        }

        [HttpPut("{likeId:int}", Name = "UpdateLike")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateLike(int likeId, [FromBody] LikeDto likeDto)
        {
            if (likeDto == null || likeId != likeDto.Id)
            {
                return BadRequest(ModelState);
            }

            var likeObj = _mapper.Map<LikeModel>(likeDto);
            if (!_likeRepository.UpdateLike(likeObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {likeObj.RecipeId}");
                return StatusCode(500, ModelState);
            }

            //return NoContent();
            return CreatedAtRoute("GetLike", new { likeId = likeObj.Id }, likeObj);
        }

        [HttpDelete("{likeId:int}", Name = "DeleteLike")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteLike(int likeId)
        {
            if (!_likeRepository.LikeExists(likeId))
            {
                return NotFound();
            }

            var likeObj = _likeRepository.GetLike(likeId);
            if (!_likeRepository.DeleteLike(likeObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {likeObj.RecipeId}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
