using Achiever.Infrastucture.Extensions;
using Xunit;
using Moq;
using System.Security.Claims;
using System.Threading;
using Achiever.Infrastucture.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Achiever.Tests.Fakes;

namespace Achiever.Tests.Unit
{


    public class EndpointRouteBuilderExtensionsTests
    {
        [Fact]
        public async Task Handle_Returns_Ok_When_Endpoint_Returns_Response()
        {
            // Arrange
            var request = new MockRequest();
            var response = new MockResponse();
            var endpointResult = new EndpointResult<MockResponse>(response);
            var endpoint = new Mock<IEndpoint<MockRequest, MockResponse>>();
            endpoint.Setup(e => e.Handle(request, It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(endpointResult);

            // Act
            var result = await EndpointRouteBuilderExtensions.Handle(request, endpoint.Object, new ClaimsPrincipal(), new CancellationToken());

            // Assert
            Assert.IsType<Ok<MockResponse>>(result.Result);
            Assert.Equal(response, ((Ok<MockResponse>)result.Result).Value);
        }

        [Fact]
        public async Task Handle_Returns_NotFound_When_Endpoint_Returns_ValidationError_With_NotFound_Message()
        {
            // Arrange
            var request = new MockRequest();
            var validationError = new ValidationError("not found");
            var endpoint = new Mock<FakeEndpoint>();
            endpoint.Setup(e => e.Handle(request, It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationError);

            // Act
            var result = await EndpointRouteBuilderExtensions.Handle(request,endpoint.Object, new ClaimsPrincipal(), new CancellationToken());

            // Assert
            Assert.IsType<NotFound<ValidationError>>(result.Result);
        }

        [Fact]
        public async Task Handle_Returns_BadRequest_When_Endpoint_Returns_ValidationError()
        {
            // Arrange
            var request = new MockRequest();
            var validationError = new ValidationError("");
            var endpointResult = new EndpointResult<MockResponse>(validationError);
            var endpoint = new Mock<IEndpoint<MockRequest, MockResponse>>();
            endpoint.Setup(e => e.Handle(request, It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(endpointResult);

            // Act
            var result = await EndpointRouteBuilderExtensions.Handle(request, endpoint.Object, new ClaimsPrincipal(), new CancellationToken());

            // Assert
            Assert.IsType<BadRequest<ValidationError>>(result.Result);
            Assert.Equal(validationError, ((BadRequest<ValidationError>)result.Result).Value);
        }

        [Fact]
        public async Task Handle_Returns_Unauthorized_When_Endpoint_Returns_Unauthorized()
        {
            // Arrange
            var request = new MockRequest();
            var unauthorized = new Unauthorized();
            var endpointResult = new EndpointResult<MockResponse>(unauthorized);
            var endpoint = new Mock<IEndpoint<MockRequest, MockResponse>>();
            endpoint.Setup(e => e.Handle(request, It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(endpointResult);

            // Act
            var result = await EndpointRouteBuilderExtensions.Handle(request, endpoint.Object, new ClaimsPrincipal(), new CancellationToken());

            // Assert
            Assert.IsType<UnauthorizedHttpResult>(result.Result);
        }
    }
}
