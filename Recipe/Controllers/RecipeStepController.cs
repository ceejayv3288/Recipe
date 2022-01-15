using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetNationalParks()
        {
            var objList = _recipeStepRepository.GetRecipeSteps();

            var objDto = new List<RecipeStepDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<RecipeStepDto>(obj));
            }

            return Ok(objDto);
        }

        [HttpGet("{recipeStepId:int}")]
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
    }
}
