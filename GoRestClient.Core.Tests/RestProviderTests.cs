using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoRestClient.Core.Tests
{
    [TestFixture]
    public class RestProviderTests
    {
        private const string FakeResourceUrl = "FakeResource";
        private const int FakeContent = 1234;

        private RestProvider _unitUnderTest;
        private HttpClient _fakeHttpClient;
        private FakeHttpMessageHandler _fakeHttpHandler;
        private readonly Mock<IConfigurationProvider> _mockConfigurationProvider;
        private Mock<IJsonProvider> _mockJsonProvider;
        private Mock<IStatusManager> _mockStatusManager;

        public RestProviderTests()
        {
            _mockConfigurationProvider = new Mock<IConfigurationProvider>();
            _mockConfigurationProvider.SetupGet(m => m.ApiUrl).Returns("http://xpto/");
            _mockConfigurationProvider.SetupGet(m => m.ApiToken).Returns("FakeToken");
        }

        [SetUp]
        public void SetUp()
        {
            _mockJsonProvider = new Mock<IJsonProvider>();
            _mockJsonProvider.Setup(m => m.Deserialize<int>(It.IsAny<string>())).Returns<string>(int.Parse);
            _mockJsonProvider.Setup(m => m.Serialize(It.IsAny<int>())).Returns<int>(r => r.ToString());
            _mockStatusManager = new Mock<IStatusManager>();

            _fakeHttpHandler = new FakeHttpMessageHandler();
            _fakeHttpClient = new HttpClient(_fakeHttpHandler);
            _unitUnderTest = new RestProvider(
                _mockConfigurationProvider.Object,
                _mockJsonProvider.Object,
                _mockStatusManager.Object,
                _fakeHttpClient);
        }

        protected static HttpResponseMessage CreateResponseMessage(HttpStatusCode status = HttpStatusCode.OK, string expectedContent = "")
        {
            return new HttpResponseMessage { StatusCode = status, Content = new StringContent(expectedContent, Encoding.UTF8) };
        }

        protected void WhenTheUriISNullOrEmpty_ShouldThrowArgumentNullException(Func<Task> task)
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await task());
        }

        protected async Task WhenParametersAreValid_ShouldUseTheGetAction(Func<Task> task, HttpMethod method)
        {
            // Arrange
            _fakeHttpHandler.NextResponse = CreateResponseMessage(HttpStatusCode.OK, FakeContent.ToString());

            // Act
            await task();

            // Assert
            Assert.AreEqual(method, _fakeHttpHandler.LastRequest.Method);
        }

        protected async Task WhenStatusCodeIsOK_ShouldReturnExpectedContentBack(Func<Task<int>> task)
        {
            // Arrange
            _fakeHttpHandler.NextResponse = CreateResponseMessage(HttpStatusCode.OK, FakeContent.ToString());

            // Act
            var result = await task();

            // Assert
            Assert.AreEqual(FakeContent, result);
        }

        protected void WhenSendRequestFails_ShouldThrowAndLog(Func<Task> task)
        {
            //Arrange
            _fakeHttpHandler.NextRequestMustThrow = true;

            // Act & Assert
            Assert.ThrowsAsync<FakeRequestException>(async () => await task());
            _mockStatusManager.Verify(m => m.ReportException(It.IsAny<string>(), It.IsAny<Exception>()));
        }

        protected void WhenDeserializationFails_ShouldThrowAndLog(Func<Task> task)
        {
            //Arrange
            _mockJsonProvider
                .Setup(m => m.Deserialize<int>(It.IsAny<string>()))
                .Throws(new FakeJsonException());

            // Act & Assert
            Assert.ThrowsAsync<FakeJsonException>(async () => await task());
            _mockStatusManager.Verify(m => m.ReportException(It.IsAny<string>(), It.IsAny<Exception>()));
        }

        protected void WhenSerializationFails_ShouldThrowAndLog(Func<Task> task)
        {
            //Arrange
            _mockJsonProvider
                .Setup(m => m.Serialize(It.IsAny<int>()))
                .Throws(new FakeJsonException());

            // Act & Assert
            Assert.ThrowsAsync<FakeJsonException>(async () => await task());
            _mockStatusManager.Verify(m => m.ReportException(It.IsAny<string>(), It.IsAny<Exception>()));
        }

        [TestFixture]
        public class GetAsyncMethod : RestProviderTests
        {
            [TestCase(null)]
            [TestCase("")]
            public void WhenTheUriISNullOrEmpty_ShouldThrowArgumentNullException(string requestUri)
            {
                WhenTheUriISNullOrEmpty_ShouldThrowArgumentNullException(() => _unitUnderTest.GetAsync<int>(requestUri));
            }

            [Test]
            public async Task WhenParametersAreValid_ShouldUseTheGetAction()
            {
                await WhenParametersAreValid_ShouldUseTheGetAction(() => _unitUnderTest.GetAsync<int>(FakeResourceUrl), HttpMethod.Get);
            }

            [Test]
            public async Task WhenStatusCodeIsOK_ShouldReturnExpectedContentBack()
            {
                await WhenStatusCodeIsOK_ShouldReturnExpectedContentBack(() => _unitUnderTest.GetAsync<int>(FakeResourceUrl));
            }

            [Test]
            public void WhenSendRequestFails_ShouldThrowAndLog()
            {
                WhenSendRequestFails_ShouldThrowAndLog(() => _unitUnderTest.GetAsync<int>(FakeResourceUrl));
            }

            [Test]
            public void WhenDeserializationFails_ShouldThrowAndLog()
            {
                WhenDeserializationFails_ShouldThrowAndLog(() => _unitUnderTest.GetAsync<int>(FakeResourceUrl));
            }
        }

        [TestFixture]
        public class PostAsyncMethod : RestProviderTests
        {
            [TestCase(null)]
            [TestCase("")]
            public void WhenTheUriISNullOrEmpty_ShouldThrowArgumentNullException(string requestUri)
            {
                WhenTheUriISNullOrEmpty_ShouldThrowArgumentNullException(() => _unitUnderTest.PostAsync<int, int>(requestUri, FakeContent));
            }

            [Test]
            public async Task Always_ShouldUseTheGetAction()
            {
                await WhenParametersAreValid_ShouldUseTheGetAction(() => _unitUnderTest.PostAsync<int, int>(FakeResourceUrl, FakeContent), HttpMethod.Post);
            }

            [Test]
            public async Task WhenStatusCodeIsOK_ShouldReturnExpectedContentBack()
            {
                await WhenStatusCodeIsOK_ShouldReturnExpectedContentBack(() => _unitUnderTest.PostAsync<int, int>(FakeResourceUrl, FakeContent));
            }

            [Test]
            public void WhenSendRequestFails_ShouldThrowAndLog()
            {
                WhenSendRequestFails_ShouldThrowAndLog(() => _unitUnderTest.PostAsync<int, int>(FakeResourceUrl, FakeContent));
            }

            [Test]
            public void WhenDeserializationFails_ShouldThrowAndLog()
            {
                WhenDeserializationFails_ShouldThrowAndLog(() => _unitUnderTest.PostAsync<int, int>(FakeResourceUrl, FakeContent));
            }

            [Test]
            public void WhenSerializationFails_ShouldThrowAndLog()
            {
                WhenSerializationFails_ShouldThrowAndLog(()=> _unitUnderTest.PostAsync<int, int>(FakeResourceUrl, FakeContent));
            }
        }

        [TestFixture]
        public class PatchAsyncMethod : RestProviderTests
        {
            [TestCase(null)]
            [TestCase("")]
            public void WhenTheUriISNullOrEmpty_ShouldThrowArgumentNullException(string requestUri)
            {
                WhenTheUriISNullOrEmpty_ShouldThrowArgumentNullException(() => _unitUnderTest.PatchAsync<int, int>(requestUri, FakeContent));
            }

            [Test]
            public async Task Always_ShouldUseTheGetAction()
            {
                await WhenParametersAreValid_ShouldUseTheGetAction(() => _unitUnderTest.PatchAsync<int, int>(FakeResourceUrl, FakeContent), HttpMethod.Patch);
            }

            [Test]
            public async Task WhenStatusCodeIsOK_ShouldReturnExpectedContentBack()
            {
                await WhenStatusCodeIsOK_ShouldReturnExpectedContentBack(() => _unitUnderTest.PatchAsync<int, int>(FakeResourceUrl, FakeContent));
            }

            [Test]
            public void WhenSendRequestFails_ShouldThrowAndLog()
            {
                WhenSendRequestFails_ShouldThrowAndLog(() => _unitUnderTest.PatchAsync<int, int>(FakeResourceUrl, FakeContent));
            }

            [Test]
            public void WhenDeserializationFails_ShouldThrowAndLog()
            {
                WhenDeserializationFails_ShouldThrowAndLog(() => _unitUnderTest.PatchAsync<int, int>(FakeResourceUrl, FakeContent));
            }

            [Test]
            public void WhenSerializationFails_ShouldThrowAndLog()
            {
                WhenSerializationFails_ShouldThrowAndLog(() => _unitUnderTest.PatchAsync<int, int>(FakeResourceUrl, FakeContent));
            }
        }

        [TestFixture]
        public class DeleteWithResponseAsyncMethod : RestProviderTests
        {
            [TestCase(null)]
            [TestCase("")]
            public void WhenTheUriISNullOrEmpty_ShouldThrowArgumentNullException(string requestUri)
            {
                WhenTheUriISNullOrEmpty_ShouldThrowArgumentNullException(() => _unitUnderTest.DeleteAsync<int>(requestUri));
            }

            [Test]
            public async Task Always_ShouldUseTheGetAction()
            {
                await WhenParametersAreValid_ShouldUseTheGetAction(() => _unitUnderTest.DeleteAsync<int>(FakeResourceUrl), HttpMethod.Delete);
            }

            [Test]
            public async Task WhenStatusCodeIsOK_ShouldReturnExpectedContentBack()
            {
                await WhenStatusCodeIsOK_ShouldReturnExpectedContentBack(() => _unitUnderTest.DeleteAsync<int>(FakeResourceUrl));
            }

            [Test]
            public void WhenSendRequestFails_ShouldThrowAndLog()
            {
                WhenSendRequestFails_ShouldThrowAndLog(() => _unitUnderTest.DeleteAsync<int>(FakeResourceUrl));
            }

            [Test]
            public void WhenDeserializationFails_ShouldThrowAndLog()
            {
                WhenDeserializationFails_ShouldThrowAndLog(() => _unitUnderTest.DeleteAsync<int>(FakeResourceUrl));
            }
        }
    }

    internal class FakeHttpMessageHandler : HttpMessageHandler
    {
        public HttpResponseMessage NextResponse { get; set; }
        public HttpRequestMessage LastRequest { get; private set; }
        public bool NextRequestMustThrow { get; set; } = false;

        public virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            LastRequest = request;
            return NextResponse ??
                   new HttpResponseMessage
                   {
                       StatusCode = HttpStatusCode.OK,
                       Content = new StringContent(string.Empty, Encoding.UTF8)
                   };
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (NextRequestMustThrow) throw new FakeRequestException();
            return Task.FromResult(Send(request));
        }
    }

    internal class FakeRequestException : Exception
    {
        internal FakeRequestException() : base("This request was configured to fail.") { }
    }

    internal class FakeJsonException : Exception
    {
        internal FakeJsonException() : base("This json operation was configured to fail.") { }
    }
}
