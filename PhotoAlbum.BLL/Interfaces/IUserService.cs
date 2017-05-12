using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using PhotoAlbum.BLL.EnittyBLL;
using PhotoAlbum.BLL.Infrastructure;

namespace PhotoAlbum.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserBLL userBll);
        Task<ClaimsIdentity> Authenticate(UserBLL userBll);
        Task SetInitialData(UserBLL adminBll, List<string> roles);
        OperationDetails ChangePassword(string id, string currentPassword, string newPassword);


        IEnumerable<UserBLL> GetAllUsers();
        UserBLL GetUser(string id);
        void UpdateUser(UserBLL userBll);
        void RemoveUser(UserBLL userBll);
    }
}
