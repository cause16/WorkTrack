using Dapper;
using Microsoft.Data.SqlClient;
using System.Text;
using WorkTrack.Models;
using WorkTrack.Services.Interfaces;

namespace WorkTrack.Services
{
	public class EmployeeFilterService : IEmployeeFilterService
	{
		private readonly string? _connectionString;
		private readonly IAppCrudService _appCrudService;

		public EmployeeFilterService(IConfiguration configuration, IAppCrudService appCrudService)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
			_appCrudService = appCrudService;
		}

		public List<Employee> GetFilteredEmployees(int positionId, int departmentId, string fullName)
		{
			if (positionId == 0 && departmentId == 0 && String.IsNullOrEmpty(fullName))
				return _appCrudService.GetAllEmployees();

			var query = new StringBuilder(@"SELECT e.*, p.*, a.*, d.*
							  FROM Employees AS e
							  JOIN Positions AS p
							  ON e.PositionId = p.PositionId
							  JOIN Addresses AS a
							  ON e.AddressId = a.AddressId
							  JOIN Departments AS d
							  ON e.DepartmentId = d.DepartmentId 
							  WHERE ");

			if (positionId != 0)
			{
				query.Append($"p.PositionId = {positionId} ");
			}

			if (departmentId != 0)
			{
				if (positionId != 0)
				{
					query.Append("AND ");
				}
				query.Append($"d.DepartmentId = {departmentId} ");
			}

			if (!string.IsNullOrEmpty(fullName))
			{
				var parsedFullName = ParseFullNameToQueryString(fullName);

				if (positionId != 0 || departmentId != 0)
				{
					query.Append("AND ");
				}
				query.Append(parsedFullName);
			}

			using (var connection = new SqlConnection(_connectionString))
			{
				var filteredEmployees = connection.Query<Employee, Position, Address, Department, Employee>(query.ToString(),
					(employee, position, address, department) =>
					{
						employee.Position = position;
						employee.Address = address;
						employee.Department = department;
						return employee;
					},
					splitOn: "PositionId, AddressId, DepartmentId").ToList();

				return filteredEmployees;
			}
		}

		private string ParseFullNameToQueryString(string fullName)
		{
			var nameParts = fullName?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];

			var conditions = new List<string>();

			if (nameParts.Length > 0 && !string.IsNullOrEmpty(nameParts[0]))
			{
				conditions.Add($"e.LastName = '{nameParts[0]}'");
			}

			if (nameParts.Length > 1 && !string.IsNullOrEmpty(nameParts[1]))
			{
				conditions.Add($"e.FirstName = '{nameParts[1]}'");
			}

			if (nameParts.Length > 2 && !string.IsNullOrEmpty(nameParts[2]))
			{
				conditions.Add($"e.MiddleName = '{nameParts[2]}'");
			}

			return string.Join(" AND ", conditions);
		}
	}
}
