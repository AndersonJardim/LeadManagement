using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Importa para Logging

namespace LeadManagementApi.Services
{
    public class FakeEmailService : IEmailService
    {
        private readonly ILogger<FakeEmailService> _logger;
        private readonly string _logFilePath;

        public FakeEmailService(ILogger<FakeEmailService> logger)
        {
            _logger = logger;
            // Define um caminho para o arquivo de log do e-mail.
            // Para Docker, considere um volume ou apenas log para console.
            _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "email_log.txt");
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var emailContent = $"Simulated Email to: {toEmail}\nSubject: {subject}\nMessage: {message}\n---End of Email---\n\n";

            // Loga no console
            _logger.LogInformation($"[FakeEmailService] {emailContent}");

            // Opcional: Escreve em um arquivo de texto para simular o envio
            try
            {
                await File.AppendAllTextAsync(_logFilePath, emailContent);
                _logger.LogInformation($"[FakeEmailService] Email logged to {_logFilePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[FakeEmailService] Error writing email to log file: {ex.Message}");
            }

            await Task.CompletedTask; // Retorna um Task completado para assincronicidade
        }
    }
}
