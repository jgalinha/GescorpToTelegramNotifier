using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Microsoft.Extensions.Options;
using GescorpToTelegramNotifier.Host.Dtos;
using Microsoft.Extensions.Caching.Memory;

namespace GescorpToTelegramNotifier.Host.Services
{
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

        public async Task AuthenticationAsync() {
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

