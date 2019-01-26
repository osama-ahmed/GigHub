using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GigHub.Persistance.Repositories
{
    public class GigRepository : IGigRepository
    {
        private ApplicationDbContext _context;

        public GigRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Gig> GetArtistUpcomingGigs(string artistId)
        {
            return _context.Gigs
                .Include(g => g.Genre)
                .Where(g => g.ArtistId == artistId && g.DateTime > DateTime.Now && g.IsCanceled == false)
                .OrderBy(g => g.DateTime)
                .ToList();
        }

        public IEnumerable<Gig> GetGigsUserAttending(string userId)
        {
            return _context.Attendances
               .Where(a => a.AttendeeId == userId)
               .Select(a => a.Gig)
               .Where(g => g.DateTime > DateTime.Now)
               .Include(g => g.Artist)
               .Include(g => g.Genre)
               .OrderBy(g => g.DateTime)
               .ToList();
        }

        public Gig GetUserGig(int gigId, string userId)
        {
            return _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .SingleOrDefault(g => g.Id == gigId && g.ArtistId == userId);
        }

        public IEnumerable<Gig> FindGigs(string searchTerm)
        {
            return _context.Gigs
                .Where(g => g.Artist.Name.Contains(searchTerm) ||
                    g.Venue.Contains(searchTerm) ||
                    g.Genre.Name.Contains(searchTerm))
                .Where(g => g.IsCanceled==false && g.DateTime > DateTime.Now)
                .OrderBy(m => m.DateTime)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();
        }

        public Gig FindGig(int gigId)
        {
            return _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Artist.Followers)
                .Include(g => g.Attendances)
                .Where(g => g.Id == gigId)
                .FirstOrDefault();
        }

        public void Add(Gig gig)
        {
            _context.Gigs.Add(gig);
        }

        public IEnumerable<Gig> GetUpcomingGigs(string searchTerm = null)
        {
            var upcomingGigs = _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Where(g => g.DateTime > DateTime.Now && !g.IsCanceled);

            if (!String.IsNullOrWhiteSpace(searchTerm))
            {
                upcomingGigs = upcomingGigs
                    .Where(g =>
                            g.Artist.Name.Contains(searchTerm) ||
                            g.Genre.Name.Contains(searchTerm) ||
                            g.Venue.Contains(searchTerm));
            }

            return upcomingGigs.ToList();
        }

        public Gig GetGigWithAttendees(int gigId)
        {
            return _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .SingleOrDefault(g => g.Id == gigId);
        }
    }
}