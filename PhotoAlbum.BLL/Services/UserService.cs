using System;
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
                user = new ApplicationUser { Email = userBll.Email, UserName = userBll.UserName };
                var result = await _db.UserManager.CreateAsync(user, userBll.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                // добавляем роль
                await _db.UserManager.AddToRoleAsync(user.Id, userBll.Role);
                // создаем профиль клиента
                ClientProfile clientProfile = new ClientProfile { Id = user.Id, Bithday = userBll.Bithday, Name = userBll.Name };
                _db.ClientManager.Create(clientProfile);
                await _db.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(UserBLL userBll)
        {
            ClaimsIdentity claim = null;
            // находим пользователя
            ApplicationUser user = await _db.UserManager.FindAsync(userBll.UserName, userBll.Password);
            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = await _db.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }


        // public bool ConfirmEmail(string Token, string Email)
        //{

        //}


        public IEnumerable<UserBLL> GetAllUsers()
        {
            return _db.ClientManager.GetAll().Select(_mapper.Map<ClientProfile, UserBLL>);
            //return _db.ClientManager.GetAll().Select(p => new UserBLL
            //{
            //    Id = p.Id,
            //    Name = p.Name,
            //    Address = p.Address,
            //    Email = p.ApplicationUser.Email,
            //    UserName = p.ApplicationUser.UserName,
            //    Password = p.ApplicationUser.PasswordHash,
            //    Role = _db.RoleManager.FindById(p.ApplicationUser.Roles.First(p2=>p2.UserId==p.Id).RoleId).Name
            //});
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

        public OperationDetails ConfirmEmail( string currentPassword, string newPassword)
        {
            try
            {
                
                _db.SaveAsync();
                return new OperationDetails(true, "Пароль успешно изменен", "");

            }
            catch (Exception e)
            {
                return new OperationDetails(false, "Проверьте введенные данные", "");
            }
        }

        public void UpdateUser(UserBLL userBll)
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
            user.ApplicationUser.UserName = userBll.UserName;
            //_db.UserManager.ChangePasswordAsync(userBll.Id,)
            //    user.Password = userBll.ApplicationUser.PasswordHash;
            
            //    user.Role = _db.RoleManager.FindById(userBll.ApplicationUser.Roles.First(p2=>p2.UserId==userBll.Id).RoleId).Name;

            _db.ClientManager.Update(user);


            _db.SaveAsync();
        }

        public void RemoveUser(UserBLL userBll)
        {
            if (userBll == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }
            var product = _db.ClientManager.Find(p => p.Id == userBll.Id).Single();
            _db.ClientManager.Remove(product);
        }
    }
}
