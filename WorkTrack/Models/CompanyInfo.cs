namespace WorkTrack.Models
{
    public class CompanyInfo
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public int AddressId { get; set; }
        public string PhoneNumber { get; set; } = null!;
		public Address Address { get; set; } = null!;
	}
}
