using oktaMFA.Models;

namespace oktaMFA.Service
{
    public interface IMfaService
    {
        Task<PasscodeResponse> ActivateOtp(PasscodeRequest request);

        Task<FactorResponse> VerifyOtp(string userId, string factorId, string passcode);

        Task<FactorEnrollResponse> EnrollUser(string userId , FactorPayload factor);

        Task<List<ListOfFactorResponse>>ListofFactor(string userId );
    }
}
