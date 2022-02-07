using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe.Models;
using Recipe.Models.Dtos;
using Recipe.Repositories.IRepositories;
using System.Collections.Generic;

namespace Recipe.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/recipes")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class RecipeController : ControllerBase
    {
        private IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public RecipeController(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of recipes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<RecipeDto>))]
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

        /// <summary>
        /// Get individual recipe
        /// </summary>
        /// <param name="recipeId"> The Id of the recipe</param>
        /// <returns></returns>
        [HttpGet("{recipeId:int}", Name = "GetRecipe")]
        [ProducesResponseType(200, Type = typeof(RecipeDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
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
        [ProducesResponseType(201, Type = typeof(RecipeDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        [HttpPut("{recipeId:int}", Name = "UpdateRecipe")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        [HttpDelete("{recipeId:int}", Name = "DeleteRecipe")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteRecipe(int recipeId)
        {
            if (!_recipeRepository.RecipeExists(recipeId))
            {
                return NotFound();
            }

            var recipeObj = _recipeRepository.GetRecipe(recipeId);
            if (!_recipeRepository.DeleteRecipe(recipeObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {recipeObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
