using AutoMapper;
using RecipeAPI.Models;
using RecipeAPI.Models.Dtos;

namespace RecipeAPI.Mappers.RecipeMapper
{
    public class RecipeMappings : Profile
    {
        public RecipeMappings()
        {
            CreateMap<RecipeModel, RecipeDto>().ReverseMap();
            CreateMap<RecipeModel, RecipeCreateDto>().ReverseMap();
            CreateMap<CommentModel, CommentDto>().ReverseMap();
            CreateMap<CommentModel, CommentCreateDto>().ReverseMap();
            CreateMap<CommentModel, CommentUpdateDto>().ReverseMap();
            CreateMap<LikeModel, LikeDto>().ReverseMap();
            CreateMap<LikeModel, LikeCreateDto>().ReverseMap();
            CreateMap<LikeModel, LikeUpdateDto>().ReverseMap();
            CreateMap<RecipeStepModel, RecipeStepDto>().ReverseMap();
            CreateMap<RecipeStepModel, RecipeStepCreateDto>().ReverseMap();
            CreateMap<RecipeStepModel, RecipeStepUpdateDto>().ReverseMap();
            CreateMap<RecipeIngredientModel, RecipeIngredientDto>().ReverseMap();
            CreateMap<RecipeIngredientModel, RecipeIngredientCreateDto>().ReverseMap();
            CreateMap<RecipeIngredientModel, RecipeIngredientUpdateDto>().ReverseMap();
            CreateMap<UserModel, UserRegistrationModel>().ReverseMap();
        }
    }
}
