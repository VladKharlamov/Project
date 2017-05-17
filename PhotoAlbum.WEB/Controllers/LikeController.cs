using System.Collections.Generic;
using System.Linq;
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

        public ActionResult Likes(string photoid)
        {
            IEnumerable<LikeBLL> likeByPhoto = _likeService.GetLikesByPhoto(photoid);
            var model = likeByPhoto.Select(_mapper.Map<LikeBLL, LikeModel>);
            return View(model);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

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
            return LikeCount(like);
        }

        public string LikeCount(LikeBLL like)
        {
            var isLike = _likeService.GetLikeByUserToPhoto(like.UserId, like.PhotoId);
            return (@"<span class='glyphicon glyphicon-heart " + (isLike != null ? " red-font" : "") + "'></span>" + GetLikesCount(like.PhotoId)).ToString();
        }

        public int GetLikesCount(string photoid)
        {

            return _likeService.GetCountLikesByPhoto(photoid);
        }
    }
}
