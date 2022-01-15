using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Recipe.Repositories.IRepositories;

namespace Recipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : Controller
    {
        private ILikeRepository _likeRepository;
        private readonly IMapper _mapper;

        public LikeController(ICommentRepository likeRepository, IMapper mapper)
        {
            _likeRepository = likeRepository;
            _mapper = mapper;
        }
    }
}
