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
    public class SubcribeService : ISubcribeService
    {
        private readonly IPhotoUnitOfWork _db;
        private IMapper _mapper;

        public SubcribeService(IPhotoUnitOfWork uow)
        {
            _db = uow;
            _mapper = new MappingPhotoProfile(uow).Config.CreateMapper();
        }

        public void AddComment(CommentBLL commentBll)
        {
            if (commentBll == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }
            if (String.IsNullOrEmpty(commentBll.Message))
            {
                throw new ArgumentException("Message can't be empty");
            }
            _db.Comments.Add(new Comment()
            {
                Id = Guid.NewGuid().ToString(),
                Message = commentBll.Message,
                Photo = _db.Photos.Find(p => p.Id == commentBll.PhotoId).Single(),
                User = _db.ClientManager.Find(p => p.Id == commentBll.UserId).Single()
            });

            _db.SaveAsync();
        }

        public CommentBLL GetComment(string id)
        {
            return _mapper.Map<Comment, CommentBLL>(_db.Comments.Get(id));
        }

        public IEnumerable<CommentBLL> GetCommentsByPhoto(string photoId)
        {
            var comments = _db.Comments.Find(p => p.Photo.Id == photoId);
            return _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentBLL>>(comments);
        }

        public IEnumerable<CommentBLL> GetCommentsByUser(string userId)
        {
            var comments = _db.Comments.Find(p => p.User.Id == userId);
            return _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentBLL>>(comments);
        }

        public void EditComment(CommentBLL commentBll)
        {
            if (commentBll == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }

            if (String.IsNullOrEmpty(commentBll.Message))
            {
                throw new ArgumentException("Message can't be empty");
            }

            var comment = _db.Comments.Find(p => p.Id == commentBll.Id).Single();
            comment.Id = commentBll.Id;
            comment.Message = commentBll.Message;
            comment.Photo = _db.Photos.Get(commentBll.PhotoId);
            comment.User = _db.ClientManager.Get(commentBll.UserId);

            _db.Comments.Update(comment);
            _db.SaveAsync();
        }

        public void RemoveComment(CommentBLL commentBll)
        {
            if (commentBll == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }
            var comment = _db.Comments.Find(p => p.Id == commentBll.Id).Single();
            _db.Comments.Remove(comment);
        }

        public void AddSubcriber(SubscribeBLL subscribeBll)
        {
            if (subscribeBll == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }

            _db.Subcribers.Add(new Subscribe()
            {
                Id = Guid.NewGuid().ToString(),
                Subcriber = _db.ClientManager.Find(p => p.Id == subscribeBll.SubcriberId).Single(),
                User = _db.ClientManager.Find(p => p.Id == subscribeBll.UserId).Single()
            });

            _db.SaveAsync();
        }

        public SubscribeBLL GetSubcriber(string id)
        {
            return _mapper.Map<Subscribe, SubscribeBLL>(_db.Subcribers.Get(id));
        }

        public IEnumerable<SubscribeBLL> GetSubcribersByUser(string userId)
        {
            var subcribers = _db.Subcribers.Find(p => p.Subcriber.Id == userId);
            return _mapper.Map<IEnumerable<Subscribe>, IEnumerable<SubscribeBLL>>(subcribers);
        }

        public IEnumerable<SubscribeBLL> GetUsersBySubcriber(string subcriberId)
        {
            var subcribers = _db.Subcribers.Find(p => p.User.Id == subcriberId);
            return _mapper.Map<IEnumerable<Subscribe>, IEnumerable<SubscribeBLL>>(subcribers);
        }

        public void RemoveSubcribe(SubscribeBLL subscribeBll)
        {
            if (subscribeBll == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }
            var subcribe = _db.Subcribers.Find(p => p.Id == subscribeBll.Id).Single();
            _db.Subcribers.Remove(subcribe);
        }
    }
}
