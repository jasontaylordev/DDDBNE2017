using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Application.Customers.Queries.GetCustomerList
{
    public interface IGetCustomerListQuery
    {
        Task<IEnumerable<CustomerListModel>> Execute();
    }
}