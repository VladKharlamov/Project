using System.Collections.Generic;
using PhotoAlbum.BLL.EnittyBLL;

namespace PhotoAlbum.BLL.Interfaces
{
    public interface ISubcribeService
    {
        void AddSubcriber(SubcribeBLL subcribeBLL);
        SubcribeBLL GetSubcriber(string id);
        IEnumerable<SubcribeBLL> GetSubcribersByUser(string userId);
        IEnumerable<SubcribeBLL> GetUsersBySubcriber(string subcriberId);
        void RemoveSubcribe(SubcribeBLL commentBll);
    }
}
