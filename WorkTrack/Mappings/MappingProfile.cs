using AutoMapper;
using WorkTrack.Models;
using WorkTrack.ViewModels;

namespace Taskmaster.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CompanyInfo, CompanyInfoViewModel>();

        CreateMap<Address, AddressViewModel>().ReverseMap();

        CreateMap<Employee, EmployeeViewModel>().ReverseMap();
	}
}
