using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe.Models;
using Recipe.Models.Dtos;
using Recipe.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipe.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/recipeIngredients")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin, Member, Tester")]
    public class RecipeIngredientController : ControllerBase
    {
        private IRecipeIngredientRepository _recipeIngredientRepository;
        private readonly IMapper _mapper;

        public RecipeIngredientController(IRecipeIngredientRepository recipeIngredientRepository, IMapper mapper)
        {
            _recipeIngredientRepository = recipeIngredientRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<RecipeIngredientDto>))]
        public IActionResult GetRecipeIngredients()
        {
            var objList = _recipeIngredientRepository.GetRecipeIngredients();

            var objDto = new List<RecipeIngredientDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<RecipeIngredientDto>(obj));
            }

            return Ok(objDto);
        }

        [HttpGet("{recipeIngredientId:int}", Name = "GetRecipeIngredient")]
        [ProducesResponseType(200, Type = typeof(RecipeIngredientDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetRecipeIngredient(int recipeIngredientId)
        {
            var obj = _recipeIngredientRepository.GetRecipeIngredient(recipeIngredientId);

            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<RecipeIngredientDto>(obj);

            return Ok(objDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(RecipeIngredientDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateRecipeIngredient([FromBody] RecipeIngredientDto recipeIngredientDto)
        {
            if (recipeIngredientDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_recipeIngredientRepository.RecipeIngredientExists(recipeIngredientDto.Id))
            {
                ModelState.AddModelError("", "The recipe Ingredient already exist!");
                return StatusCode(404, ModelState);
            }

            var recipeIngredientObj = _mapper.Map<RecipeIngredient>(recipeIngredientDto);
            if (!_recipeIngredientRepository.CreateRecipeIngredient(recipeIngredientObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {recipeIngredientObj.Description}");
                return StatusCode(500, ModelState);
            }

            //return Ok();
            return CreatedAtRoute("GetRecipeIngredient", new { recipeIngredientId = recipeIngredientObj.Id }, recipeIngredientObj);
        }

        [HttpPut("{recipeIngredientId:int}", Name = "UpdateRecipeIngredient")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateRecipeIngredient(int recipeIngredientId, [FromBody] RecipeIngredientDto recipeIngredientDto)
        {
            if (recipeIngredientDto == null || recipeIngredientId != recipeIngredientDto.Id)
            {
                return BadRequest(ModelState);
            }

            var recipeIngredientObj = _mapper.Map<RecipeIngredient>(recipeIngredientDto);
            if (!_recipeIngredientRepository.UpdateRecipeIngredient(recipeIngredientObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {recipeIngredientObj.Description}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{recipeIngredientId:int}", Name = "DeleteRecipeIngredient")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteRecipeIngredient(int recipeIngredientId)
        {
            if (!_recipeIngredientRepository.RecipeIngredientExists(recipeIngredientId))
            {
                return NotFound();
            }

            var recipeIngredientObj = _recipeIngredientRepository.GetRecipeIngredient(recipeIngredientId);
            if (!_recipeIngredientRepository.DeleteRecipeIngredient(recipeIngredientObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {recipeIngredientObj.Description}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
