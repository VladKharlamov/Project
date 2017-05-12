﻿using System.Configuration;
using System.Threading.Tasks;
using AutoMapper;
using MailKit.Net.Smtp;
using Microsoft.AspNet.Identity;
using MimeKit;
using PhotoAlbum.BLL.Infrastructure;
using PhotoAlbum.BLL.Interfaces;
using PhotoAlbum.DAL.Interfaces;

namespace PhotoAlbum.BLL.Services
{
    public class EmailService : IEmailService
    {
        IIdentityUnitOfWork Database { get; set; }
        private IMapper _mapper;

        public EmailService(IIdentityUnitOfWork uow)
        {
            Database = uow;
            _mapper = new MappingIdentityProfile(uow).Config.CreateMapper();
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", ConfigurationManager.AppSettings["login"]));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(ConfigurationManager.AppSettings["server"], int.Parse(ConfigurationManager.AppSettings["port"]), false);
                await client.AuthenticateAsync(ConfigurationManager.AppSettings["login"], ConfigurationManager.AppSettings["password"]);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

        public string GenerateEmailConfirmationToken(string email)
        {
            return Database.UserManager.GenerateEmailConfirmationToken(
                    Database.UserManager.FindByName(email).Id);
                //Database.UserManager.Users.Where(p=>p.Id==id).Single());
        }

        public async Task<OperationDetails> ConfirmEmailAsync(string userId, string code)
        {
            var user = await Database.UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new OperationDetails(false, "Пользователь не найден", "");
            }
            var result = await Database.UserManager.ConfirmEmailAsync(userId, code);

            return new OperationDetails(true, "Успех", "");

        }
        public async Task<OperationDetails> CheckConfirmEmailAsync(string userName)
        {
            var user = await Database.UserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new OperationDetails(false, "Пользователь не найден", "");
            }
            if(await Database.UserManager.IsEmailConfirmedAsync(user.Id))
            return new OperationDetails(true, "Успех", "");
            else
                return new OperationDetails(false, "Вы не подтвердили свой email", "");
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
