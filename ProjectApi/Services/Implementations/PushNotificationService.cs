using Google.Apis.Auth.OAuth2;
using ProjectApi.Services.Abstractions;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ProjectApi.Services.Implementations
{
    public class PushNotificationService : IPushNotificationService
    {
        private const string FirebaseScope = "https://www.googleapis.com/auth/firebase.messaging";

        private const string FirebaseProjectId = "sparkly-8cdd0";

        private static readonly string FcmUrl = 
            $"https://fcm.googleapis.com/v1/projects/{FirebaseProjectId}/messages:send";

        private readonly string? _serviceAccountPath;

        private readonly HttpClient _httpClient;

        public PushNotificationService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _serviceAccountPath = Environment.GetEnvironmentVariable("FIREBASE_KEY");

            if (string.IsNullOrEmpty(_serviceAccountPath))
            {
                Console.WriteLine("Warning: FIREBASE_KEY environment variable is not set.");
            }
            else if (!File.Exists(_serviceAccountPath))
            {
                Console.WriteLine($"Warning: Service account file not found at {_serviceAccountPath}");
            }

        }

        public async Task<bool> SendPushAsync(string deviceToken, string title, string body)
        {
            if (string.IsNullOrEmpty(_serviceAccountPath) || !File.Exists(_serviceAccountPath))
            {
                Console.WriteLine("Cannot send push notification: service account key missing.");
                return false;
            }

            if (string.IsNullOrEmpty(deviceToken))
            {
                Console.WriteLine("Cannot send push notification: device token is empty.");
                return false;
            }

            try
            {
                var accessToken = await GetAccessTokenAsync();

                var payload = new
                {
                    message = new
                    {
                        token = deviceToken,
                        notification = new
                        {
                            title,
                            body
                        }
                    }
                };

                var json = JsonSerializer.Serialize(payload);

                using var request = new HttpRequestMessage(HttpMethod.Post, FcmUrl);
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"{_serviceAccountPath}");
                Console.WriteLine($"token {deviceToken}");
                Console.WriteLine("FCM response: " + responseContent);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private async Task<string> GetAccessTokenAsync()
        {
            ServiceAccountCredential serviceAccountCredential;

            using (var stream = File.OpenRead(_serviceAccountPath))
            {
                serviceAccountCredential =
                    ServiceAccountCredential.FromServiceAccountData(stream);
            }

            var credential = GoogleCredential
                .FromServiceAccountCredential(serviceAccountCredential)
                .CreateScoped(FirebaseScope);

            return await credential.UnderlyingCredential
                .GetAccessTokenForRequestAsync();
        }
    }
}
