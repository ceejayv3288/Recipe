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
    public class RecipeController : Controller
    {
        private IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public RecipeController(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetRecipes()
        {
            var objList = _recipeRepository.GetRecipes();

            var objDto = new List<RecipeDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<RecipeDto>(obj));
            }

            return Ok(objDto);
        }

        [HttpGet("{recipeId:int}")]
        public IActionResult GetRecipe(int recipeId)
        {
            var obj = _recipeRepository.GetRecipe(recipeId);

            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<RecipeDto>(obj);

            return Ok(objDto);
        }

        [HttpPost]
        public IActionResult CreateRecipe([FromBody] RecipeDto recipeDto)
        {
            if (recipeDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_recipeRepository.RecipeExists(recipeDto.Id))
            {
                ModelState.AddModelError("", "The recipe already exist!");
                return StatusCode(404, ModelState);
            }

            var recipeObj = _mapper.Map<RecipeModel>(recipeDto);
            if (!_recipeRepository.CreateRecipe(recipeObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {recipeObj.Name}");
                return StatusCode(500, ModelState);
            }

            //return Ok();
            return CreatedAtRoute("GetRecipe", new { recipeId = recipeObj.Id }, recipeObj);
        }

        [HttpPatch("{recipeId:int}", Name = "UpdateRecipe")]
        public IActionResult UpdateRecipe(int recipeId, [FromBody] RecipeDto recipeDto)
        {
            if (recipeDto == null || recipeId != recipeDto.Id)
            {
                return BadRequest(ModelState);
            }

            var recipeObj = _mapper.Map<RecipeModel>(recipeDto);
            if (!_recipeRepository.UpdateRecipe(recipeObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {recipeObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
