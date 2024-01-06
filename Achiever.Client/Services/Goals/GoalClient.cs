using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Achiever.Client.Models;
using Achiever.Client.Services.Goals;
using Achiever.Shared.Goals.Endpoints;
using Achiever.Shared.Goals.ViewModels;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using static Achiever.Shared.Goals.Endpoints.CreateGoalRequestModel;
using static Achiever.Shared.Goals.Endpoints.CreateSubtaskRequestModel;
using static Achiever.Shared.Goals.Endpoints.DeleteGoalRequestModel;
using static Achiever.Shared.Goals.Endpoints.GetGoalByIdRequestModel;
using static Achiever.Shared.Goals.Endpoints.GetGoalsRequestModel;
using static Achiever.Shared.Goals.Endpoints.UpdateGoalRequestModel;
using static Achiever.Shared.Goals.Requests.DeleteSubtaskRequestModels;

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
                    return new ErrorResult<Goal>($"failed with {await response.Content.ReadAsStringAsync()}");

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
                    return new ErrorResult<Guid?>($"failed with {await response.Content.ReadAsStringAsync()}");

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
                var response = await _httpClient.PutAsJsonAsync("UpdateGoal", request);

                if (!response.IsSuccessStatusCode)
                    return new ErrorResult<Guid?>($"failed with {await response.Content.ReadAsStringAsync()}");

                var updatedGoal = await response.Content.ReadFromJsonAsync<UpdateGoalResponse>();

                return new SuccessResult<Guid?>(updatedGoal?.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex?.Message);
                return new ErrorResult<Guid?>(ex.Message);
            }
        }

        public async Task<ClientResult<bool?>> DeleteGoalAsync(Guid goalId)
        {
            var request = new DeleteGoalRequest(goalId);

            try
            {
                var response = await _httpClient.PostAsJsonAsync("DeleteGoal", request);

                if (!response.IsSuccessStatusCode)
                    return new ErrorResult<bool?>($"failed with {response.StatusCode}");

                var content = await response.Content.ReadFromJsonAsync<DeleteGoalResponse>();

                return new SuccessResult<bool?>(content.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex?.Message);
                return new ErrorResult<bool?>(ex.Message);
            }
        }

        public async Task<ClientResult<bool?>> DeleteSubTaskAsync(Guid subTaskId)
        {
            var request = new DeleteSubtaskRequest(subTaskId);

            try
            {
                var response = await _httpClient.PostAsJsonAsync("DeleteSubTask", request);

                if (!response.IsSuccessStatusCode)
                    return new ErrorResult<bool?>($"failed with {response.StatusCode}");

                var content = await response.Content.ReadFromJsonAsync<DeleteSubtaskResponse>();

                return new SuccessResult<bool?>(content.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex?.Message);
                return new ErrorResult<bool?>(ex.Message);
            }
        }

        public async Task<ClientResult<Guid>> CreateSubtaskAsync(Guid goalId, SubTask subTask)
        {
            var request = new CreateSubtaskRequest(goalId, subTask);
            try
            {
                var response = await _httpClient.PostAsJsonAsync("CreateSubTask", request);

                if (!response.IsSuccessStatusCode)
                    return new ErrorResult<Guid>($"failed with {response.StatusCode}");

                var content = await response.Content.ReadFromJsonAsync<CreateSubtaskResponse>();

                return new SuccessResult<Guid>(content!.SubtaskId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex?.Message);
                return new ErrorResult<Guid>(ex?.Message ?? "Unknown error");
            }
        }
    }
}
