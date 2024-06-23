using FullStackAPI.Data;
using FullStackAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;

namespace FullStackAPI.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	public class EmployeesController : Controller
	{
		private FullStackDbContext _fullStackDbContext;
        public EmployeesController(FullStackDbContext fullStackDbContext)
        {
			this._fullStackDbContext = fullStackDbContext;

		}

		[HttpGet]
		[Route("country")]
		public async Task<IActionResult> GetCountry()
		{
			
			var countries = await _fullStackDbContext.Employees
									.Select(e => e.Country)
									.Distinct()
									.ToListAsync();
			if (countries == null)
			{
				return NotFound();
			}

			return Ok(countries);
		}

		[HttpPost]
		[Route("state")]
		public async Task<IActionResult> GetState([FromBody] string country)
		{
			
			var states = await _fullStackDbContext.Employees
							   .Where(e => e.Country == country)
							   .Select(e => e.State)
							   .Distinct()
							   .ToListAsync();
			if (states == null)
			{
				return NotFound();
			}
			return Ok(states);
		}




		[HttpGet]
		public async Task<IActionResult> GetAllEmployees()
		{
			var employee = await _fullStackDbContext.Employees.ToListAsync();
			return Ok(employee);
		}



		[HttpPost]
		public async Task<IActionResult> AddEmployee([FromBody] Employee employeeRequest)
		{
			employeeRequest.Id = Guid.NewGuid().ToString();
			await _fullStackDbContext.Employees.AddAsync(employeeRequest);
			await _fullStackDbContext.SaveChangesAsync();
			return Ok(employeeRequest);
		}


		[HttpGet]
		[Route("{id}")]
		public async Task<IActionResult> GetEmployee([FromRoute] string id)
		{
			var employee = await _fullStackDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
			if(employee==null)
			{
				return NotFound();
			}
			return Ok(employee);
		}

		[HttpPut]
		[Route("{id}")]
		public async Task<IActionResult> updateEmployee([FromRoute] string id,Employee updateEmployeeRequest)
		{
			var employee = await _fullStackDbContext.Employees.FindAsync(id);
			if(employee==null)
			{
				return NotFound();
			}
			employee.Name = updateEmployeeRequest.Name;
			employee.Email = updateEmployeeRequest.Email;
			employee.Phone = updateEmployeeRequest.Phone;
			employee.Department = updateEmployeeRequest.Department;

			employee.Country = updateEmployeeRequest.Country;
			employee.State = updateEmployeeRequest.State;

			await _fullStackDbContext.SaveChangesAsync();
			return Ok(employee);
		}


		[HttpDelete]
		[Route("{id}")]
		public async Task<IActionResult> DeleteEmployee([FromRoute] string id)
		{
			var employee = await _fullStackDbContext.Employees.FindAsync(id);
			if (employee == null)
			{
				return NotFound();
			}
			_fullStackDbContext.Employees.Remove(employee);
			await _fullStackDbContext.SaveChangesAsync();
			return Ok(employee);
		}

	}
}
