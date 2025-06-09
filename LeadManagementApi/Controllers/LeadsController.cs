using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LeadManagementApi.Features.Leads.Commands;
using LeadManagementApi.Features.Leads.Queries;
using LeadManagementApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeadsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeadsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtém todos os Leads.
        /// </summary>
        /// <returns>Uma lista de LeadResponse.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LeadResponse>>> GetLeads()
        {
            var leads = await _mediator.Send(new GetAllLeadsQuery());
            return Ok(leads);
        }

        /// <summary>
        /// Obtém um Lead por ID.
        /// </summary>
        /// <param name="id">O ID do Lead.</param>
        /// <returns>Um LeadResponse ou NotFound.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LeadResponse>> GetLead(Guid id)
        {
            var lead = await _mediator.Send(new GetLeadByIdQuery(id));
            if (lead == null)
            {
                return NotFound();
            }
            return Ok(lead);
        }

        /// <summary>
        /// Cria um novo Lead.
        /// </summary>
        /// <param name="request">Dados para criação do Lead.</param>
        /// <returns>O Lead recém-criado.</returns>
        [HttpPost] // Mapeia para requisições POST em /api/leads
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LeadResponse>> CreateLead([FromBody] CreateLeadRequest request)
        {
            // Cria um comando a partir do DTO de requisição
            var command = new CreateLeadCommand(
                request.ContactFirstName,
                request.Suburb,
                request.Category,
                request.Description,
                request.Price
            );
            var leadResponse = await _mediator.Send(command);
            // Retorna 201 Created com a localização do novo recurso
            return CreatedAtAction(nameof(GetLead), new { id = leadResponse.Id }, leadResponse);
        }

        /// <summary>
        /// Atualiza um Lead existente.
        /// </summary>
        /// <param name="id">O ID do Lead a ser atualizado.</param>
        /// <param name="request">Novos dados para o Lead.</param>
        /// <returns>O Lead atualizado ou NotFound.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LeadResponse>> UpdateLead(Guid id, [FromBody] UpdateLeadRequest request)
        {
            var command = new UpdateLeadCommand(
                id,
                request.ContactFirstName,
                request.Suburb,
                request.Category,
                request.Description,
                request.Price
            );
            var leadResponse = await _mediator.Send(command);
            if (leadResponse == null)
            {
                return NotFound();
            }
            return Ok(leadResponse);
        }

        /// <summary>
        /// Deleta um Lead.
        /// </summary>
        /// <param name="id">O ID do Lead a ser deletado.</param>
        /// <returns>NoContent ou NotFound.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLead(Guid id)
        {
            var success = await _mediator.Send(new DeleteLeadCommand(id));
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
