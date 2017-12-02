using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Northwind.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Presentation.Filters
{
    public class ValidateCustomerExistsAttribute : TypeFilterAttribute
    {
        public ValidateCustomerExistsAttribute() : base(typeof(ValidateCustomerExistsFilterImpl)) { }

        private class ValidateCustomerExistsFilterImpl : IAsyncActionFilter
        {
            private readonly NorthwindContext _context;

            public ValidateCustomerExistsFilterImpl(NorthwindContext context)
            {
                _context = context;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("id"))
                {
                    var id = context.ActionArguments["id"] as string;
                    if (id != null)
                    {
                        if (await _context.Customers.AllAsync(c => c.CustomerId != id))
                        {
                            context.Result = new NotFoundObjectResult(id);
                            
                            return;
                        }
                    }
                }
                await next();
            }
        }
    }
}