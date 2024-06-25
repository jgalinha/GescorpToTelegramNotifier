using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using GescorpToTelegramNotifier.Host.Dtos;
using Microsoft.Extensions.Caching.Memory;
using GescorpToTelegramNotifier.Host.Dtos.Incident;

namespace GescorpToTelegramNotifier.Host.Services {
    public sealed class GescorpApiClient {
        private readonly IGescorpApi _api;
        private readonly ILogger<GescorpApiClient> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly GescorpApiOptions _options;
        private string _accessKey = String.Empty;

        public GescorpApiClient(IGescorpApi api, ILogger<GescorpApiClient> logger, IOptions<GescorpApiOptions> options, IMemoryCache memoryCache) {
            _api = api;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memoryCache = memoryCache;
            _options = options.Value;
            _logger.LogInformation("GescorpApiClient created with the following configuration:\n" +
                                                   "\tBase URL: {BaseUrl}\n" +
                                                   "\tAPI Version: {ApiVersion}",
                                                   _options.BaseUrl,
                                                   _options.ApiVersion);
        }

        public async Task GetIncidentByIdAsync(string id) {
            await AuthenticationAsync();
            var incident = await _api.GetIncidentsAsync("incidents_list", id, id, _accessKey);
        }

        public async Task<IncidentsDto> GetIncidentsAsync(string method = "incidents_list", DateOnly? dateFrom = null, DateOnly? dateTo = null) {
            await AuthenticationAsync();
            if (!dateFrom.HasValue) {
                dateFrom = DateOnly.FromDateTime(DateTime.Now);
            }
            if (!dateTo.HasValue) {
                dateTo = DateOnly.FromDateTime(DateTime.Now);
            }
            var date = DateOnly.FromDateTime(DateTime.Now);
            var incidents = await _api.GetIncidentsAsync(method, dateFrom.ToString(), dateTo.ToString(), _accessKey);

            return incidents;
        }

        private async Task AuthenticationAsync() {
            const string cacheKey = "AuthenticationData";
            if (!_memoryCache.TryGetValue(cacheKey, out _)) {
                var authResponse = await GetAuthenticationResponseAsync();

                if (authResponse?.authentication != null) {
                    
                    SetAuthenticationData(authResponse, cacheKey);
                } else {
                    _logger.LogError("Authentication response or authentication data is null");
                }
            }
        }

        private async Task<AuthenticationResponseDto> GetAuthenticationResponseAsync() {
            var response = await _api.GetAuthenticationAsync();

            return response;
        }

        private void SetAuthenticationData(AuthenticationResponseDto authResponse, string cacheKey) {
            if (DateTime.TryParse(authResponse.authentication.expiration_date, out DateTime date)) {
                var expiration = date - DateTime.Now;

                _accessKey = authResponse.authentication.access_key;

                _memoryCache.Set(cacheKey, authResponse, expiration);

                _logger.LogInformation("Get new Access Key from Gescorp API. Expires in {Expiration} minutes", expiration.TotalMinutes);
            } else {
                _logger.LogError("Failed to parse expiration date: {ExpirationDate}", authResponse.authentication.expiration_date);
            }
        }


    }

}

