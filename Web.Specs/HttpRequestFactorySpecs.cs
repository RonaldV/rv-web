using Xunit;

namespace RV.Web.Specs
{
    // TODO fix all....!
    public class HttpRequestFactorySpecs
    {
        private const string BaseUri = "http://localhost/";
        private const string FirstSegment = "one";
        private const string SecondSegment = "two";
        private readonly HttpRequestFactory httpRequestFactory;

        public HttpRequestFactorySpecs()
        {
            httpRequestFactory = new HttpRequestFactory(BaseUri, HttpContentType.JSON);
        }

        [Fact]
        public void Given_segments_when_creating_then_needs_to_have_a_correct_uri()
        {
            var request = httpRequestFactory.Create(FirstSegment, SecondSegment);
            Assert.Equal(request.RequestUri.ToString(), string.Concat(BaseUri, FirstSegment, "/", SecondSegment));
        }
    }
}
