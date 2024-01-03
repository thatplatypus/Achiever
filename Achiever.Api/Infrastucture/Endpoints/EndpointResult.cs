﻿using OneOf;

namespace Achiever.Api.Infrastucture.Endpoints
{

    [GenerateOneOf]
    public partial class EndpointResult<TResponse> : OneOfBase<TResponse, ValidationError, Unauthorized> { }

    public record ValidationError(string Message);

    public record Unauthorized;
}
