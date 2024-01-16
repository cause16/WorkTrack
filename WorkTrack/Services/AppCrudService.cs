using Dapper;
using Microsoft.Data.SqlClient;
using WorkTrack.Models;
using WorkTrack.Services.Interfaces;

namespace WorkTrack.Services
{
    public class AppCrudService : IAppCrudService
    {
        private readonly string? _connectionString;

        public AppCrudService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public CompanyInfo? GetCompanyInfo()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT ci.*, a.*
	                        FROM CompanyInfo AS ci
	                        JOIN [Addresses] AS a
	                        ON ci.AddressId = a.AddressId;";

                var companyInfo = connection.Query<CompanyInfo, Address, CompanyInfo>(
                    query,
                    (companyInfo, address) =>
                    {
                        companyInfo.Address = address;
                        return companyInfo;
                    },
                    splitOn: "AddressId");

                return companyInfo.FirstOrDefault();
            }
        }

        public List<Employee> GetAllEmployees()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT e.*, p.*, a.*, d.*
							  FROM Employees AS e
							  JOIN Positions AS p
							  ON e.PositionId = p.PositionId
							  JOIN Addresses AS a
							  ON e.AddressId = a.AddressId
							  JOIN Departments AS d
							  ON e.DepartmentId = d.DepartmentId;";

                var employees = connection.Query<Employee, Position, Address, Department, Employee>(
                    query,
                    (employee, position, address, department) =>
                    {
                        employee.Position = position;
                        employee.Address = address;
                        employee.Department = department;
                        return employee;
                    },
                    splitOn: "PositionId, AddressId, DepartmentId"
                );

                return employees.ToList();
            }
        }

        public Employee? GetEmployeeById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = $@"SELECT e.*, p.*, a.*, d.*
							  FROM Employees AS e
							  JOIN Positions AS p
							  ON e.PositionId = p.PositionId
							  JOIN Addresses AS a
							  ON e.AddressId = a.AddressId
							  JOIN Departments AS d
							  ON e.DepartmentId = d.DepartmentId
							  WHERE e.EmployeeId = {id};";

                var employee = connection.Query<Employee, Position, Address, Department, Employee>(
                    query,
                    (employee, position, address, department) =>
                    {
                        employee.Position = position;
                        employee.Address = address;
                        employee.Department = department;
                        return employee;
                    },
                    splitOn: "PositionId, AddressId, DepartmentId"
                );

                return employee.FirstOrDefault();
            }
        }

        public List<Department> GetDepartments()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var departments = connection.Query<Department>("SELECT * FROM Departments");

                return departments.ToList();
            }
        }

        public List<Position> GetPositions()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var positions = connection.Query<Position>("SELECT * FROM Positions");

                return positions.ToList();
            }
        }

        public void EditEmployeeInfo(Employee updatedEmployee)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var updateAddressQuery = @"UPDATE Addresses
                                    SET Country = @Country,
                                        City = @City,
                                        Street = @Street,
                                        HouseNumber = @HouseNumber,
                                        ApartmentNumber = @ApartmentNumber,
                                        PostIndex = @PostIndex
                                    WHERE AddressId = @AddressId";

                var updateEmployeeQuery = @"UPDATE Employees
                                     SET PositionId = @PositionId,
                                         DepartmentId = @DepartmentId,
                                         FirstName = @FirstName,
                                         LastName = @LastName,
                                         MiddleName = @MiddleName,
                                         PhoneNumber = @PhoneNumber,
                                         BirthDate = @BirthDate,
                                         HireDate = @HireDate,
                                         Salary = @Salary
                                     WHERE EmployeeId = @EmployeeId";

                connection.Execute(updateAddressQuery, updatedEmployee.Address);

                connection.Execute(updateEmployeeQuery, updatedEmployee);
            }
        }
    }
}
