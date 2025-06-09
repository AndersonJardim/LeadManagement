using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LeadManagementApi.Data;
using LeadManagementApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LeadManagementApi.Features.Leads.Commands
{
    public record UpdateLeadCommand(Guid Id, string ContactFirstName, string Suburb, string Category, string Description, decimal Price) : IRequest<LeadResponse>;

    public class UpdateLeadCommandHandler : IRequestHandler<UpdateLeadCommand, LeadResponse>
    {
        private readonly ApplicationDbContext _context;

        public UpdateLeadCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LeadResponse> Handle(UpdateLeadCommand request, CancellationToken cancellationToken)
        {
            var lead = await _context.Leads.FindAsync(new object[] { request.Id }, cancellationToken);

            if (lead == null)
            {
                return null;
            }

            lead.ContactFirstName = request.ContactFirstName;
            lead.Suburb = request.Suburb;
            lead.Category = request.Category;
            lead.Description = request.Description;
            lead.Price = request.Price;

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
                Price = lead.Price
            };
        }
    }
}