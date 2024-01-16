using Dapper;
using Microsoft.Data.SqlClient;
using WorkTrack.Services.Interfaces;
using WorkTrack.ViewModels;

namespace WorkTrack.Services
{
	public class SalaryReportService : ISalaryReportService
	{
		private readonly string? _connectionString;

		public SalaryReportService(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public List<SalaryReportViewModel> GetSalaryReportForDepartments()
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				var query = $@"SELECT 
                            d.DepartmentName AS Name,
                            COUNT(e.EmployeeId) AS EmployeeQty,
                            MIN(e.Salary) AS MinSalary,
                            MAX(e.Salary) AS MaxSalary,
                            AVG(e.Salary) AS AvgSalary,
                            SUM(e.Salary) AS TotalSalarySum
                        FROM Employees e
						JOIN Departments d ON e.DepartmentId = d.DepartmentId
                        GROUP BY d.DepartmentName";

				var salaryReports = connection.Query<SalaryReportViewModel>(query);

				return salaryReports.ToList();
			}
		}

		public List<SalaryReportViewModel> GetSalaryReportForPositions()
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				var query = $@"SELECT 
                            p.PositionName AS Name,
                            COUNT(e.EmployeeId) AS EmployeeQty,
                            MIN(e.Salary) AS MinSalary,
                            MAX(e.Salary) AS MaxSalary,
                            AVG(e.Salary) AS AvgSalary,
                            SUM(e.Salary) AS TotalSalarySum
                        FROM Employees e
						JOIN Positions p ON e.PositionId = p.PositionId
                        GROUP BY p.PositionName";

				var salaryReports = connection.Query<SalaryReportViewModel>(query);

				return salaryReports.ToList();
			}
		}
	}
}
