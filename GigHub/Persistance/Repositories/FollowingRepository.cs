using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.Persistance.Repositories
{
    public class FollowingRepository :IFollowingRepository
    {
        private ApplicationDbContext _context;

        public FollowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ApplicationUser> GetFolllowees(string userId)
        {
            return _context.Followings
                .Where(f => f.FollowerId == userId)
                .Select(f => f.Followee)
                .ToList();
        }

        public Following GetFollowing(string followerId, string followeeId)
        {
            return _context.Followings
                .SingleOrDefault(f => f.FolloweeId == followeeId && f.FollowerId == followerId);
        }

        public void Add(Following following)
        {
            _context.Followings.Add(following);
        }
    }
}