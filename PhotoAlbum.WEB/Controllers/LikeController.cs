using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PhotoAlbum.BLL.EnittyBLL;
using PhotoAlbum.BLL.Interfaces;
using PhotoAlbum.BLL.Services;
using PhotoAlbum.WEB.Models;

namespace PhotoAlbum.WEB.Controllers
{
    public class LikeController : Controller
    {
        private int PageSize = 12;
        private IMapper _mapper;

        public LikeController(ILikeService likeService, IPhotoService photoService)
        {
            _mapper = new MappingMVCProfile().Config.CreateMapper();
            _likeService = likeService;
            _photoService = photoService;
        }

        private IPhotoService _photoService;

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

        public ActionResult GetMyLikes(int page=1)
        {
            var likes = _likeService.GetAllLikesByUser(User.Identity.GetUserId());
            List<UserPhotoBLL> photosBySearch = new List<UserPhotoBLL>();
            foreach (var item in likes)
            {
               photosBySearch.AddRange(_photoService.GetPhotos().Where(p => p.Id == item.PhotoId));
            }
            PhotoPageViewModel model = new PhotoPageViewModel()
            {
                UserPhotos = photosBySearch.Select(_mapper.Map<UserPhotoBLL, UserPhotoModel>).OrderByDescending(p => p.Date).Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = photosBySearch.Count()
                }
            };
            return View(model);
        }
    }
}
