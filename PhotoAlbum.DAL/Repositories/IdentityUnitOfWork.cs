﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using PhotoAlbum.DAL.EF;
using PhotoAlbum.DAL.Entities;
using PhotoAlbum.DAL.Identity;
using PhotoAlbum.DAL.Interfaces;

namespace PhotoAlbum.DAL.Repositories
{
    public class IdentityUnitOfWork : IIdentityUnitOfWork
    {
        private ApplicationContext _db;

        public ApplicationUserManager UserManager { get; }
        public ApplicationRoleManager RoleManager { get; }
        public IClientManager ClientManager { get; }

        public IdentityUnitOfWork(string connectionString)
        {
            _db = new ApplicationContext(connectionString);
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db));
            RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_db));
            ClientManager = new ClientManager(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    UserManager.Dispose();
                    RoleManager.Dispose();
                    ClientManager.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
