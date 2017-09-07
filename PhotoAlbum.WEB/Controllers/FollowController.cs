using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PhotoAlbum.BLL.EnittyBLL;
using PhotoAlbum.BLL.Interfaces;
using PhotoAlbum.WEB.Models;

namespace PhotoAlbum.WEB.Controllers
{
    public class FollowController : Controller
    {
        private IMapper _mapper;

        public FollowController(IFollowService followService)
        {
            _mapper = new MappingMVCProfile().Config.CreateMapper();
            _followService = followService;
        }
        private IFollowService _followService;


        public ActionResult GetCount(string Id)
        {
            var Follower = _followService.GetFollowerByUser(Id, User.Identity.GetUserId());

            ViewBag.Count = _followService.GetFollowersByUser(Id).Count();
            ViewBag.UserId = Id;
            ViewBag.IsFollowed = Follower != null;
            return PartialView();
        }


        [HttpPost]
        public ActionResult AddOrRemoveFollower(FollowModel follower)
        {
            follower.FollowerId = User.Identity.GetUserId();

            var Follower = _followService.GetFollowerByUser(follower.UserId, follower.FollowerId);

            if (Follower == null)
            {
                _followService.AddFollower(new FollowBLL()
                {
                    UserId = follower.UserId,
                    FollowerId = follower.FollowerId,
                });
            }
            else
            {
                _followService.RemoveFollower(Follower);
            }

            ViewBag.Count = _followService.GetFollowersByUser(follower.UserId).Count();
            ViewBag.UserId = follower.UserId;
            ViewBag.IsFollowed = Follower == null;
            return PartialView("GetCount");
        }

        [HttpPost]
        public ActionResult DeleteFollower(FollowModel follower)
        {
            _followService.RemoveFollower(new FollowBLL()
            {
                Id = follower.Id,
            });

            return View();
        }
    }
}
