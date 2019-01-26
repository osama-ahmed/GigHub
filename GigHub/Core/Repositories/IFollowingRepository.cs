using GigHub.Core.Models;
using System.Collections.Generic;
namespace GigHub.Core.Repositories
{
    public interface IFollowingRepository
    {
        IEnumerable<ApplicationUser> GetFolllowees(string userId);
        Following GetFollowing(string followerId, string followeeId);
        void Add(Following following);
    }
}
