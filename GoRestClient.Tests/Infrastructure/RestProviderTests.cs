using GoRestClient.Infrastructure;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoRestClient.Tests.Infrastructure
{
    [TestFixture]
    public class RestProviderTests
    {
        private const string FakeResourceUrl = "FakeResource";
        private const int FakeContent = 1234;

        private RestProvider _unitUnderTest;
        private HttpClient _httpClient;
        private FakeHttpMessageHandler _fakeHttpHandler;
        readonly Mock<IConfigurationProvider> _configurationProviderMock;

        public RestProviderTests()
        {
            _configurationProviderMock = new Mock<IConfigurationProvider>();
            _configurationProviderMock.SetupGet(m => m.ApiUrl).Returns("http://xpto/");
            _configurationProviderMock.SetupGet(m => m.ApiToken).Returns("FakeToken");
        }

        [SetUp]
        public void SetUp()
        {
            _fakeHttpHandler = new FakeHttpMessageHandler();
            _httpClient = new HttpClient(_fakeHttpHandler);
            _unitUnderTest = new RestProvider(_configurationProviderMock.Object, _httpClient);
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
        }
    }

    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        public HttpResponseMessage NextResponse { get; set; }
        public HttpRequestMessage LastRequest { get; private set; }

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
            return Task.FromResult(Send(request));
        }
    }
}
