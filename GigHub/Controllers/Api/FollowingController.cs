using GigHub.Core;
using GigHub.Core.Dto;
using GigHub.Core.Models;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class FollowingController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public FollowingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();

            var following = _unitOfWork.Followings.GetFollowing(userId, dto.FolloweeId);
            if (following != null)
                return BadRequest("Following already exists.");

            following = new Following
            {
                FolloweeId=dto.FolloweeId,
                FollowerId=userId
            };

            _unitOfWork.Followings.Add(following);
            _unitOfWork.Complete();

            return Ok();
        }
    }
}
