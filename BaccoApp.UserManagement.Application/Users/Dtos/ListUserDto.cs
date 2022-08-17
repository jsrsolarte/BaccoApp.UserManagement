namespace BaccoApp.UserManagement.Application.Users.Dtos
{
    public class ListUserDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
    }
}