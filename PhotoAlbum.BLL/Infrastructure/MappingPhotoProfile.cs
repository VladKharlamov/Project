﻿using System.Linq;
using AutoMapper;
using PhotoAlbum.BLL.EnittyBLL;
using PhotoAlbum.DAL.Entities;
using PhotoAlbum.DAL.Interfaces;

namespace PhotoAlbum.BLL.Infrastructure
{
    public class MappingPhotoProfile : Profile
    {
            public MapperConfiguration Config { get; set; }

            public MappingPhotoProfile(IPhotoUnitOfWork uow)
            {

                Config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ClientPhoto, UserPhotoBLL>()
                        .ForMember(dto => dto.UserId, m => m.MapFrom(cp => cp.User.Id));
    //                cfg.CreateMap<ClientProfile, UserBLL>()
    //.ForMember(dto => dto.Avatar, m => m.MapFrom(cp => cp.Photos.FirstOrDefault(p=>p.PhotoAddress).PhotoAddress));

                    cfg.CreateMap<Like, LikeBLL>()
                    .ForMember(dto => dto.UserId, m => m.MapFrom(cp => cp.User.Id))
                    .ForMember(dto => dto.PhotoId, m => m.MapFrom(cp => cp.Photo.Id));

                    cfg.CreateMap<Subcribe, SubcribeBLL>()
                        .ForMember(dto => dto.UserId, m => m.MapFrom(cp => cp.User.Id))
                        .ForMember(dto => dto.SubcriberId, m => m.MapFrom(cp => cp.User.Id));


                    cfg.CreateMap<Comment, CommentBLL>()
                    .ForMember(dto => dto.UserId, m => m.MapFrom(cp => cp.User.Id))
                    .ForMember(dto => dto.PhotoId, m => m.MapFrom(cp => cp.Photo.Id));
                });
        }
    }
}
