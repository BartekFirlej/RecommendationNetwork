using Microsoft.AspNetCore.Mvc;
using ProductStore.DTOs;
using ProductStore.Services;

namespace ProductStore.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers() {
            ICollection<CustomerResponse> customers;
            try
            {
                customers = await _customerService.GetCustomers();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            CustomerResponse customer;
            try
            {
                customer = await _customerService.GetCustomerResponse(id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> PostCustomer(CustomerRequest customerToAdd)
        {
            CustomerResponse customer;
            try
            {
                customer = await _customerService.PostCustomer(customerToAdd);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            CustomerResponse customer;
            try
            {
                customer = await _customerService.DeleteCustomer(id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return Ok(customer);
        }
    }
}
