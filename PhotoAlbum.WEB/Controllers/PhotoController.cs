using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PhotoAlbum.BLL.EnittyBLL;
using PhotoAlbum.BLL.Interfaces;
using PhotoAlbum.WEB.Models;
using PhotoAlbum.WEB.Utils;

namespace PhotoAlbum.WEB.Controllers
{
    public class PhotoController : Controller
    {
        private int PageSize = 9;
        private IMapper _mapper;
        public PhotoController(IPhotoService photoService)
        {
            _mapper = new MappingMVCProfile().Config.CreateMapper();
            PhotoService = photoService;
        }

        private IPhotoService PhotoService;

        // GET: Photo
        [AllowAnonymous]
        public ActionResult Photos( string id , int page = 1)
        {
            IEnumerable<UserPhotoBLL> photosByUser = PhotoService.GetPhotosByUser(string.IsNullOrEmpty(id) ? User.Identity.GetUserId() : id );
            PhotoPageViewModel model = new PhotoPageViewModel()
            {
                UserPhotos = photosByUser.Select(_mapper.Map<UserPhotoBLL, UserPhotoModel>).OrderBy(p => p.Id).Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = photosByUser.Count()
                }
            };
            return View(model);
        }
        public ImageResult Partial(string path)
        {
            
            ViewBag.Message = "Это частичное представление.";
            return new ImageResult(path);
        }
        //// GET: Product/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //// POST: Product/Create
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                if (IsImage(upload))
                {
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    Directory.CreateDirectory(Server.MapPath("/Content/UserPhotos/" + User.Identity.GetUserId() + "/"));
                    string address = Server.MapPath("/Content/UserPhotos/" + User.Identity.GetUserId() + "/" + fileName);
                    upload.SaveAs(address);

                    PhotoService.AddPhoto(new UserPhotoBLL()
                    {
                        PhotoAddress = @"\" + address.Replace(Request.PhysicalApplicationPath, String.Empty),
                        UserId = User.Identity.GetUserId()
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
        public ActionResult Delete(string id, string photoAddress)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var photo = PhotoService.GetPhoto(id);
            if (photo == null)
            {
                return HttpNotFound();
            }

            return View(_mapper.Map<UserPhotoBLL, UserPhotoModel>(photo));
        }

        [HttpPost]
        public ActionResult Delete(UserPhotoModel photo)
        {
            try
            {
                PhotoService.RemovePhoto(new UserPhotoBLL()
                {
                    Id = photo.Id,
                });
                System.IO.File.Delete(photo.PhotoAddress);
                return RedirectToAction("Photos");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult OnAvatar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var photo = PhotoService.GetPhoto(id);
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
                PhotoService.EditPhoto(new UserPhotoBLL()
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
            var photo = PhotoService.GetPhoto(id);
            if (photo == null)
            {
                return HttpNotFound();
            }

            return View(_mapper.Map<UserPhotoBLL, UserPhotoModel>(photo));
        }
    }

}