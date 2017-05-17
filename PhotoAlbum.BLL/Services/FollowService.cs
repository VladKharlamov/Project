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
    public class FollowService : IFollowService
    {
        private readonly IPhotoUnitOfWork _db;
        private IMapper _mapper;

        public FollowService(IPhotoUnitOfWork uow)
        {
            _db = uow;
            _mapper = new MappingPhotoProfile(uow).Config.CreateMapper();
        }

        public void AddFollower(FollowBLL followBll)
        {
            if (followBll == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }

            _db.Followers.Add(new Follow()
            {
                Id = Guid.NewGuid().ToString(),
                Follower = _db.ClientManager.Find(p => p.Id == followBll.FollowerId).Single(),
                User = _db.ClientManager.Find(p => p.Id == followBll.UserId).Single()
            });

            _db.Save();
        }

        public FollowBLL GetFollower(string id)
        {
            return _mapper.Map<Follow, FollowBLL>(_db.Followers.Get(id));
        }
        public FollowBLL GetFollowerByUser(string userId, string followerId)
        {
            var follow = _db.Followers.Find(p => (p.User.Id == userId) && (p.Follower.Id == followerId)).SingleOrDefault();
            return _mapper.Map<Follow, FollowBLL>(follow);
        }
        public IEnumerable<FollowBLL> GetFollowersByUser(string userId)
        {
            var subcribers = _db.Followers.Find(p => p.User.Id == userId);
            return _mapper.Map<IEnumerable<Follow>, IEnumerable<FollowBLL>>(subcribers);
        }

        public IEnumerable<FollowBLL> GetUsersByFollower(string subcriberId)
        {
            var subcribers = _db.Followers.Find(p => p.User.Id == subcriberId);
            return _mapper.Map<IEnumerable<Follow>, IEnumerable<FollowBLL>>(subcribers);
        }

        public void RemoveFollower(FollowBLL followBll)
        {
            if (followBll == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }
            var subcribe = _db.Followers.Find(p => p.Id == followBll.Id).Single();
            _db.Followers.Remove(subcribe);
            _db.Save();

        }
    }
}
