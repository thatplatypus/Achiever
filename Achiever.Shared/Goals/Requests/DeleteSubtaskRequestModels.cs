using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Achiever.Shared.Goals.Requests
{
    public static class DeleteSubtaskRequestModels
    {
        public record DeleteSubtaskRequest(Guid SubtaskId);

        public record DeleteSubtaskResponse(bool Success);
    }
}
