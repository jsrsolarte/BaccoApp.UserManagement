using Ardalis.Specification;
using BaccoApp.UserManagement.Domain.Entities;

namespace BaccoApp.UserManagement.Domain.Specifications
{
    public class FindUserByEmailSpec : Specification<User>
    {
        public FindUserByEmailSpec(string email)
        {
            Query.Where(_ => _.Email.Equals(email));
        }
    }
}