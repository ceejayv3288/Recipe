using AutoMapper;
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
    [Route("api/v1/recipeIngredient")]
    [ApiController]
    public class RecipeIngredientController : Controller
    {
        private IRecipeIngredientRepository _recipeIngredientRepository;
        private readonly IMapper _mapper;

        public RecipeIngredientController(IRecipeIngredientRepository recipeIngredientRepository, IMapper mapper)
        {
            _recipeIngredientRepository = recipeIngredientRepository;
            _mapper = mapper;
        }

        [HttpGet]
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
