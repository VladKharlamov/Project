using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PhotoAlbum.BLL.EnittyBLL;
using PhotoAlbum.BLL.Infrastructure;
using PhotoAlbum.BLL.Interfaces;
using PhotoAlbum.DAL.Entities;
using PhotoAlbum.DAL.Interfaces;
using System.Data.Entity;

namespace PhotoAlbum.BLL.Services
{
    public class LikeService : ILikeService
    {
        private readonly IPhotoUnitOfWork _db;
        private IMapper _mapper;

        public LikeService(IPhotoUnitOfWork uow)
        {
            _db = uow;
            _mapper = new MappingPhotoProfile(uow).Config.CreateMapper();
        }

        public LikeBLL GetLikeByUserToPhoto(string userId, string photoId)
        {
            var like = _db.Likes.Find(p => (p.User.Id == userId) && (p.Photo.Id == photoId)).SingleOrDefault();
            return _mapper.Map<Like, LikeBLL>(like);
        }

        public IEnumerable<LikeBLL> GetAllLikesByUser(string userId)
        {
            var likes = _db.Likes.Find(p => p.User.Id == userId);
            return _mapper.Map<IEnumerable<Like>, IEnumerable<LikeBLL>>(likes);
        }

        public IEnumerable<LikeBLL> GetLikesByPhoto(string photoId)
        {
            var likes = _db.Likes.Find(p => p.Photo.Id == photoId);
            return _mapper.Map<IEnumerable<Like>, IEnumerable<LikeBLL>>(likes);
        }
        public int GetCountLikesByPhoto(string photoId)
        {
            var likes = _db.Likes.Find(p => p.Photo.Id == photoId);
            return likes.Count();
        }

        public void AddLike(LikeBLL likeBll)
        {
            if (likeBll == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }
            _db.Likes.Add(new Like()
            {
                Id = Guid.NewGuid().ToString(),
                Photo = _db.Photos.Find(p => p.Id == likeBll.PhotoId).Single(),
                User = _db.UserRepository.Find(p => p.Id == likeBll.UserId).Single()
            });
            _db.Save();
        }

        public void RemoveLike(LikeBLL likeBll)
        {
            if (likeBll == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }
            var like = _db.Likes.Find(p => p.Id == likeBll.Id).SingleOrDefault();
            _db.Likes.Remove(like);
            _db.Save();
        }
    }
}
