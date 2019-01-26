using GigHub.Core.Models;
using System.Collections.Generic;

namespace GigHub.Core.Repositories
{
    public interface IGigRepository
    {
        IEnumerable<Gig> GetArtistUpcomingGigs(string artistId);
        IEnumerable<Gig> GetGigsUserAttending(string userId);
        Gig GetUserGig(int gigId, string userId);
        IEnumerable<Gig> FindGigs(string searchTerm);
        Gig FindGig(int gigId);
        void Add(Gig gig);
        IEnumerable<Gig> GetUpcomingGigs(string searchTerm = null);
        Gig GetGigWithAttendees(int gigId);
    }
}