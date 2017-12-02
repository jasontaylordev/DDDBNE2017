using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Domain;
using Northwind.Persistence;

namespace Northwind.Presentation.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TypicalCustomersController : Controller
    {
        private readonly NorthwindContext _context;

        public TypicalCustomersController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            return Ok(await _context.Customers.ToListAsync());
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] string id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<IActionResult> PostCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Customers.Add(customer);
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer([FromRoute] string id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            var entity = await _context.Customers.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.CompanyName = customer.CompanyName;
            entity.ContactTitle = customer.ContactTitle;
            entity.ContactName = customer.ContactName;
            entity.Address = customer.Address;
            entity.City = customer.City;
            entity.Country = customer.Country;
            entity.PostalCode = customer.PostalCode;
            entity.Region = customer.Region;
            entity.Phone = customer.Phone;
            entity.Fax = customer.Fax;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();

            return Ok(customer);
        }
    }
}