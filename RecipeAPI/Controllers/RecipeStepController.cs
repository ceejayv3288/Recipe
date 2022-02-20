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
    [Route("api/v{version:apiVersion}/recipeSteps")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin, Member, Tester")]
    public class RecipeStepController : ControllerBase
    {
        private IRecipeStepRepository _recipeStepRepository;
        private readonly IMapper _mapper;

        public RecipeStepController(IRecipeStepRepository recipeStepRepository, IMapper mapper)
        {
            _recipeStepRepository = recipeStepRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<RecipeStepDto>))]
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
        [ProducesResponseType(200, Type = typeof(RecipeStepDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
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
        [ProducesResponseType(201, Type = typeof(RecipeStepDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

            var recipeStepObj = _mapper.Map<RecipeStepModel>(recipeStepDto);
            if (!_recipeStepRepository.CreateRecipeStep(recipeStepObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {recipeStepObj.Description}");
                return StatusCode(500, ModelState);
            }

            //return Ok();
            return CreatedAtRoute("GetRecipeStep", new { recipeStepId = recipeStepObj.Id }, recipeStepObj);
        }

        [HttpPut("{recipeStepId:int}", Name = "UpdateRecipeStep")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateRecipeStep(int recipeStepId, [FromBody] RecipeStepDto recipeStepDto)
        {
            if (recipeStepDto == null || recipeStepId != recipeStepDto.Id)
            {
                return BadRequest(ModelState);
            }

            var recipeStepObj = _mapper.Map<RecipeStepModel>(recipeStepDto);
            if (!_recipeStepRepository.UpdateRecipeStep(recipeStepObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {recipeStepObj.Description}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{recipeStepId:int}", Name = "DeleteRecipeStep")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
