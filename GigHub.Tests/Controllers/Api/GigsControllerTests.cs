using GigHub.Controllers.Api;
using GigHub.Core;
using GigHub.Core.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GigHub.Tests.Controllers.Api
{
    [TestClass]
    public class GigsControllerTests
    {
        private GigsController _controller;
        private Mock<IGigRepository> _mockRepository;
        private string _userId;

        [TestInitialize]
        public void TestIntialize()
        {
            _mockRepository = new Mock<IGigRepository>();

            var mockUOW = new Mock<IUnitOfWork>();
            mockUOW.SetupGet(u => u.Gigs).Returns(_mockRepository.Object);

            _controller = new GigsController(mockUOW.Object);
            _userId = "1";
            _controller.
        }
    }
}
