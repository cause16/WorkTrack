using WorkTrack.ViewModels;

namespace WorkTrack.Services.Interfaces
{
	public interface ISalaryReportService
	{
		List<SalaryReportViewModel> GetSalaryReportForDepartments();
		List<SalaryReportViewModel> GetSalaryReportForPositions();
	}
}
