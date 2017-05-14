using System.Collections.Generic;
using PhotoAlbum.BLL.EnittyBLL;

namespace PhotoAlbum.BLL.Interfaces
{
    public interface ISubcribeService
    {
        void AddSubcriber(SubscribeBLL subscribeBll);
        SubscribeBLL GetSubcriber(string id);
        IEnumerable<SubscribeBLL> GetSubcribersByUser(string userId);
        IEnumerable<SubscribeBLL> GetUsersBySubcriber(string subcriberId);
        void RemoveSubcribe(SubscribeBLL commentBll);
    }
}
