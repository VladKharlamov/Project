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
    class UserRepository : IUserRepository
    {
        public ApplicationContext Context { get; set; }

        public UserRepository(ApplicationContext db)
        {
            Context = db;
        }

        public void Create(UserProfile item)
        {
            Context.UserProfiles.Add(item);
        }

        public UserProfile Get(string id)
        {
            return Context.Set<UserProfile>().Find(id);
        }

        public IEnumerable<UserProfile> GetAll()
        {
            return Context.Set<UserProfile>().ToList();
        }

        public IEnumerable<UserProfile> Find(Expression<Func<UserProfile, bool>> predicate)
        {
            return Context.Set<UserProfile>().Where(predicate);
        }

        public UserProfile SingleOrDefault(Expression<Func<UserProfile, bool>> predicate)
        {
            return Context.Set<UserProfile>().SingleOrDefault(predicate);
        }

        public void Add(UserProfile entity)
        {
            Context.Set<UserProfile>().Add(entity);
        }

        public void AddRange(IEnumerable<UserProfile> entities)
        {
            Context.Set<UserProfile>().AddRange(entities);
        }

        public void Remove(UserProfile entity)
        {
            Context.Set<UserProfile>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<UserProfile> entities)
        {
            Context.Set<UserProfile>().RemoveRange(entities);
        }

        public void Update(UserProfile entities)
        {
            Context.Set<UserProfile>().AddOrUpdate(entities);
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
