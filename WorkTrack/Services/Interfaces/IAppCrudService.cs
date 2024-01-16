using WorkTrack.Models;

namespace WorkTrack.Services.Interfaces
{
	public interface IAppCrudService
	{
		CompanyInfo? GetCompanyInfo();
		List<Employee> GetAllEmployees();
        Employee? GetEmployeeById(int id);
        List<Department> GetDepartments();
        List<Position> GetPositions();
        void EditEmployeeInfo(Employee employee);
    }
}
