using WorkTrack.Models;

namespace WorkTrack.Services.Interfaces
{
	public interface IEmployeeFilterService
	{
		List<Employee> GetFilteredEmployees(int positionId, int departmentId, string fullName);
	}
}
