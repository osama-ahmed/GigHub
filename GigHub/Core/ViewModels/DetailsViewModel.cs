
using GigHub.Core.Models;
namespace GigHub.Core.ViewModels
{
    public class DetailsViewModel
    {
        public Gig Gig { get; set; }

        public bool AuthenticatedUser { get; set; }

        public bool following { get; set; }

        public bool going { get; set; }
    }
}