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
    public class RecipeStepController : Controller
    {
        private IRecipeStepRepository _recipeStepRepository;
        private readonly IMapper _mapper;

        public RecipeStepController(IRecipeStepRepository recipeStepRepository, IMapper mapper)
        {
            _recipeStepRepository = recipeStepRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetRecipeSteps()
        {
            var objList = _recipeStepRepository.GetRecipeSteps();

            var objDto = new List<RecipeStepDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<RecipeStepDto>(obj));
            }

            return Ok(objDto);
        }

        [HttpGet("{recipeStepId:int}", Name = "GetRecipeStep")]
        public IActionResult GetRecipeStep(int recipeStepId)
        {
            var obj = _recipeStepRepository.GetRecipeStep(recipeStepId);

            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<RecipeStepDto>(obj);

            return Ok(objDto);
        }

        [HttpPost]
        public IActionResult CreateRecipeStep([FromBody] RecipeStepDto recipeStepDto)
        {
            if (recipeStepDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_recipeStepRepository.RecipeStepExists(recipeStepDto.Id))
            {
                ModelState.AddModelError("", "The recipe steps already exist!");
                return StatusCode(404, ModelState);
            }

            var recipeStepObj = _mapper.Map<RecipeStep>(recipeStepDto);
            if (!_recipeStepRepository.CreateRecipeStep(recipeStepObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {recipeStepObj.Description}");
                return StatusCode(500, ModelState);
            }

            //return Ok();
            return CreatedAtRoute("GetRecipeStep", new { recipeStepId = recipeStepObj.Id }, recipeStepObj);
        }

        [HttpPatch("{recipeStepId:int}", Name = "UpdateRecipeStep")]
        public IActionResult UpdateRecipeStep(int recipeStepId, [FromBody] RecipeStepDto recipeStepDto)
        {
            if (recipeStepDto == null || recipeStepId != recipeStepDto.Id)
            {
                return BadRequest(ModelState);
            }

            var recipeStepObj = _mapper.Map<RecipeStep>(recipeStepDto);
            if (!_recipeStepRepository.UpdateRecipeStep(recipeStepObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {recipeStepObj.Description}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{recipeStepId:int}", Name = "DeleteRecipeStep")]
        public IActionResult DeleteRecipeStep(int recipeStepId)
        {
            if (!_recipeStepRepository.RecipeStepExists(recipeStepId))
            {
                return NotFound();
            }

            var recipeStepObj = _recipeStepRepository.GetRecipeStep(recipeStepId);
            if (!_recipeStepRepository.DeleteRecipeStep(recipeStepObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {recipeStepObj.Description}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
