using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using PhotoAlbum.BLL.EnittyBLL;
using PhotoAlbum.BLL.Interfaces;
using PhotoAlbum.WEB.Models;

namespace PhotoAlbum.WEB.Controllers
{
    public class LikeController : Controller
    {
        private int PageSize = 5;
        private IMapper _mapper;

        public LikeController(ILikeService likeService)
        {
            _mapper = new MappingMVCProfile().Config.CreateMapper();
            _likeService = likeService;
        }

        private ILikeService _likeService;
        // GET: Like
        public ActionResult Likes(string photoid)
        {
            IEnumerable<LikeBLL> likeByPhoto = _likeService.GetLikesByPhoto(photoid);
            var model = likeByPhoto.Select(_mapper.Map<LikeBLL, LikeModel>);
            return View(model);
        }

        // GET: Like/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //// GET: Like/Create
        public ActionResult Like(LikeBLL like)
        {
            if (_likeService.GetLikeByUserToPhoto(like.UserId, like.PhotoId) != null)
            {
                ViewBag.liked = true;
            }
            else
            {
                ViewBag.liked = false;
            }
            return PartialView();
        }
        [HttpPost]
        public string LikeSave(LikeBLL like)
        {
            var isLike = _likeService.GetLikeByUserToPhoto(like.UserId, like.PhotoId);
            if (isLike==null)
            {
                _likeService.AddLike(like);
            }
            else
            {
                _likeService.RemoveLike(isLike);
            }
            int liked = _likeService.GetCountLikesByPhoto(like.PhotoId);
            //Debug.WriteLine(liked);
            //ViewBag.liked = _likeService.Like(like);
            return LikeCount(like);

        }
        //"<span class='glyphicon glyphicon-heart'</span>"
        public string LikeCount(LikeBLL like)
        {
            int liked = _likeService.GetCountLikesByPhoto(like.PhotoId);
            var isLike = _likeService.GetLikeByUserToPhoto(like.UserId, like.PhotoId);
            return (@"<span class='glyphicon glyphicon-heart " + (isLike != null ? " red-font" : "") + "'></span> " + liked).ToString();
        }
        // POST: Like/Create
        //[HttpPost]
        //public ActionResult Like(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Like/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Like/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Like/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Like/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
