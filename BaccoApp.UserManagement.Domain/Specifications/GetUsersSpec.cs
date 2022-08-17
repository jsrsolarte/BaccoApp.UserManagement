using Ardalis.Specification;
using BaccoApp.UserManagement.Domain.Entities;

namespace BaccoApp.UserManagement.Domain.Specifications
{
    public class GetUsersSpec : Specification<User>
    {
        public GetUsersSpec(string search, int recordsPerPage, int page)
        {
            if (!string.IsNullOrEmpty(search))
            {
                Query.Where(_ => _.FirstName.Contains(search) ||
                            _.LastName.Contains(search) ||
                            _.Email.Equals(search));
            }
            Query.Skip((page - 1) * recordsPerPage)
              .Take(recordsPerPage);
        }

        public GetUsersSpec(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                Query.Where(_ => _.FirstName.Contains(search) ||
                            _.LastName.Contains(search) ||
                            _.Email.Equals(search));
            }
        }
    }
}