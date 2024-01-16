using WorkTrack.Models;

namespace WorkTrack.ViewModels
{
	public class CompanyInfoViewModel
	{
		public string CompanyName { get; set; } = null!;

		public string PhoneNumber { get; set; } = null!;

		public Address Address { get; set; } = null!;
	}
}
