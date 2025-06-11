using LeadManagementApi.Data;
using LeadManagementApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LeadManagementApi.Features.Leads.Queries
{
    public record GetAllLeadsQuery : IRequest<IEnumerable<LeadResponse>>;

    public class GetAllLeadsQueryHandler : IRequestHandler<GetAllLeadsQuery, IEnumerable<LeadResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllLeadsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeadResponse>> Handle(GetAllLeadsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Leads.AsNoTracking()
                                   .Select(lead => new LeadResponse
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
                                   })
                                   .ToListAsync(cancellationToken);
        }
    }
}