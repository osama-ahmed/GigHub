
using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly IUnitOfWork uow;

        public GigsController(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        [Authorize]
        public ActionResult Mine()
        {
            var gigs = uow.Gigs.GetArtistUpcomingGigs(User.Identity.GetUserId());

            return View(gigs);
        }

        public ActionResult Attending()
        {
            var viewModel = new GigsViewModel
            {
                UpcomingGigs=uow.Gigs.GetGigsUserAttending(User.Identity.GetUserId()),
                ShowActions=User.Identity.IsAuthenticated,
                Heading="Gigs I'm Attending",
                Attendances=uow.Attendances.GetUserAttendances(User.Identity.GetUserId()).ToLookup(a => a.GigId)
            };

            return View("Gigs", viewModel);
        }

        public ActionResult Following()
        {
            var followees = uow.Followings.GetFolllowees(User.Identity.GetUserId());

            return View(followees);
        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres=uow.Genres.GetGenres(),
                Heading="Add a Gig"
            };

            return View("GigForm", viewModel);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres =uow.Genres.GetGenres();

                return View("GigForm", viewModel);
            }
                

            var gig = new Gig
            {
                ArtistId=User.Identity.GetUserId(),
                DateTime=viewModel.GetDateTime(),
                GenreId=viewModel.Genre,
                Venue=viewModel.Venue
            };

            uow.Gigs.Add(gig);
            uow.Complete();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var gig = uow.Gigs.GetUserGig(id, User.Identity.GetUserId());

            var viewModel = new GigFormViewModel
            {
                Heading="Edit a Gig",
                Id=gig.Id,
                Genres = uow.Genres.GetGenres(),
                Venue=gig.Venue,
                Date=gig.DateTime.ToString("d MMM yyyy"),
                Time=gig.DateTime.ToString("HH:MM"),
                Genre=gig.GenreId
            };

            return View("GigForm",viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = uow.Genres.GetGenres();

                return View("GigForm", viewModel);
            }

            var gig = uow.Gigs.GetUserGig(viewModel.Id, User.Identity.GetUserId());

            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);

            uow.Complete();

            return RedirectToAction("Mine", "Gigs");
        }

        [HttpPost]
        public ActionResult Search(string searchTerm)
        {
            var viewModel=new GigsViewModel
            {
                UpcomingGigs=uow.Gigs.FindGigs(searchTerm),
                ShowActions=User.Identity.IsAuthenticated,
                Heading="Search",
                SearchTerm=searchTerm,
                Attendances=uow.Attendances.GetUserAttendances(User.Identity.GetUserId()).ToLookup(a => a.GigId)
            };

            return View("Gigs", viewModel);
        }

        public ActionResult Details(int gigId)
        {
            var gig = uow.Gigs.FindGig(gigId);

            if (gig == null)
                return HttpNotFound();

            var userId = User.Identity.GetUserId();

            var detailsViewModel = new DetailsViewModel
            {
                Gig=gig,
                AuthenticatedUser=userId=="" ? false : true,
                following=gig.Artist.Followers.Any(f => f.FollowerId == userId),
                going=gig.Attendances.Any(a => a.AttendeeId == userId)
            };

            return View(detailsViewModel);
        }
    }
}