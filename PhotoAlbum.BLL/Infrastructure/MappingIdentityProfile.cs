using System.Linq;
using AutoMapper;
using Microsoft.AspNet.Identity;
using MimeKit.Cryptography;
using PhotoAlbum.BLL.EnittyBLL;
using PhotoAlbum.DAL.Entities;
using PhotoAlbum.DAL.Interfaces;

namespace PhotoAlbum.BLL.Infrastructure
{
    public class MappingIdentityProfile : Profile
    {
        public MapperConfiguration Config { get; set; }

        public MappingIdentityProfile(IIdentityUnitOfWork uow)
        {

            Config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserProfile, UserBLL>()
                    .ForMember(dto => dto.Email, m => m.MapFrom(cp => cp.ApplicationUser.Email))
                    .ForMember(dto => dto.Password, m => m.MapFrom(cp => cp.ApplicationUser.PasswordHash))
                    .ForMember(dto => dto.UserName, m => m.MapFrom(cp => cp.ApplicationUser.UserName))
                                        .ForMember(dto => dto.Birthday, m => m.MapFrom(cp => cp.Birthday.Date))

                    //.ForMember(dto => dto.Avatar, m => m.MapFrom(cp => uow.UserRepository
                    //.Find(p=>p.Photos.Where(p0 => p0.IsAvatar == true)
                    //.Select(p1=>p1.PhotoAddress)
                    //.FirstOrDefault())))
                    .ForMember(dto => dto.Role,
                        m =>
                            m.MapFrom(
                                cp =>
                                    uow.RoleManager.FindById(
                                        cp.ApplicationUser.Roles.First(p2 => p2.UserId == cp.Id).RoleId).Name));
            });
        }
    }
}
