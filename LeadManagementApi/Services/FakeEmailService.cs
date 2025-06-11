using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LeadManagementApi.Services
{
    public class FakeEmailService : IEmailService
    {
        private readonly ILogger<FakeEmailService> _logger;
        private readonly string _logFilePath;

        public FakeEmailService(ILogger<FakeEmailService> logger)
        {
            _logger = logger;
            _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "email_log.txt");
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var emailContent = $"Simulated Email to: {toEmail}\nSubject: {subject}\nMessage: {message}\n---End of Email---\n\n";

            _logger.LogInformation($"[FakeEmailService] {emailContent}");

            try
            {
                await File.AppendAllTextAsync(_logFilePath, emailContent);
                _logger.LogInformation($"[FakeEmailService] Email logged to {_logFilePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[FakeEmailService] Error writing email to log file: {ex.Message}");
            }

            await Task.CompletedTask;
        }
    }
}
