using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
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
        private int PageSize = 5;
        private IMapper _mapper;

        public CommentController(ICommentService commentService)
        {
            _mapper = new MappingMVCProfile().Config.CreateMapper();
            _commentService = commentService;
        }

        private ICommentService _commentService;
        // GET: Comment
        public ActionResult Comments(string id, int page = 1)
        {
            IEnumerable<CommentBLL> commentByPhoto = _commentService.GetCommentsByPhoto(id);
            CommentPageViewModel model = new CommentPageViewModel()
            {
                Comments =
                    commentByPhoto.Select(_mapper.Map<CommentBLL, CommentModel>)
                        .OrderBy(p => p.Id)
                        .Skip((page - 1) * PageSize)
                        .Take(PageSize)
                        .ToList(),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = commentByPhoto.Count()
                }
            };
            return View(model);
        }

        // GET: Comment/Details/5
        public ActionResult Details(string id)
        {
            return View();
        }

        [Authorize]
        // GET: Comment/Create
        public ActionResult AddComment(string id)
        {
            return View();
        }

        // POST: Comment/Create
        [Authorize]
        [HttpPost]
        public ActionResult AddComment(CommentModel comment)
        {
            try
            {
                _commentService.AddComment(new CommentBLL()
                {
                    Message = comment.Message,
                    UserId = User.Identity.GetUserId(),
                    PhotoId = comment.PhotoId,
                });
                return RedirectToAction("Comments", new {id = comment.PhotoId});
            }
            catch
            {
                return View();
            }
        }

        // GET: Comment/Edit/5
        [Authorize]
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

        // POST: Comment/Edit/5
        [Authorize]
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
                return RedirectToAction("Comments", new {id = comment.PhotoId});

            }
            catch
            {
                return View();
            }
        }

        // GET: Comment/Delete/5
        [Authorize]
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

        // POST: Comment/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Delete(CommentModel comment)
        {
            try
            {
                _commentService.RemoveComment(new CommentBLL()
                {
                    Id = comment.Id,
                });

                return RedirectToAction("Comments", new {id = comment.PhotoId});
            }
            catch
            {
                return View();
            }
        }
    }
}
