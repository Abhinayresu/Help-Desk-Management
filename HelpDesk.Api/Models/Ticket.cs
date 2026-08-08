using System;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Api.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Priority { get; set; } = "Low"; // Low, Medium, High

        [Required]
        public string Status { get; set; } = "Open"; // Open, In Progress, Closed

        [Required]
        [MaxLength(100)]
        public string RaisedBy { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
