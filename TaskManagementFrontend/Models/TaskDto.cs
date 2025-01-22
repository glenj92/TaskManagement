namespace TaskManagementFrontend.Models
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public int? AssignedUserId { get; set; } 
        public UserDto AssignedUser { get; set; }
    }



    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // Rol del usuario (User, Admin, etc.)
    }

}
