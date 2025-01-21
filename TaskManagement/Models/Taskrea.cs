using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagement.Models
{
    public class Taskrea
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";  // "Pending", "InProgress", "Completed"
        public int? AssignedUserId { get; set; } = 1;
        public User? AssignedUser { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

     public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed
    }
}