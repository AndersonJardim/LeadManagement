using LeadManagementApi.Data;
using LeadManagementApi.Models;
using LeadManagementApi.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LeadManagementApi.Features.Leads.Commands
{
    public record AcceptLeadCommand(Guid Id) : IRequest<LeadResponse?>;

    public class AcceptLeadCommandHandler : IRequestHandler<AcceptLeadCommand, LeadResponse?>
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AcceptLeadCommandHandler(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<LeadResponse?> Handle(AcceptLeadCommand request, CancellationToken cancellationToken)
        {
            var lead = await _context.Leads.FindAsync(new object[] { request.Id }, cancellationToken);

            if (lead == null)
            {
                return null;
            }

            if (lead.Status == "Accepted")
            {
                // Já aceito, não faz nada ou retorna o lead atual
                return new LeadResponse
                {
                    Id = lead.Id,
                    ContactFirstName = lead.ContactFirstName,
                    DateCreated = lead.DateCreated,
                    Suburb = lead.Suburb,
                    Category = lead.Category,
                    Description = lead.Description,
                    Price = lead.Price,
                    Status = lead.Status,
                    ContactFullName = lead.ContactFullName,
                    ContactPhoneNumber = lead.ContactPhoneNumber,
                    ContactEmail = lead.ContactEmail
                };
            }

            // Aplicar 10% de desconto se o preço for superior a US$ 500
            if (lead.Price > 500m)
            {
                lead.Price *= 0.9m; // Aplica 10% de desconto
            }

            lead.Status = "Accepted"; // Atualiza o status

            // Preenche dados de contato (simulado, em um cenário real viriam de outro lugar)
            lead.ContactFullName = lead.ContactFirstName + " Example";
            lead.ContactPhoneNumber = "0412345678"; // Número de telefone simulado
            lead.ContactEmail = "sales@test.com"; // Email de contato simulado para este lead

            _context.Entry(lead).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);

            // Simula o envio de e-mail para vendas
            await _emailService.SendEmailAsync("sales@test.com",
                                                $"Lead Accepted: {lead.ContactFirstName} ({lead.Category})",
                                                $"Lead {lead.ContactFirstName} from {lead.Suburb} has been accepted. New price: ${lead.Price:F2}. Contact: {lead.ContactPhoneNumber}, {lead.ContactEmail}");

            return new LeadResponse
            {
                Id = lead.Id,
                ContactFirstName = lead.ContactFirstName,
                DateCreated = lead.DateCreated,
                Suburb = lead.Suburb,
                Category = lead.Category,
                Description = lead.Description,
                Price = lead.Price,
                Status = lead.Status,
                ContactFullName = lead.ContactFullName,
                ContactPhoneNumber = lead.ContactPhoneNumber,
                ContactEmail = lead.ContactEmail
            };
        }
    }
}