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
    public class CommentService : ICommentService
    {
        private readonly IPhotoUnitOfWork _db;
        private IMapper _mapper;

        public CommentService(IPhotoUnitOfWork uow)
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
                User = _db.UserRepository.Find(p => p.Id == commentBll.UserId).Single(),
                Date = DateTime.Now
            });

            _db.Save();
        }

        public CommentBLL GetComment(string id)
        {
            return _mapper.Map<Comment, CommentBLL>(_db.Comments.Get(id));
        }

        public IEnumerable<CommentBLL> GetCommentsByPhoto(string photoId)
        {
            var comments = _db.Comments.Find(p => p.PhotoId == photoId).OrderBy(x=>x.Date);
            return _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentBLL>>(comments);
        }

        public IEnumerable<CommentBLL> GetCommentsByUser(string userId)
        {
            var comments = _db.Comments.Find(p => p.UserId == userId).OrderBy(x => x.Date);
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
            comment.User = _db.UserRepository.Get(commentBll.UserId);

            _db.Comments.Update(comment);
            _db.Save();
        }

        public void RemoveComment(CommentBLL commentBll)
        {
            if (commentBll == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }
            var comment = _db.Comments.Find(p => p.Id == commentBll.Id).Single();
            _db.Comments.Remove(comment);
            _db.Save();

        }
    }
}
