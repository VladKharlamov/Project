using System.Collections.Generic;
using PhotoAlbum.BLL.EnittyBLL;

namespace PhotoAlbum.BLL.Interfaces
{
    public interface IFollowService
    {
        void AddFollower(FollowBLL followBll);
        FollowBLL GetFollower(string id);
        IEnumerable<FollowBLL> GetFollowersByUser(string userId);
        IEnumerable<FollowBLL> GetUsersByFollower(string subcriberId);
        void RemoveFollower(FollowBLL commentBll);
        FollowBLL GetFollowerByUser(string userId, string followerId);
    }
}
