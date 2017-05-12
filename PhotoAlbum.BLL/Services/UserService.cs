﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PhotoAlbum.BLL.EnittyBLL;
using PhotoAlbum.BLL.Infrastructure;
using PhotoAlbum.BLL.Interfaces;
using PhotoAlbum.DAL.Entities;
using PhotoAlbum.DAL.Interfaces;

namespace PhotoAlbum.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IIdentityUnitOfWork _db;
        private IMapper _mapper;

        public UserService(IIdentityUnitOfWork uow)
        {
            _db = uow;
            _mapper = new MappingIdentityProfile(uow).Config.CreateMapper();
        }

        public async Task<OperationDetails> Create(UserBLL userBll)
        {
            ApplicationUser user = await _db.UserManager.FindByEmailAsync(userBll.Email);
            if (user == null)
            {

                user = new ApplicationUser { Email = userBll.Email, UserName = userBll.Email };
                var result = await _db.UserManager.CreateAsync(user, userBll.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                await _db.UserManager.AddToRoleAsync(user.Id, userBll.Role);

                ClientProfile clientProfile = new ClientProfile
                {
                    Id = user.Id,
                    Bithday = userBll.Bithday,
                    Name = userBll.Name
                };
                _db.ClientManager.Create(clientProfile);
                await _db.SaveAsync();
                return new OperationDetails(true, "The registration was successful", "");
            }
            return new OperationDetails(false, "Email is already exists.", "Email");
        }

        public async Task<ClaimsIdentity> Authenticate(UserBLL userBll)
        {
            ClaimsIdentity claim = null;

            ApplicationUser user = await _db.UserManager.FindAsync(userBll.Email, userBll.Password);

            if (user != null)
                claim = await _db.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            claim.AddClaim(new Claim("Name", user.ClientProfile.Name));

            return claim;
        }

        public IEnumerable<UserBLL> GetAllUsers()
        {
            return _db.ClientManager.GetAll().Select(_mapper.Map<ClientProfile, UserBLL>);
        }

        public async Task SetInitialData(UserBLL adminBll, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await _db.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await _db.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminBll);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public UserBLL GetUser(string id)
        {
            return _mapper.Map<ClientProfile, UserBLL>(_db.ClientManager.Get(id));
        }

        public OperationDetails ChangePassword(string id, string currentPassword, string newPassword)
        {
            try
            {
                _db.UserManager.ChangePassword(id, currentPassword, newPassword);
                _db.SaveAsync();
                return new OperationDetails(true, "Пароль успешно изменен", "");

            }
            catch (Exception e)
            {
                return new OperationDetails(false, "Проверьте введенные данные", "");
            }
        }

        //public OperationDetails ConfirmPasswordEmail( string currentPassword, string newPassword)
        //{
        //    try
        //    {

        //        _db.SaveAsync();
        //        return new OperationDetails(true, "Пароль успешно изменен", "");

        //    }
        //    catch (Exception e)
        //    {
        //        return new OperationDetails(false, "Проверьте введенные данные", "");
        //    }
        //}

        public void EditUser(UserBLL userBll)
        {
            if (userBll == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }

            if (String.IsNullOrEmpty(userBll.Name))
            {
                throw new ArgumentException("Name can't be empty");
            }

            var user = _db.ClientManager.Find(p => p.Id == userBll.Id).Single();
            user.Id = userBll.Id;
            user.Name = userBll.Name;
            user.Bithday = userBll.Bithday;
            user.ApplicationUser.Email = userBll.Email;
            user.ApplicationUser.UserName = userBll.Email;
            //_db.UserManager.ChangePasswordAsync(userBll.Id,)
            //    user.Password = userBll.ApplicationUser.PasswordHash;

            //    user.Role = _db.RoleManager.FindById(userBll.ApplicationUser.Roles.First(p2=>p2.UserId==userBll.Id).RoleId).Name;

            _db.ClientManager.Update(user);


            _db.SaveAsync();
        }
        public UserBLL GetUserByEmail(string email)
        {
            UserBLL userDto = null;

            var user = _db.UserManager
                                  .Users
                                  .Where(u => u.Email == email)
                                  .FirstOrDefault();

            if (user != null)
                userDto = new UserBLL { Email = user.Email };

            return userDto;
        }
        public UserBLL GetUserById(string id)
        {
            UserBLL userDto = null;

            var user = _db.UserManager
                                  .Users
                                  .Where(u => u.Id == id)
                                  .FirstOrDefault();

            if (user != null)
            {
                userDto = new UserBLL
                {
                    Name = user.ClientProfile.Name,
                    Bithday = user.ClientProfile.Bithday,
                    Email = user.Email,
                };
            }

            return userDto;
        }

        public async Task<OperationDetails> RemoveUser(UserBLL userBll)
        {
            ApplicationUser user = _db.UserManager.Users.FirstOrDefault(u => u.Email == userBll.Email);

            if (userBll == null)
            {
                return new OperationDetails(succedeed: false,
                        message: "User not found", prop: "");
            }
            var result = await _db.UserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await _db.SaveAsync();
                return new OperationDetails(succedeed: true,
                    message: "The user has been successfully deleted", prop: "");
            }
            else
            {
                return new OperationDetails(succedeed: false,
                    message: result.Errors.FirstOrDefault(), prop: "");
            }
        }

    }
}
