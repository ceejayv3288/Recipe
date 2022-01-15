using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetNationalParks()
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
    }
}
