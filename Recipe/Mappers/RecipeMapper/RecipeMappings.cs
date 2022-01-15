using AutoMapper;
using Recipe.Models;
using Recipe.Models.Dtos;

namespace Recipe.Mappers.RecipeMapper
{
    public class RecipeMappings : Profile
    {
        public RecipeMappings()
        {
            CreateMap<RecipeModel, RecipeDto>().ReverseMap();
            CreateMap<RecipeModel, RecipeCreateDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Comment, CommentCreateDto>().ReverseMap();
            CreateMap<Comment, CommentUpdateDto>().ReverseMap();
            CreateMap<Like, LikeDto>().ReverseMap();
            CreateMap<Like, LikeCreateDto>().ReverseMap();
            CreateMap<Like, LikeUpdateDto>().ReverseMap();
            CreateMap<RecipeStep, RecipeStepDto>().ReverseMap();
            CreateMap<RecipeStep, RecipeStepCreateDto>().ReverseMap();
            CreateMap<RecipeStep, RecipeStepUpdateDto>().ReverseMap();
        }
    }
}
