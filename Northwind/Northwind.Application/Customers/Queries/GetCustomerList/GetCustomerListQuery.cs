using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Northwind.Persistence;

namespace Northwind.Application.Customers.Queries.GetCustomerList
{
    public class GetCustomerListQuery : IGetCustomerListQuery
    {
        public readonly NorthwindContext _context;

        public GetCustomerListQuery(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerListModel>> Execute()
        {
            return await _context.Customers.Select(c =>
                new CustomerListModel
                {
                    Id = c.CustomerId,
                    Name = c.CompanyName
                }).ToListAsync();
        }
    }
}