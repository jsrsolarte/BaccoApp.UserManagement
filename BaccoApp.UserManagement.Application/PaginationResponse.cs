namespace BaccoApp.UserManagement.Application
{
    public class PaginationResponse<T>
    {
        public int Page { get; set; }
        public int TotalRecords { get; set; }
        public int RecordsPerPage { get; set; }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((double)TotalRecords / RecordsPerPage);
            }
        }

        public IEnumerable<T> Records { get; set; } = Enumerable.Empty<T>();

        public PaginationResponse(PaginationRequest request, IEnumerable<T> records, int totalRecords)
        {
            Page = request.Page;
            TotalRecords = totalRecords;
            RecordsPerPage = request.RecordsPerPage;
            Records = records;
        }
    }
}