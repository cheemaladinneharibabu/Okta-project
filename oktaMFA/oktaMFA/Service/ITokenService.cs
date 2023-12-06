using oktaMFA.Models;

namespace oktaMFA.Service
{
    public interface ITokenService
    {
        Task<OktaResponse> GetToken(string username, string password);

        Task<List<UserResponse>> GetUsers();

        
    }
}
