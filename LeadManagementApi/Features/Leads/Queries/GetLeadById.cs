using LeadManagementApi.Data;
using LeadManagementApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LeadManagementApi.Features.Leads.Queries
{
    public record GetLeadByIdQuery(Guid Id) : IRequest<LeadResponse?>;

    public class GetLeadByIdQueryHandler : IRequestHandler<GetLeadByIdQuery, LeadResponse?>
    {
        private readonly ApplicationDbContext _context;

        public GetLeadByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LeadResponse?> Handle(GetLeadByIdQuery request, CancellationToken cancellationToken)
        {
            var lead = await _context.Leads.AsNoTracking()
                                     .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

            if (lead == null)
            {
                return null;
            }

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