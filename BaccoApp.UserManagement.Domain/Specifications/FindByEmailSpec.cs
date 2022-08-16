using Ardalis.Specification;
using BaccoApp.UserManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaccoApp.UserManagement.Domain.Specifications
{
    public class FindByEmailSpec: Specification<User>
    {
        public FindByEmailSpec(string email)
        {
            Query.Where(_ => _.Email.Equals(email));
        }
    }
}
