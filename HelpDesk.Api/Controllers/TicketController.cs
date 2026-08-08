using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;

namespace HelpDesk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _repository;

        public TicketController(ITicketRepository repository)
        {
            _repository = repository;
        }

        // GET /api/Ticket/All
        [HttpGet("All")]
        public async Task<ActionResult<List<Ticket>>> GetAllTickets()
        {
            var tickets = await _repository.GetAllTicketsAsync();
            return Ok(tickets);
        }

        // GET /api/Ticket/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Ticket>> GetTicketById(int id)
        {
            var ticket = await _repository.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound(new { message = $"Ticket with ID {id} not found." });
            }
            return Ok(ticket);
        }

        // POST /api/Ticket
        [HttpPost]
        public async Task<ActionResult<Ticket>> CreateTicket([FromBody] Ticket? ticket)
        {
            if (ticket == null)
            {
                return BadRequest(new { message = "Ticket data cannot be null." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure valid priority
            var priority = ticket.Priority;
            if (priority != "Low" && priority != "Medium" && priority != "High")
            {
                return BadRequest(new { message = "Invalid priority level. Valid levels are Low, Medium, High." });
            }

            // Status is forced to Open upon creation as per business rules
            ticket.Status = "Open";
            ticket.CreatedDate = DateTime.UtcNow;

            await _repository.CreateTicketAsync(ticket);

            return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, ticket);
        }

        // PUT /api/Ticket/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTicket(int id, [FromBody] Ticket? ticket)
        {
            if (ticket == null)
            {
                return BadRequest(new { message = "Ticket data cannot be null." });
            }

            if (id != ticket.Id)
            {
                return BadRequest(new { message = "Mismatched Ticket ID." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await _repository.GetTicketByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new { message = $"Ticket with ID {id} not found." });
            }

            // Validate status
            var status = ticket.Status;
            if (status != "Open" && status != "In Progress" && status != "Closed")
            {
                return BadRequest(new { message = "Invalid status. Valid statuses are Open, In Progress, Closed." });
            }

            // Validate priority
            var priority = ticket.Priority;
            if (priority != "Low" && priority != "Medium" && priority != "High")
            {
                return BadRequest(new { message = "Invalid priority level. Valid levels are Low, Medium, High." });
            }

            await _repository.UpdateTicketAsync(ticket);
            return Ok(ticket);
        }

        // DELETE /api/Ticket/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var existing = await _repository.GetTicketByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new { message = $"Ticket with ID {id} not found." });
            }

            await _repository.DeleteTicketAsync(id);
            return Ok(new { message = $"Ticket with ID {id} deleted successfully." });
        }

        // GET /api/Ticket/Status/{status}
        [HttpGet("Status/{status}")]
        public async Task<ActionResult<List<Ticket>>> GetTicketsByStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return BadRequest(new { message = "Status parameter cannot be empty." });
            }

            if (status != "Open" && status != "In Progress" && status != "Closed")
            {
                return BadRequest(new { message = "Invalid status parameter. Valid statuses are Open, In Progress, Closed." });
            }

            var tickets = await _repository.GetTicketsByStatusAsync(status);
            return Ok(tickets);
        }
    }
}
