namespace ProjectApi.Services.Abstractions
{
    public interface IPushNotificationService
    {
        public Task<bool> SendPushAsync(string deviceToken, string title, string body);
    }
}
