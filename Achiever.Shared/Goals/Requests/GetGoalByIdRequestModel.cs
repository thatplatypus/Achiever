
using System.Text.Json;
using System.Text.Json.Serialization;
using Achiever.Shared.Goals.ViewModels;

namespace Achiever.Shared.Goals.Endpoints
{
    public static class GetGoalByIdRequestModel
    {
        public record GetGoalByIdRequest(Guid Id)
        {
            /// <summary>
            /// This is required for the endpoint to work with the id query in the route
            /// </summary>
            public static bool TryParse(string input, out GetGoalByIdRequest result)
            {
                result = default!;
                var request = JsonSerializer.Deserialize<GetGoalByIdRequest>(input, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                if(request != null)
                {
                    result = request;
                    return true;
                }

                if (Guid.TryParse(input, out Guid id))
                {
                    result = new GetGoalByIdRequest(id);
                    return true;
                }
                return false;
            }
        }

        public record GetGoalByIdResponse(Goal Goal);
    }
}
