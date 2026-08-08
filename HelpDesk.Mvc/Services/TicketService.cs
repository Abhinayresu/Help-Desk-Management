using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using HelpDesk.Mvc.Models;

namespace HelpDesk.Mvc.Services
{
    public class TicketService : ITicketService
    {
        private readonly HttpClient _httpClient;

        public TicketService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            var baseUrl = configuration["HelpDeskApiUrl"];
            if (!string.IsNullOrEmpty(baseUrl))
            {
                _httpClient.BaseAddress = new Uri(baseUrl);
            }
        }

        public async Task<List<TicketViewModel>> GetAllTicketsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<TicketViewModel>>("api/Ticket/All");
                return response ?? new List<TicketViewModel>();
            }
            catch
            {
                return new List<TicketViewModel>();
            }
        }

        public async Task<TicketViewModel?> GetTicketByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Ticket/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TicketViewModel>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateTicketAsync(TicketViewModel ticket)
        {
            try
            {
                ticket.Status = "Open"; // Forced to Open on creation
                ticket.CreatedDate = DateTime.UtcNow;

                var response = await _httpClient.PostAsJsonAsync("api/Ticket", ticket);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateTicketAsync(int id, TicketViewModel ticket)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Ticket/{id}", ticket);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTicketAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Ticket/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<TicketViewModel>> GetTicketsByStatusAsync(string status)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<TicketViewModel>>($"api/Ticket/Status/{status}");
                return response ?? new List<TicketViewModel>();
            }
            catch
            {
                return new List<TicketViewModel>();
            }
        }

        public async Task<DashboardViewModel> GetDashboardStatsAsync()
        {
            var allTickets = await GetAllTicketsAsync();
            return new DashboardViewModel
            {
                TotalTickets = allTickets.Count,
                OpenTickets = allTickets.Count(t => t.Status.Equals("Open", StringComparison.OrdinalIgnoreCase)),
                InProgressTickets = allTickets.Count(t => t.Status.Equals("In Progress", StringComparison.OrdinalIgnoreCase)),
                ClosedTickets = allTickets.Count(t => t.Status.Equals("Closed", StringComparison.OrdinalIgnoreCase))
            };
        }
    }
}
