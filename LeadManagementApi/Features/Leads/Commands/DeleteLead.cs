using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LeadManagementApi.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LeadManagementApi.Features.Leads.Commands
{
    public record DeleteLeadCommand(Guid Id) : IRequest<bool>;

    public class DeleteLeadCommandHandler : IRequestHandler<DeleteLeadCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public DeleteLeadCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteLeadCommand request, CancellationToken cancellationToken)
        {
            var lead = await _context.Leads.FindAsync(new object[] { request.Id }, cancellationToken);

            if (lead == null)
            {
                return false;
            }

            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync(cancellationToken); // Salva as mudanças no DB

            return true;
        }
    }
}