namespace BaccoApp.UserManagement.Application
{
    public class PaginationRequest
    {
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 20;
        public string Search { get; set; } = string.Empty;
    }
}