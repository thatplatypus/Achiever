using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Achiever.Client.Models;
using Achiever.Client.Services.Goals;
using Achiever.Shared.Goals;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using static Achiever.Shared.Goals.Endpoints.CreateGoalRequestModel;
using static Achiever.Shared.Goals.Endpoints.GetGoalByIdRequestModel;
using static Achiever.Shared.Goals.Endpoints.GetGoalsRequestModel;
using static Achiever.Shared.Goals.Endpoints.UpdateGoalRequestModel;

namespace Achiever.Client.Services.Goals
{
    public class GoalClient(HttpClient httpClient,
        ILogger<GoalClient> logger)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<GoalClient> _logger = logger;

        public async Task<ClientResult<List<Goal>>> GetGoalsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<GetGoalsResponse>("GetGoals");

                return new SuccessResult<List<Goal>>(response.Goals.ToList());
            }
            catch (Exception ex)
            {
                return new ErrorResult<List<Goal>>(ex.Message);
            }
        }

        public async Task<ClientResult<Goal>> GetGoalByIdAsync(Guid id)
        {
            try
            {                
                var response = await _httpClient.GetAsync($"GetGoalById?request={JsonSerializer.Serialize(new GetGoalByIdRequest(id))}");

                if (!response.IsSuccessStatusCode)
                    return new ErrorResult<Goal>($"failed with {(int)response.StatusCode}:{response.StatusCode}");

                var content = await response.Content.ReadFromJsonAsync<GetGoalByIdResponse>();

                return new SuccessResult<Goal>(content.Goal);                
            }
            catch (Exception ex)
            {
                return new ErrorResult<Goal>(ex.Message);
            }
        }

        public async Task<ClientResult<Guid?>> CreateGoalAsync(Goal goal)
        {
            var request = new CreateGoalRequest(goal);

            try
            {
                var response = await _httpClient.PostAsJsonAsync("CreateGoal", request);

                if(!response.IsSuccessStatusCode)
                    return new ErrorResult<Guid?>($"failed with {response.StatusCode}");

                var newGoal = await response.Content.ReadFromJsonAsync<CreateGoalResponse>();

                return new SuccessResult<Guid?>(newGoal?.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex?.Message);
                return new ErrorResult<Guid?>(ex.Message);
            }
        }

        public async Task<ClientResult<Guid?>> UpdateGoalAsync(Goal goal)
        {
            var request = new UpdateGoalRequest(goal);

            try
            {
                var response = await _httpClient.PostAsJsonAsync("UpdateGoal", request);

                if (!response.IsSuccessStatusCode)
                    return new ErrorResult<Guid?>($"failed with {response.StatusCode}");

                var updatedGoal = await response.Content.ReadFromJsonAsync<UpdateGoalResponse>();

                return new SuccessResult<Guid?>(updatedGoal?.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex?.Message);
                return new ErrorResult<Guid?>(ex.Message);
            }
        }
    }
}
