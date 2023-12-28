using Achiever.Infrastucture.Endpoints;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Achiever.Tests.Fakes
{
    public class FakeEndpoint : IEndpoint<MockRequest, MockResponse>
    {
        public virtual async Task<EndpointResult<MockResponse>> Handle(MockRequest request, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new MockResponse());
        }

        public void Map(IEndpointRouteBuilder app)
        {
        }
    }
    public record MockRequest { }
    public record MockResponse { }
}
