using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Achiever.Shared.Goals.Endpoints
{
    public static class CreateGoalRequestModel
    {
        public record CreateGoalRequest(Goal Goal);

        public record CreateGoalResponse(Guid Id);
    }
}
