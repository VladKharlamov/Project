using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using PhotoAlbum.DAL.EF;
using PhotoAlbum.DAL.Entities;
using PhotoAlbum.DAL.Interfaces;

namespace PhotoAlbum.DAL.Repositories
{
    class ClientManager : IClientManager
    {
        public ApplicationContext Context { get; set; }

        public ClientManager(ApplicationContext db)
        {
            Context = db;
        }

        public void Create(ClientProfile item)
        {
            Context.ClientProfiles.Add(item);
            Context.SaveChanges();
        }

        public ClientProfile Get(string id)
        {
            return Context.Set<ClientProfile>().Find(id);
        }

        public IEnumerable<ClientProfile> GetAll()
        {
            return Context.Set<ClientProfile>().ToList();
        }

        public IEnumerable<ClientProfile> Find(Expression<Func<ClientProfile, bool>> predicate)
        {
            return Context.Set<ClientProfile>().Where(predicate);
        }

        public ClientProfile SingleOrDefault(Expression<Func<ClientProfile, bool>> predicate)
        {
            return Context.Set<ClientProfile>().SingleOrDefault(predicate);
        }

        public void Add(ClientProfile entity)
        {
            Context.Set<ClientProfile>().Add(entity);
        }

        public void AddRange(IEnumerable<ClientProfile> entities)
        {
            Context.Set<ClientProfile>().AddRange(entities);
        }

        public void Remove(ClientProfile entity)
        {
            Context.Set<ClientProfile>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<ClientProfile> entities)
        {
            Context.Set<ClientProfile>().RemoveRange(entities);
        }

        public void Update(ClientProfile entities)
        {
            Context.Set<ClientProfile>().AddOrUpdate(entities);
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
