using AutoMapper;
using PhotoAlbum.BLL.EnittyBLL;
using PhotoAlbum.WEB.Models;

namespace PhotoAlbum.WEB
{
    public class MappingMVCProfile : Profile
    {
        public MapperConfiguration Config { get; set; }

        public MappingMVCProfile()
        {
            Config = new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<ProductDTO, ProductViewModel>()
                //    .ForMember(vm => vm.CategoryName, m => m.MapFrom(dto => dto.CategoryName))
                //    .ForMember(vm => vm.ProviderName, m => m.MapFrom(dto => dto.ProviderName));
                cfg.CreateMap<UserBLL, UserModel>();
                cfg.CreateMap<UserPhotoBLL, UserPhotoModel>();
                cfg.CreateMap<LikeBLL, LikeModel>();
                cfg.CreateMap<CommentBLL, CommentModel>();
                cfg.CreateMap<FollowBLL, FollowModel>();
            });
        }
    }
}