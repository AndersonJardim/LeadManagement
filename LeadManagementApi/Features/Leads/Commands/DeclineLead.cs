using LeadManagementApi.Data;
using LeadManagementApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LeadManagementApi.Features.Leads.Commands
{
    public record DeclineLeadCommand(Guid Id) : IRequest<LeadResponse?>;

    public class DeclineLeadCommandHandler : IRequestHandler<DeclineLeadCommand, LeadResponse?>
    {
        private readonly ApplicationDbContext _context;

        public DeclineLeadCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LeadResponse?> Handle(DeclineLeadCommand request, CancellationToken cancellationToken)
        {
            var lead = await _context.Leads.FindAsync(new object[] { request.Id }, cancellationToken);

            if (lead == null)
            {
                return null;
            }

            if (lead.Status == "Declined")
            {
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

            lead.Status = "Declined";

            _context.Entry(lead).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);

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