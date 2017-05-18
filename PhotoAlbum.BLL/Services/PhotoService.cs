using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PhotoAlbum.BLL.EnittyBLL;
using PhotoAlbum.BLL.Infrastructure;
using PhotoAlbum.BLL.Interfaces;
using PhotoAlbum.DAL.Entities;
using PhotoAlbum.DAL.Interfaces;

namespace PhotoAlbum.BLL.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoUnitOfWork _db;
        private IMapper _mapper;

        public PhotoService(IPhotoUnitOfWork uow)
        {
            _db = uow;
            _mapper = new MappingPhotoProfile(uow).Config.CreateMapper();
        }

        public void AddPhoto(UserPhotoBLL userPhotoBll)
        {
            if (userPhotoBll == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }
            _db.Photos.Add(new UserPhoto()
            {
                Id = Guid.NewGuid().ToString(),
                PhotoAddress = userPhotoBll.PhotoAddress,
                IsBlocked = userPhotoBll.IsBlocked,
                User = _db.UserRepository.Find(p=>p.Id == userPhotoBll.UserId).Single(),
                Description =userPhotoBll.Description,
                Date = DateTime.Now
            });

            _db.SaveAsync();
        }

        public UserPhotoBLL GetPhoto(string id)
        {
            return _mapper.Map<UserPhoto,UserPhotoBLL>(_db.Photos.Get(id));
        }

        public IEnumerable<UserPhotoBLL> GetPhotos()
        {
            return _db.Photos.GetAll().Select(_mapper.Map<UserPhoto, UserPhotoBLL>);
        }

        public IEnumerable<UserPhotoBLL> GetPhotosByUser(string userId)
        {
            var photos = _db.Photos.Find(p => p.User.Id == userId);
            return _mapper.Map<IEnumerable<UserPhoto>, IEnumerable<UserPhotoBLL>>(photos);
        }
        public IEnumerable<UserPhotoBLL> GetPhotosBySearch(string search)
        {
            var photos = _db.Photos.Find(p => p.Description.Contains(search));
            return _mapper.Map<IEnumerable<UserPhoto>, IEnumerable<UserPhotoBLL>>(photos);
        }
        public void EditPhoto(UserPhotoBLL userPhotoBll)
        {
            if (userPhotoBll == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }

            var photo = _db.Photos.Find(p => p.Id == userPhotoBll.Id).Single();
            photo.Id = userPhotoBll.Id;
            photo.IsAvatar = userPhotoBll.IsAvatar;
            photo.IsBlocked = userPhotoBll.IsBlocked;
            photo.Description = userPhotoBll.Description;

            _db.Photos.Update(photo);
            _db.Save();
        }

        public void RemovePhoto(UserPhotoBLL userPhotoBll)
        {
            if (userPhotoBll == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }
            var photo = _db.Photos.Find(p => p.Id == userPhotoBll.Id).Single();
            _db.Photos.Remove(photo);
            _db.Save();
        }
    }
}
