using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace GescorpToTelegramNotifier.Host.Services {
    public static class DependencyInjection {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) {

            var gescorpApiBaseUrl = configuration["GescorpApi:BaseUrl"] ?? throw new ArgumentNullException("GescorpApi:BaseUrl");
            var apiVersion = configuration["GescorpApi:ApiVersion"] ?? throw new ArgumentNullException("GescorpApi:ApiVersion");
            var apiKey = configuration["GescorpApi:ApiKey"] ?? throw new ArgumentNullException("GescorpApi:ApiKey");
            var apiUri = new Uri($"{gescorpApiBaseUrl}/{apiVersion}/");

            services.AddRefitClient<IGescorpApi>()
                .ConfigureHttpClient(client => {
                    client.BaseAddress = apiUri;
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("User-Agent", "GescorpToTelegramNotifier");
                    client.DefaultRequestHeaders.Add("Api-Version", apiVersion);
                    client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                });

            services.AddSingleton<GescorpApiClient>();


            return services;
        }

    }
}
