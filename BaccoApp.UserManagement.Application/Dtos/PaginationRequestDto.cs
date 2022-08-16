namespace BaccoApp.UserManagement.Application.Dtos
{
    public class PaginationRequestDto
    {
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 20;
        public string Search { get; set; } = string.Empty;
    }
}