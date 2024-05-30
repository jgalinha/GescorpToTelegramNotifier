using GescorpToTelegramNotifier.Host.Dtos;
using Refit;

namespace GescorpToTelegramNotifier.Host.Services {
    public interface IGescorpApi {
        [Get("/authentication")]
        Task<AuthenticationResponseDto> GetAuthenticationAsync();
    }
}
