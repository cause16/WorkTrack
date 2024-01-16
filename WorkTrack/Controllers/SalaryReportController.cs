using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using WorkTrack.Services.Interfaces;

namespace WorkTrack.Controllers
{
	public class SalaryReportController : Controller
	{
		private readonly ISalaryReportService _salaryReportService;

		public SalaryReportController(ISalaryReportService salaryReportService)
		{
			_salaryReportService = salaryReportService;
		}

		[HttpGet]
		public IActionResult Index()
		{
			GetSelectList("Відділ");

			var salaryReport = _salaryReportService.GetSalaryReportForDepartments();

			return View(salaryReport);
		}

		[HttpPost]
		public IActionResult GenerateReport(string filterOption)
		{
			GetSelectList(filterOption);

			var salaryReport = filterOption == "Відділ" ? _salaryReportService.GetSalaryReportForDepartments()
				: _salaryReportService.GetSalaryReportForPositions();

			return View("Index", salaryReport);
		}

		[HttpPost]
		public IActionResult ExportToTxt(string filterOption)
		{
			var salaryReport = filterOption == "Відділ" ? _salaryReportService.GetSalaryReportForDepartments()
				: _salaryReportService.GetSalaryReportForPositions();

			var stringBuilder = new StringBuilder();

			foreach (var reportItem in salaryReport)
			{
				stringBuilder.AppendLine($"Назва: {reportItem.Name}");
				stringBuilder.AppendLine($"Кількість працівників: {reportItem.EmployeeQty}");
				stringBuilder.AppendLine($"Мінімальний оклад: {reportItem.MinSalary.ToString("0.00")}");
				stringBuilder.AppendLine($"Максимальний оклад: {reportItem.MaxSalary.ToString("0.00")}");
				stringBuilder.AppendLine($"Середній оклад: {reportItem.AvgSalary.ToString("0.00")}");
				stringBuilder.AppendLine($"Загальна сума окладів: {reportItem.TotalSalarySum.ToString("0.00")}");
				stringBuilder.AppendLine();
			}

			System.IO.File.WriteAllText("Salary report.txt", stringBuilder.ToString());

			return File(Encoding.UTF8.GetBytes(stringBuilder.ToString()), "text/plain", "Salary report.txt");
		}

		private void GetSelectList(string selectedOption)
		{
			var options = new List<string> { "Відділ", "Посада" };

			var salaryReportFilter = new SelectList(options, selectedOption);

			ViewBag.FilterOptions = salaryReportFilter;
		}
	}
}
