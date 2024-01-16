using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkTrack.Models;
using WorkTrack.Services.Interfaces;
using WorkTrack.ViewModels;

namespace WorkTrack.Controllers
{
	public class PersonnelController : Controller
	{
		private readonly IAppCrudService _appCrudService;
		private readonly IEmployeeFilterService _employeeFilterService;
		private readonly IMapper _mapper;

		public PersonnelController(IAppCrudService appCrudService, IEmployeeFilterService employeeFilterService, IMapper mapper)
		{
			_appCrudService = appCrudService;
			_employeeFilterService = employeeFilterService;
			_mapper = mapper;
		}

		[HttpGet]
		public IActionResult Index(int position, int department, string fullName)
		{
			SetSelectedParameters();

			List<Employee> employees;

			if (position == 0 && department == 0 && fullName == null)
				employees = _appCrudService.GetAllEmployees();
			else
				employees = _employeeFilterService.GetFilteredEmployees(position, department, fullName);

			var model = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);

			GetSelectLists(Convert.ToInt32(HttpContext.Request.Query["position"]),
						   Convert.ToInt32(HttpContext.Request.Query["department"]), true);

			return View(model);
		}

		[HttpPost]
		public IActionResult FilterEmployees(int position, int department, string fullName)
		{
			if (position != 0)
				ViewBag.FilterPositionParam = position;
			if (department != 0)
				ViewBag.FilterDepartmentParam = department;
			if (!String.IsNullOrEmpty(fullName))
				ViewBag.FilterFullNameParam = fullName;

			var filteredEmployees = _employeeFilterService.GetFilteredEmployees(position, department, fullName);

			var model = _mapper.Map<IEnumerable<EmployeeViewModel>>(filteredEmployees);
			
			GetSelectLists(position, department, true);

			return View("index", model);
		}

		[HttpGet]
		public IActionResult EditEmployee(int id)
		{
			SetSelectedParameters();

			var employee = _appCrudService.GetEmployeeById(id);


			if (employee == null)
				return NotFound();

			var model = _mapper.Map<EmployeeViewModel>(employee);

			model.SelectedPositionId = employee.PositionId;
			model.SelectedDepartmentId = employee.DepartmentId;

			GetSelectLists(model.SelectedPositionId, model.SelectedDepartmentId, false);

			return View(model);
		}

		[HttpPost]
		public IActionResult EditEmployee(EmployeeViewModel model)
		{
			SetSelectedParameters();

			if (ModelState.IsValid)
			{
				var updatedEmployee = _mapper.Map<Employee>(model);

				updatedEmployee.PositionId = model.SelectedPositionId;
				updatedEmployee.DepartmentId = model.SelectedDepartmentId;

				_appCrudService.EditEmployeeInfo(updatedEmployee);

				return RedirectToAction("index", new
				{
					position = HttpContext.Request.Query["position"],
					department = HttpContext.Request.Query["department"],
					fullName = HttpContext.Request.Query["fullName"]
				});
			}

			GetSelectLists(model.SelectedPositionId, model.SelectedDepartmentId, false);

			return View(model);
		}

		private void SetSelectedParameters()
		{
			ViewBag.FilterPositionParam = HttpContext.Request.Query["position"];
			ViewBag.FilterDepartmentParam = HttpContext.Request.Query["department"];
			ViewBag.FilterFullNameParam = HttpContext.Request.Query["fullName"];
		}

		private void GetSelectLists(int selectedPositionId, int selectedDepartmentId, bool isForFiltering)
		{
			var positions = _appCrudService.GetPositions();
			var departments = _appCrudService.GetDepartments();

			if (isForFiltering)
			{
				positions.Insert(0, new Position() { PositionName = "Нічого", PositionId = 0 });
				departments.Insert(0, new Department() { DepartmentName = "Нічого", DepartmentId = 0 });
			}

			var positionsList = new SelectList(positions, nameof(Position.PositionId),
				nameof(Position.PositionName), selectedPositionId);
			var departmentsList = new SelectList(departments, nameof(Department.DepartmentId),
				nameof(Department.DepartmentName), selectedDepartmentId);

			ViewBag.Positions = positionsList;
			ViewBag.Departments = departmentsList;
		}
	}
}
