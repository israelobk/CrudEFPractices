using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCCRUD.Data;
using MVCCRUD.Models;
using MVCCRUD.Models.Domain;

namespace MVCCRUD.Controllers
{
	public class EmployeesController : Controller
	{
		private readonly DemoDbContext _demoDbContext;
		public EmployeesController(DemoDbContext _demoDbContext) 
		{ 
			this._demoDbContext = _demoDbContext;
		}
		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
		{
			var employee = new Employee()
			{
				Id = Guid.NewGuid(),
				Name = addEmployeeRequest.Name,
				Email = addEmployeeRequest.Email,
				Salary = addEmployeeRequest.Salary,
				DateOfBirth = addEmployeeRequest.DateOfBirth,
				Department = addEmployeeRequest.Department,
			};

			await _demoDbContext.Employees.AddAsync(employee);
			await _demoDbContext.SaveChangesAsync();
			return RedirectToAction(nameof(ListOfEmployees));
		}
 

		[HttpGet]
		public async Task<IActionResult> ListOfEmployees()
		{
			var employees = await _demoDbContext.Employees.ToListAsync();
			return View(employees);
		}

		[HttpGet]
		public async Task<IActionResult> Update([FromRoute]Guid id) 
		{
			var employee = await _demoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
			if (employee != null) 
			{
				var viewModel = new UpdateEmployeeView()
				{
					Id =employee.Id,
					Name = employee.Name,
					Email = employee.Email,
					Salary = employee.Salary,
					DateOfBirth = employee.DateOfBirth,
					Department = employee.Department,
				};

				return await Task.Run(() => View(viewModel));
			}
			return RedirectToAction(nameof(ListOfEmployees));

		}

		[HttpPost]
		public async Task<IActionResult> Update (UpdateEmployeeView model)
		{
			var employee = await _demoDbContext.Employees.FindAsync(model.Id);
			
			if (employee != null)
			{
				employee.Name = model.Name;
				employee.Email = model.Email;
				employee.Salary = model.Salary;
				employee.DateOfBirth = model.DateOfBirth;
				employee.Department = model.Department;

				await _demoDbContext.SaveChangesAsync();

				return RedirectToAction(nameof(ListOfEmployees));
			}
			return RedirectToAction(nameof(ListOfEmployees));

		}

		[HttpPost]
		public async Task<IActionResult> Delete(UpdateEmployeeView model)
		{
			var employee = await _demoDbContext.Employees.FindAsync(model.Id);
			if (employee != null)
			{
				_demoDbContext.Employees.Remove(employee);
				await _demoDbContext.SaveChangesAsync();

				return RedirectToAction(nameof(ListOfEmployees));
			}
			return RedirectToAction(nameof(ListOfEmployees));

		}
	}


}
