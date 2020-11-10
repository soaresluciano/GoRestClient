using GoRestClient.Core;
using GoRestClient.Models;
using GoRestClient.Services;
using GoRestClient.Services.Models;
using GoRestClient.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using Prism.Commands;

namespace GoRestClient.Tests.ViewModels
{
    [TestFixture]
    public class MainWindowViewModelTests
    {
        private Mock<IUserService> _mockUserService;
        private Mock<IStatusManager> _mockStatusManager;
        MainWindowViewModel _unitUnderTest;

        [SetUp]
        public void SetUp()
        {
            this._mockUserService = new Mock<IUserService>();
            this._mockStatusManager = new Mock<IStatusManager>();
            _unitUnderTest = new MainWindowViewModel(
                this._mockUserService.Object,
                this._mockStatusManager.Object);
        }

        [Test]
        public void SearchCommand_WhenExecutesAndFetchesTheResultSuccessfully_ShouldLogUpdatePaginationAndCollection()
        {
            // Arrange
            var expectedResult = new SearchResultModel
            {
                Pagination = new PaginationModel(),
                Records = new[] { GenerateFakeUser(), GenerateFakeUser() }
            };

            _mockUserService.Setup(m => m.Search(null, 1)).ReturnsAsync(expectedResult);

            // Act
            _unitUnderTest.SearchCommand.Execute();

            // Assert
            _mockStatusManager.Verify(m=>m.ReportInfo(It.IsAny<string>()));
            Assert.AreEqual(expectedResult.Pagination, _unitUnderTest.Pagination);
            CollectionAssert.AreEqual(expectedResult.Records, _unitUnderTest.UsersCollection);
        }


        [Test]
        public void SearchCommand_WhenExecutesAndTheResultFetchFails_ShouldLogAndWipePaginationAndCollection()
        {
            // Arrange
            var expectedException = new FakeServiceException();
            _mockUserService.Setup(m => m.Search(null, 1)).Throws(expectedException);

            // Act 
            _unitUnderTest.SearchCommand.Execute();

            // Assert
            _mockStatusManager.Verify(m => m.ReportException(It.IsAny<string>(), expectedException));
            Assert.IsNull(_unitUnderTest.Pagination);
            Assert.IsTrue(_unitUnderTest.UsersCollection.Count == 0);
        }

        [Test]
        public void InsertCommand_WhenExecutesAndCreatesSuccessfully_ShouldLogUpdatePaginationAndCollection()
        {
            // Arrange
            var inputUser = new UserModel();
            var expectedUser = GenerateFakeUser();

            _mockUserService.Setup(m => m.Create(inputUser)).ReturnsAsync(expectedUser);

            _unitUnderTest.SelectedUser = inputUser;

            // Act
            _unitUnderTest.InsertCommand.Execute();

            // Assert
            _mockStatusManager.Verify(m => m.ReportInfo(It.IsAny<string>()));
            Assert.AreEqual(expectedUser, _unitUnderTest.SelectedUser);
            CollectionAssert.Contains(_unitUnderTest.UsersCollection, expectedUser);
        }

        [Test]
        public void InsertCommand_WhenExecutesAndFailsToCreate_ShouldLog()
        {
            // Arrange
            var expectedException = new FakeServiceException();

            _mockUserService.Setup(m => m.Create(It.IsAny<UserModel>())).Throws(expectedException);

            // Act
            _unitUnderTest.InsertCommand.Execute();

            // Assert
            _mockStatusManager.Verify(m => m.ReportException(It.IsAny<string>(), expectedException));
        }

        [Test]
        public void UpdateCommand_WhenExecutesAndUpdatesSuccessfully_ShouldLogUpdatePaginationAndCollection()
        {
            // Arrange
            var inputUser = new UserModel();
            var expectedUser = GenerateFakeUser();

            _mockUserService.Setup(m => m.Update(inputUser)).ReturnsAsync(expectedUser);

            _unitUnderTest.SelectedUser = inputUser;

            // Act
            _unitUnderTest.UpdateCommand.Execute();

            // Assert
            _mockStatusManager.Verify(m => m.ReportInfo(It.IsAny<string>()));
            Assert.AreEqual(expectedUser, _unitUnderTest.SelectedUser);
        }

        [Test]
        public void UpdateCommand_WhenExecutesAndFailsToUpdate_ShouldLog()
        {
            // Arrange
            var expectedException = new FakeServiceException();

            _mockUserService.Setup(m => m.Update(It.IsAny<UserModel>())).Throws(expectedException);

            // Act
            _unitUnderTest.UpdateCommand.Execute();

            // Assert
            _mockStatusManager.Verify(m => m.ReportException(It.IsAny<string>(), expectedException));
        }

        [Test]
        public void DeleteCommand_WhenExecutesAndDeletesSuccessfully_ShouldLogUpdatePaginationAndCollection()
        {
            // Arrange
            var expectedUser = GenerateFakeUser();
            _unitUnderTest.SelectedUser = expectedUser;
            _unitUnderTest.UsersCollection.Add(expectedUser);

            // Act
            _unitUnderTest.DeleteCommand.Execute();

            // Assert
            _mockStatusManager.Verify(m => m.ReportInfo(It.IsAny<string>()));
            Assert.AreNotEqual(expectedUser, _unitUnderTest.SelectedUser);
            CollectionAssert.DoesNotContain(_unitUnderTest.UsersCollection, expectedUser);
        }

