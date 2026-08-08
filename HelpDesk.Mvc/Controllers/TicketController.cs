using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Mvc.Models;
using HelpDesk.Mvc.Services;

namespace HelpDesk.Mvc.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: /Ticket/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var stats = await _ticketService.GetDashboardStatsAsync();
            return View(stats);
        }

        // GET: /Ticket/Index or /Ticket
        public async Task<IActionResult> Index(string? status)
        {
            ViewBag.CurrentFilter = status;
            if (string.IsNullOrEmpty(status))
            {
                var tickets = await _ticketService.GetAllTicketsAsync();
                return View(tickets);
            }
            else
            {
                var tickets = await _ticketService.GetTicketsByStatusAsync(status);
                return View(tickets);
            }
        }

        // GET: /Ticket/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // GET: /Ticket/Create
        public IActionResult Create()
        {
            return View(new TicketViewModel());
        }

        // POST: /Ticket/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketViewModel model)
        {
            // Status is hardcoded to "Open" during creation
            model.Status = "Open";

            if (ModelState.IsValid)
            {
                var success = await _ticketService.CreateTicketAsync(model);
                if (success)
                {
                    TempData["SuccessMessage"] = "Ticket raised successfully.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Unable to create ticket at this time.");
            }
            return View(model);
        }

        // GET: /Ticket/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: /Ticket/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var success = await _ticketService.UpdateTicketAsync(id, model);
                if (success)
                {
                    TempData["SuccessMessage"] = "Ticket updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Unable to update ticket at this time.");
            }
            return View(model);
        }

        // GET: /Ticket/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: /Ticket/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _ticketService.DeleteTicketAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Ticket deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Unable to delete ticket.";
            return RedirectToAction(nameof(Index));
        }
    }
}
