using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PhotoAlbum.BLL.EnittyBLL;
using PhotoAlbum.BLL.Interfaces;
using PhotoAlbum.WEB.Models;

namespace PhotoAlbum.WEB.Controllers
{
    public class CommentController : Controller
    {
        private IMapper _mapper;

        public CommentController(ICommentService commentService)
        {
            _mapper = new MappingMVCProfile().Config.CreateMapper();
            _commentService = commentService;
        }

        private ICommentService _commentService;

        public int GetCount(string PhotoId)
        {
            return _commentService.GetCommentsByPhoto(PhotoId).Count();
        }

        public ActionResult Comments(string id)
        {
            IEnumerable<CommentBLL> commentByPhoto = _commentService.GetCommentsByPhoto(id);
            return PartialView(_mapper.Map<IEnumerable<CommentBLL>, IEnumerable<CommentModel>>(commentByPhoto));
        }


        public ActionResult Details(string id)
        {
            return View();
        }

        public ActionResult AddComment(string id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddComment(CommentModel comment)
        {
            comment.UserId = User.Identity.GetUserId();
            _commentService.AddComment(new CommentBLL()
            {
                Message = comment.Message,
                UserId = comment.UserId,
                PhotoId = comment.PhotoId,
            });
            IEnumerable<CommentBLL> commentByPhoto = _commentService.GetCommentsByPhoto(comment.PhotoId);
            return PartialView("Comments", _mapper.Map<IEnumerable<CommentBLL>, IEnumerable<CommentModel>>(commentByPhoto));
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var comment = _commentService.GetComment(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            if (comment.UserId != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(_mapper.Map<CommentBLL, CommentModel>(comment));
        }

        [HttpPost]
        public ActionResult Edit(CommentModel comment)
        {
            try
            {
                if (!string.IsNullOrEmpty(comment.Message))
                {
                    _commentService.EditComment(new CommentBLL()
                    {
                        Message = comment.Message
                    });
                }
                return RedirectToAction("Comments", new { id = comment.PhotoId });

            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var comment = _commentService.GetComment(id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(_mapper.Map<CommentBLL, CommentModel>(comment));
        }

        [HttpPost]
        public ActionResult Delete(CommentModel comment)
        {
            try
            {
                _commentService.RemoveComment(new CommentBLL()
                {
                    Id = comment.Id,
                });

                return RedirectToAction("Comments", new { id = comment.PhotoId });
            }
            catch
            {
                return View();
            }
        }
    }
}
