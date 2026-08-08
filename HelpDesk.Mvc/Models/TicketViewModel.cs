using System;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Mvc.Models
{
    public class TicketViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Priority must be selected")]
        public string Priority { get; set; } = "Low"; // Low, Medium, High

        public string Status { get; set; } = "Open"; // Open, In Progress, Closed

        [Required(ErrorMessage = "Your name/ID is required")]
        [Display(Name = "Raised By")]
        public string RaisedBy { get; set; } = string.Empty;

        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
