using System;
using System.Threading.Tasks;
using PhotoAlbum.BLL.Infrastructure;

namespace PhotoAlbum.BLL.Interfaces
{
    public interface IEmailService:IDisposable
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task<OperationDetails> ConfirmEmailAsync(string userId, string code);
        string GenerateEmailConfirmationToken(string email);
        Task<OperationDetails> CheckConfirmEmailAsync(string userName);
    }
}
