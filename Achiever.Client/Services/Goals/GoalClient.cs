using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Achiever.Client.Models;
using Achiever.Client.Services.Goals;
using Achiever.Shared.Goals;
using static Achiever.Shared.Goals.Endpoints.CreateGoalRequestModel;
using static Achiever.Shared.Goals.Endpoints.GetGoalsRequestModel;

namespace Achiever.Client.Services.Goals
{
    public class GoalClient(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<Goal>> GetGoalsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<GetGoalsResponse>("GetGoals");

            return response.Goals.ToList();
        }

        public async Task<ClientResult<Guid?>> CreateGoalAsync(Goal goal)
        {
            var request = new CreateGoalRequest(goal);

            var response = await _httpClient.PostAsJsonAsync("CreateGoal", request);

            var result = new ClientResult<Guid?>();

            if (!response.IsSuccessStatusCode)
            {
                result.IsSuccess = false;
                result.Message = await response.Content.ReadAsStringAsync();
                result.Value = null;
                return result;
            }

            var newGoal = await response.Content.ReadFromJsonAsync<CreateGoalResponse>();

            result.IsSuccess = true;
            result.Value = newGoal.Id;

            return result;
        }
    }
}
