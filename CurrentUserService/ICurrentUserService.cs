
using System.Threading.Tasks;

using MovieMania.Contracts.CurrentUserService.GetCurrentUser;
using MovieMania.Infrastructure;

namespace CurrentUserService
{
    public interface ICurrentUserService
    {
        string GetUserId();
        string GetUserId(string token);
        Task<ResultModel<GetCurrentUserResponse>> GetCurrentUser();
    }
}
