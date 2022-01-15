﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Recipe.Models;
using Recipe.Models.Dtos;
using Recipe.Repositories.IRepositories;
using System.Collections.Generic;

namespace Recipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : Controller
    {
        private ILikeRepository _likeRepository;
        private readonly IMapper _mapper;

        public LikeController(ILikeRepository likeRepository, IMapper mapper)
        {
            _likeRepository = likeRepository;
            _mapper = mapper;
        }

        [HttpGet]
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

        [HttpGet("{likeId:int}")]
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

            var likeObj = _mapper.Map<Like>(likeDto);
            if (!_likeRepository.CreateLike(likeObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {likeObj.RecipeId}");
                return StatusCode(500, ModelState);
            }

            //return Ok();
            return CreatedAtRoute("GetLike", new { likeId = likeObj.Id }, likeObj);
        }
    }
}
