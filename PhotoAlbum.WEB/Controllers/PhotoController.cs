using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PhotoAlbum.BLL.EnittyBLL;
using PhotoAlbum.BLL.Interfaces;
using PhotoAlbum.WEB.Models;

namespace PhotoAlbum.WEB.Controllers
{
    public class PhotoController : Controller
    {
        private int PageSize = 12;
        private IMapper _mapper;
        public PhotoController(IPhotoService photoService, ILikeService likeService)
        {
            _mapper = new MappingMVCProfile().Config.CreateMapper();
            _photoService = photoService;
            _likeService = likeService;
        }
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }
        private IPhotoService _photoService;
        private ILikeService _likeService;


        public ActionResult Photos(string id = null, int page = 1)
        {
            id = string.IsNullOrEmpty(id) ? User.Identity.GetUserId() : id;
            if (UserService.GetUser(id)==null)
            {
               return new HttpNotFoundResult();
            }

            IEnumerable<UserPhotoBLL> photosByUser = _photoService.GetPhotosByUser(id);
            PhotoPageViewModel model = new PhotoPageViewModel()
            {
                UserPhotos = photosByUser.Select(_mapper.Map<UserPhotoBLL, UserPhotoModel>).OrderByDescending(p => p.Date).Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = photosByUser.Count()
                }
            };
            ViewBag.Id = id;
            return View(model);
        }
        public ActionResult Search(string @string, int page = 1)
        {
            IEnumerable<UserPhotoBLL> photosBySearch = _photoService.GetPhotosBySearch(@string);
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

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase upload, UserPhotoModel model)
        {
            if (upload != null)
            {
                if (IsImage(upload))
                {
                    //string fileName = System.IO.Path.GetFileName(upload.FileName);
                    Directory.CreateDirectory(Server.MapPath("/Content/UserPhotos/" + User.Identity.GetUserId() + "/"));
                    string address = Server.MapPath("/Content/UserPhotos/" + User.Identity.GetUserId() + "/" + Guid.NewGuid().ToString() + "." + upload.FileName.Substring(upload.FileName.LastIndexOf(".") + 1));
                    upload.SaveAs(address);

                    _photoService.AddPhoto(new UserPhotoBLL()
                    {
                        PhotoAddress = @"\" + address.Replace(Request.PhysicalApplicationPath, String.Empty),
                        UserId = User.Identity.GetUserId(),
                        Description = model.Description
                    });
                    return RedirectToAction("Photos");
                }
            }
            return View();
        }
        private bool IsImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }
            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" }; // add more if u like...
            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var photo = _photoService.GetPhoto(id);
            if (photo == null)
            {
                return HttpNotFound();
            }

            return View(_mapper.Map<UserPhotoBLL, UserPhotoModel>(photo));
        }

        [HttpPost]
        public ActionResult Delete(UserPhotoModel photo)
        {

            var photoModel = _photoService.GetPhoto(photo.Id);

            _photoService.RemovePhoto(new UserPhotoBLL()
            {
                Id = photo.Id,
            });
            System.IO.File.Delete(Server.MapPath(photoModel.PhotoAddress));
            return RedirectToAction("Photos");

        }
        public ActionResult OnAvatar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var photo = _photoService.GetPhoto(id);
            if (photo == null)
            {
                return HttpNotFound();
            }

            return View(_mapper.Map<UserPhotoBLL, UserPhotoModel>(photo));
        }

        [HttpPost]
        public ActionResult OnAvatar(UserPhotoModel photo)
        {
            try
            {
                _photoService.EditPhoto(new UserPhotoBLL()
                {
                    Id = photo.Id,
                    IsAvatar = true
                });
                return RedirectToAction("Photos");
            }
            catch
            {
                return View();
            }

        }
        public ActionResult ViewPhoto(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var photo = _photoService.GetPhoto(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(_mapper.Map<UserPhotoBLL, UserPhotoModel>(photo));
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var photo = _photoService.GetPhoto(id);
            if (photo == null)
            {
                return HttpNotFound();
            }

            return View(_mapper.Map<UserPhotoBLL, UserPhotoModel>(photo));
        }

        [HttpPost]
        public ActionResult Edit(UserPhotoModel photo)
        {
            try
            {
                if (!string.IsNullOrEmpty(photo.Description))
                {
                    _photoService.EditPhoto(new UserPhotoBLL()
                    {
                        Id = photo.Id,
                        Description = photo.Description
                    });
                }
                return RedirectToAction("Photos", new { id = User.Identity.GetUserId() });

            }
            catch
            {
                return View();
            }
        }

    }
}