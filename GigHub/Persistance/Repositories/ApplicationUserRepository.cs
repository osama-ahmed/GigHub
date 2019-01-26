using GigHub.Core.Models;
namespace GigHub.Persistance.Repositories
{
    public class ApplicationUserRepository
    {
        private ApplicationDbContext _context;

        public ApplicationUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}