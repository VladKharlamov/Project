using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PhotoAlbum.DAL.Entities;

namespace PhotoAlbum.DAL.Interfaces
{
    public interface IUserRepository:IDisposable
    {
        void Create(UserProfile item);
        UserProfile Get(string id);

         IEnumerable<UserProfile> GetAll();

         IEnumerable<UserProfile> Find(Expression<Func<UserProfile, bool>> predicate);

         void Add(UserProfile entity);

         void AddRange(IEnumerable<UserProfile> entities);

         void Remove(UserProfile entity);

         void RemoveRange(IEnumerable<UserProfile> entities);

         void Update(UserProfile entities);

    }
}
