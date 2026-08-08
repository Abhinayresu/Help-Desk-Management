using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Models;

namespace HelpDesk.Api.Data
{
    public class HelpDeskDbContext : DbContext
    {
        public HelpDeskDbContext(DbContextOptions<HelpDeskDbContext> options) : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial data
            modelBuilder.Entity<Ticket>().HasData(
                new Ticket
                {
                    Id = 1,
                    Title = "Cannot access corporate email",
                    Description = "Getting error 503 when logging into Outlook on web.",
                    Priority = "High",
                    Status = "Open",
                    RaisedBy = "John Doe",
                    CreatedDate = new DateTime(2026, 8, 1, 10, 0, 0, DateTimeKind.Utc)
                },
                new Ticket
                {
                    Id = 2,
                    Title = "VPN connection drops",
                    Description = "The VPN connection drops every 15 minutes on Windows 11.",
                    Priority = "Medium",
                    Status = "In Progress",
                    RaisedBy = "Jane Smith",
                    CreatedDate = new DateTime(2026, 8, 2, 11, 30, 0, DateTimeKind.Utc)
                },
                new Ticket
                {
                    Id = 3,
                    Title = "New monitor request",
                    Description = "Requesting a secondary 27-inch monitor for development workspace.",
                    Priority = "Low",
                    Status = "Closed",
                    RaisedBy = "Alice Johnson",
                    CreatedDate = new DateTime(2026, 8, 3, 14, 15, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
