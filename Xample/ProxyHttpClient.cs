using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Java.Lang;
using Serilog;
using Serilog.Sinks.Xamarin;

namespace XampleProxy
{
    public class ProxyHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public ProxyHttpClient()
        {
            // Set up the HttpClient with a timeout and proxy settings
            var handler = new HttpClientHandler
            {
                Proxy = GetSystemProxy()
            };

            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(10) // Set timeout to 10 seconds
            };

            // Initialize Serilog for logging
            _logger = new LoggerConfiguration()
                        .WriteTo.AndroidLog() // Write logs to Android log
                        .CreateLogger();
        }

        // Method to make a GET request to the given URL
        public async Task GetAsync(string url)
        {
            _logger.Information($"Starting request to {url}");

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.Information($"Response: {content}");
                }
                else
                {
                    _logger.Warning($"Error: {response.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                _logger.Error($"HttpRequestException: {e.Message}");
                _logger.Error($"Stack trace: {e.StackTrace}");
            }
            catch (System.Exception e)
            {
                _logger.Error($"Exception: {e.Message}");
                _logger.Error($"Stack trace: {e.StackTrace}");
            }
        }

        // Helper method to get system proxy settings
        private WebProxy GetSystemProxy()
        {
            var proxyHost = JavaSystem.GetProperty("http.proxyHost");
            var proxyPort = JavaSystem.GetProperty("http.proxyPort");

            return !string.IsNullOrEmpty(proxyHost) && !string.IsNullOrEmpty(proxyPort)
                ? new WebProxy($"{proxyHost}:{proxyPort}")
                : null;
        }
    }
}
