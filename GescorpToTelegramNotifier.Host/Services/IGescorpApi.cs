using GescorpToTelegramNotifier.Host.Dtos;
using GescorpToTelegramNotifier.Host.Dtos.Incident;
using Refit;

namespace GescorpToTelegramNotifier.Host.Services {
    public interface IGescorpApi {

        [Get("/incidents")]
        Task<IncidentsDto> GetIncidentByIdAsync([Query] string id, [Header("X-AccessKey")] string XAccessKey);
        [Get("/incidents")]
        Task<IncidentsDto> GetIncidentsAsync([Query] string? method, [Query] string? date_from, [Query] string? date_to, [Header("X-AccessKey")] string XAccessKey);

        [Get("/authentication")]
        Task<AuthenticationResponseDto> GetAuthenticationAsync();

    }
}
