using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LeadManagementApi.Data;
using LeadManagementApi.Entities;
using LeadManagementApi.Models;
using MediatR;

namespace LeadManagementApi.Features.Leads.Commands
{
    public record CreateLeadCommand(string ContactFirstName, string Suburb, string Category, string Description, decimal Price) : IRequest<LeadResponse>;

    public class CreateLeadCommandHandler : IRequestHandler<CreateLeadCommand, LeadResponse>
    {
        private readonly ApplicationDbContext _context;

        public CreateLeadCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LeadResponse> Handle(CreateLeadCommand request, CancellationToken cancellationToken)
        {
            var lead = new Lead
            {
                Id = Guid.NewGuid(),
                ContactFirstName = request.ContactFirstName,
                DateCreated = DateTime.UtcNow,
                Suburb = request.Suburb,
                Category = request.Category,
                Description = request.Description,
                Price = request.Price
            };

            _context.Leads.Add(lead);
            await _context.SaveChangesAsync(cancellationToken);

            return new LeadResponse
            {
                Id = lead.Id,
                ContactFirstName = lead.ContactFirstName,
                DateCreated = lead.DateCreated,
                Suburb = lead.Suburb,
                Category = lead.Category,
                Description = lead.Description,
                Price = lead.Price
            };
        }
    }
}