        [Test]
        public void DeleteCommand_WhenExecutesAndFailsToCreate_ShouldLog()
        {
            // Arrange
            var expectedException = new FakeServiceException();

            _mockUserService.Setup(m => m.Delete(It.IsAny<uint>())).Throws(expectedException);

            // Act
            _unitUnderTest.DeleteCommand.Execute();

            // Assert
            _mockStatusManager.Verify(m => m.ReportException(It.IsAny<string>(), expectedException));
        }

        [Test]
        public void FirstPageCommand_WhenExecutes_ShouldCallSearchPassingPage1()
        {
            // Act & Assert
            PaginationCommands_WhenExecutesWithCurrentPageIsAsSet_ShouldCallSearchUsingExpectedPageAsSet(
                _unitUnderTest.FirstPageCommand, UInt32.MaxValue, 1);
        }

        [TestCase(1U, false)]
        [TestCase(UInt32.MaxValue, true)]
        public void FirstPageCommand_WhenCanExecuteWithCurrentPageAsSet_ShouldReturnExpectedValueAsSet(uint currentPage, bool expected)
        {
            PaginationCommands_WhenCanExecuteWithCurrentPageIsAsSet_ShouldReturnExpectedValueAsSet(
                _unitUnderTest.FirstPageCommand, currentPage, expected);
        }

        [Test]
        public void PreviousPageCommand_WhenExecutesWithCurrentPageIs10_ShouldCallSearchPassingPage9()
        {
            // Act & Assert
            PaginationCommands_WhenExecutesWithCurrentPageIsAsSet_ShouldCallSearchUsingExpectedPageAsSet(
                _unitUnderTest.PreviousPageCommand, 10, 9);
        }

        [TestCase(1U, false)]
        [TestCase(UInt32.MaxValue, true)]
        public void PreviousPageCommand_WhenCanExecuteWithCurrentPageAsSet_ShouldReturnExpectedValueAsSet(uint currentPage, bool expected)
        {
            PaginationCommands_WhenCanExecuteWithCurrentPageIsAsSet_ShouldReturnExpectedValueAsSet(
                _unitUnderTest.PreviousPageCommand, currentPage, expected);
        }

        [Test]
        public void NextPageCommand_WhenExecutesWithCurrentPageIs10_ShouldCallSearchPassingPage11()
        {
            // Act & Assert
            PaginationCommands_WhenExecutesWithCurrentPageIsAsSet_ShouldCallSearchUsingExpectedPageAsSet(
                _unitUnderTest.NextPageCommand, 10, 11);
        }

        [TestCase(1U, true)]
        [TestCase(UInt32.MaxValue, false)]
        public void NextPageCommand_WhenCanExecuteWithCurrentPageAsSet_ShouldReturnExpectedValueAsSet(uint currentPage, bool expected)
        {
            PaginationCommands_WhenCanExecuteWithCurrentPageIsAsSet_ShouldReturnExpectedValueAsSet(
                _unitUnderTest.NextPageCommand, currentPage, expected);
        }

        [Test]
        public void LastPageCommand_WhenExecutes_ShouldCallSearchPassingLastPage()
        {
            // Act & Assert
            PaginationCommands_WhenExecutesWithCurrentPageIsAsSet_ShouldCallSearchUsingExpectedPageAsSet(
                _unitUnderTest.LastPageCommand, 1, UInt32.MaxValue);
        }

        [TestCase(1U, true)]
        [TestCase(UInt32.MaxValue, false)]
        public void LastPageCommand_WhenCanExecuteWithCurrentPageAsSet_ShouldReturnExpectedValueAsSet(uint currentPage, bool expected)
        {
            PaginationCommands_WhenCanExecuteWithCurrentPageIsAsSet_ShouldReturnExpectedValueAsSet(
                _unitUnderTest.NextPageCommand, currentPage, expected);
        }

        private void PaginationCommands_WhenExecutesWithCurrentPageIsAsSet_ShouldCallSearchUsingExpectedPageAsSet(
            DelegateCommand command, uint currentPage, uint expectedPage)
        {
            //Arrange 
            _unitUnderTest.Pagination = new PaginationModel
            {
                Page = currentPage,
                Pages = UInt32.MaxValue
            };

            // Act
            command.Execute();

            //Assert
            _mockUserService.Verify(m => m.Search(null, expectedPage));
        }

        private void PaginationCommands_WhenCanExecuteWithCurrentPageIsAsSet_ShouldReturnExpectedValueAsSet(
            DelegateCommand command, uint currentPage, bool expected)
        {
            // Arrange 
            _unitUnderTest.Pagination = new PaginationModel
            {
                Page = currentPage,
                Pages = UInt32.MaxValue
            };

            // Act
            bool actual = command.CanExecute();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private UserModel GenerateFakeUser()
        {
            return new UserModel
            {
                Id = (uint)DateTime.Now.Millisecond,
                Name = Guid.NewGuid().ToString(),
                Created = DateTimeOffset.Now,
                Updated = DateTimeOffset.Now
            };
        }
    }

    internal class FakeServiceException : Exception { }
}
