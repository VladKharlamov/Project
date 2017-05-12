using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PhotoAlbum.DAL.Entities;

namespace PhotoAlbum.DAL.Interfaces
{
    public interface IClientManager:IDisposable
    {
        void Create(ClientProfile item);
        ClientProfile Get(string id);

         IEnumerable<ClientProfile> GetAll();

         IEnumerable<ClientProfile> Find(Expression<Func<ClientProfile, bool>> predicate);

         void Add(ClientProfile entity);

         void AddRange(IEnumerable<ClientProfile> entities);

         void Remove(ClientProfile entity);

         void RemoveRange(IEnumerable<ClientProfile> entities);

         void Update(ClientProfile entities);

    }
}
