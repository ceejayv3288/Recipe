using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Recipe.Repositories.IRepositories;

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
    }
}
