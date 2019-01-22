using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PhotoAlbum.BLL.EnittyBLL;
using PhotoAlbum.BLL.Infrastructure;
using PhotoAlbum.BLL.Interfaces;
using PhotoAlbum.WEB.Models;

namespace PhotoAlbum.WEB.Controllers
{
    public class AccountController : Controller
    {
        private int PageSize = 4;
        private IMapper _mapper;

        public AccountController(IEmailService emailService)
        {
            _mapper = new MappingMVCProfile().Config.CreateMapper();
            EmailService = emailService;
        }

        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        private IEmailService EmailService;


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult UserManagement(int page = 1)
        {
            IEnumerable<UserBLL> users = UserService.GetAllUsers();
            IdentityPageViewModel model = new IdentityPageViewModel()
            {
                Users = users.Select(_mapper.Map<UserBLL, UserModel>).OrderBy(p => p.Id).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = users.Count()

                }
            };
            return View(model);
        }

        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserService.GetUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(_mapper.Map<UserBLL, UserModel>(user));
        }
        [Authorize(Roles = "admin, moderator")]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserService.GetUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(_mapper.Map<UserBLL, UserModel>(user));
        }

        [Authorize(Roles = "admin, moderator")]
        [HttpPost]
        public ActionResult Edit(UserModel user)
        {
            try
            {
                UserService.EditUser(new UserBLL()
                {

                    Id = user.Id,
                    Name = user.Name,
                    Birthday = user.Birthday,
                    Email = user.Email,
                    UserName = user.UserName,
                });

                return RedirectToAction("UserManagement");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult ChangeRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserService.GetUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(_mapper.Map<UserBLL, UserModel>(user));
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult ChangeRole(UserModel user, string newRole)
        {
            try
            {
                UserService.ChangeRole(new UserBLL()
                {

                    Id = user.Id,
                    Role = user.Role,
                }, newRole);

                return RedirectToAction("UserManagement");
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = "admin")]
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserService.GetUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(_mapper.Map<UserBLL, UserModel>(user));
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Delete(UserModel user)
        {
            if (user == null)
                return HttpNotFound();
           

            UserService.RemoveUser(user.Id);

            return RedirectToAction("UserManagement");
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                //OperationDetails operationDetails = await EmailService.CheckConfirmEmailAsync(model.UserName);
                //if (operationDetails.Succedeed)
                //{


                UserBLL userBll = new UserBLL { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await UserService.Authenticate(userBll);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Invalid login or password");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
                //}
                // ModelState.AddModelError(operationDetails.Property, operationDetails.Message);

            }
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model, string returnUrl = null)
        {
            await SetInitialDataAsync();
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                UserBLL userBll = new UserBLL
                {
                    Email = model.Email,
                    Password = model.Password,
                    Birthday = model.Bithday,
                    Name = model.Name,
                    Role = "user"
                };
                OperationDetails operationDetails = await UserService.Create(userBll);

                if (operationDetails.Succedeed)
                {

                    //    var code = EmailService.GenerateEmailConfirmationToken(model.Email);
                    //    var callbackUrl = Url.Action(
                    //        "ConfirmEmail",
                    //        "Account",
                    //        new { userId = userBll.Id, code = code },
                    //        protocol: Request.Url.Scheme);
                    //    await EmailService.SendEmailAsync(model.Email, "Confirm your account",
                    //        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                    return View("SuccessRegister");
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var result = await EmailService.ConfirmEmailAsync(userId, code);
            return View(result.Succedeed ? "ConfirmEmail" : "Error");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                OperationDetails operationDetails = UserService.ChangePassword(User.Identity.GetUserId(), model.CurrentPassword, model.NewPassword);
                if (operationDetails.Succedeed)
                    return View("SuccessPasswordChange");
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }

        private async Task SetInitialDataAsync()
        {
            await UserService.SetInitialData(new UserBLL
            {
                UserName = "admin",
                Email = "harlamowlad@gmail.com",
                Password = "11111111",
                Name = "Vladyslav",
                Birthday = DateTime.Now,//DateTime.Parse("10/15/1995").Date,
                Role = "admin"
            }, new List<string> { "user", "admin", "moderator" });
        }
    }
}