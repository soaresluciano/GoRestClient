using System;
using GoRestClient.Core;
using GoRestClient.Models;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using GoRestClient.Services.Models;

namespace GoRestClient.Services.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IRestProvider> _mockRestProvider;
        private Mock<IStatusManager> _mockStatusManager;
        UserService _unitUnderTest;

        [SetUp]
        public void SetUp()
        {
            this._mockRestProvider = new Mock<IRestProvider>();
            this._mockStatusManager = new Mock<IStatusManager>();
            _unitUnderTest = new UserService(
                this._mockRestProvider.Object,
                this._mockStatusManager.Object);
        }

        [Test]
        public void Search_WhenPageIsZero_ShouldThrowArgumentException()
        {
            // Arrange
            uint page = 0;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _unitUnderTest.Search(null, page));
        }

        [Test]
        public async Task Search_WhenPageIsValid_ShouldIncludeItIntoTheParameter()
        {
            // Arrange
            _mockRestProvider
                .Setup(m => m.GetAsync<ResponseModel>(It.IsAny<string>()))
                .ReturnsAsync(new ResponseModel());
            uint page = 10;

            // Act
            await _unitUnderTest.Search(null, page);

            // Assert
            _mockRestProvider.Verify(
                m => m.GetAsync<ResponseModel>(It.Is<string>(
                    url => url.Contains($"page={page};"))));
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task Search_WhenNameFilterIsNullOrEmpty_ShouldNotIncludeItIntoTheParameter(string nameFilter)
        {
            // Arrange
            _mockRestProvider
                .Setup(m => m.GetAsync<ResponseModel>(It.IsAny<string>()))
                .ReturnsAsync(new ResponseModel());

            // Act
            await _unitUnderTest.Search(nameFilter, 1);

            // Assert
            _mockRestProvider.Verify(
                m => m.GetAsync<ResponseModel>(It.Is<string>(
                    url => !url.Contains($"name="))));
        }

        [Test]
        public async Task Search_WhenNameIsValid_ShouldIncludeItIntoTheParameter()
        {
            // Arrange
            _mockRestProvider
                .Setup(m => m.GetAsync<ResponseModel>(It.IsAny<string>()))
                .ReturnsAsync(new ResponseModel());
            string nameFilter = "Fake Name";

            // Act
            await _unitUnderTest.Search(nameFilter, 1);

            // Assert
            _mockRestProvider.Verify(
                m => m.GetAsync<ResponseModel>(It.Is<string>(
                    url => url.Contains($"name={nameFilter}"))));
        }

        [Test]
        public void Search_WhenGetAsyncFails_ShouldThrowAndLog()
        {
            // Arrange
            _mockRestProvider
                .Setup(m => m.GetAsync<ResponseModel>(It.IsAny<string>()))
                .Throws<FakeRestException>();

            // Act & Assert
            Assert.ThrowsAsync<FakeRestException>(async () => await _unitUnderTest.Search(null, 1));
            _mockStatusManager.Verify(m => m.ReportException(It.IsAny<string>(), It.IsAny<FakeRestException>()));
        }

        [Test]
        public async Task Create_WhenUsingValidUserAsParameter_ShouldCallTheServiceUsingTheSpecificUser()
        {
            // Arrange
            var specificUser = new UserModel{ Name = Guid.NewGuid().ToString() };

            _mockRestProvider
                .Setup(m => m.PostAsync<ResponseModel, UserModel>(It.IsAny<string>(), specificUser))
                .ReturnsAsync(new ResponseModel());

            // Act
            await _unitUnderTest.Create(specificUser);

            // Assert
            _mockRestProvider
                .Verify(m => m.PostAsync<ResponseModel, UserModel>(It.IsAny<string>(), specificUser));
        }

        [Test]
        public void Create_WhenPostAsyncFails_ShouldThrowAndLog()
        {
            // Arrange
            var fakeUser = new UserModel();

            _mockRestProvider
                .Setup(m => m.PostAsync<ResponseModel, UserModel>(It.IsAny<string>(), fakeUser))
                .Throws<FakeRestException>();

            // Act & Assert
            Assert.ThrowsAsync<FakeRestException>(async () => await _unitUnderTest.Create(fakeUser));
            _mockStatusManager.Verify(m => m.ReportException(It.IsAny<string>(), It.IsAny<FakeRestException>()));
        }

        [Test]
        public void Update_WhenPatchAsyncFails_ShouldThrowAndLog()
        {
            // Arrange
            var fakeUser = new UserModel();

            _mockRestProvider
                .Setup(m => m.PatchAsync<ResponseModel, UserModel>(It.IsAny<string>(), fakeUser))
                .Throws<FakeRestException>();

            // Act & Assert
            Assert.ThrowsAsync<FakeRestException>(async () => await _unitUnderTest.Update(fakeUser));
            _mockStatusManager.Verify(m => m.ReportException(It.IsAny<string>(), It.IsAny<FakeRestException>()));
        }

        //*
        [Test]
        public async Task Update_WhenSpecificUserIdIsProvided_ShouldIncludeItIntoTheUrl()
        {
            // Arrange
            uint specificId = (uint)DateTime.Now.Millisecond;
            var userToBeUpdated = new UserModel { Id = specificId };
            _mockRestProvider
                .Setup(m => m.PatchAsync<ResponseModel, UserModel>(It.IsAny<string>(), userToBeUpdated))
                .ReturnsAsync(new ResponseModel());

            // Act
            await _unitUnderTest.Update(userToBeUpdated);

            // Assert
            _mockRestProvider.Verify(
                m => m.PatchAsync<ResponseModel, UserModel>(It.Is<string>(
                    url => url.Contains($"/{specificId}")), userToBeUpdated));
        }

        [Test]
        public void Delete_WhenDeleteAsyncFails_ShouldThrowAndLog()
        {
            // Arrange
            _mockRestProvider
                .Setup(m => m.DeleteAsync<ResponseModel>(It.IsAny<string>()))
                .Throws<FakeRestException>();

            // Act & Assert
            Assert.ThrowsAsync<FakeRestException>(async () => await _unitUnderTest.Delete(1));
            _mockStatusManager.Verify(m => m.ReportException(It.IsAny<string>(), It.IsAny<FakeRestException>()));
        }

        //*
        [Test]
        public async Task Delete_WhenSpecificIdIsProvided_ShouldIncludeItIntoTheUrl()
        {
            // Arrange
            uint specificId = (uint)DateTime.Now.Millisecond;

            // Act
            await _unitUnderTest.Delete(specificId);

            // Assert
            _mockRestProvider.Verify(
                m => m.DeleteAsync<ResponseModel>(It.Is<string>(
                    url => url.Contains($"/{specificId}"))));
        }
    }

    internal class FakeRestException : Exception { }
}
