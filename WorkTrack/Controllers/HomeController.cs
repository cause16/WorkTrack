using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WorkTrack.Services.Interfaces;
using WorkTrack.ViewModels;

namespace WorkTrack.Controllers
{
	public class HomeController : Controller
	{
		private readonly IAppCrudService _appCrudService;
		private readonly IMemoryCache _cache;

		public HomeController(IAppCrudService appCrudService, IMemoryCache cache)
		{
			_appCrudService = appCrudService;
			_cache = cache;
		}

		[HttpGet]
		public IActionResult Index([FromServices] IMapper mapper)
		{
			if(_cache.TryGetValue("CompanyInfo", out CompanyInfoViewModel? model))
			{
				return View(model);
			}

			var companyInfo = _appCrudService.GetCompanyInfo();

			if(companyInfo == null)
				return NotFound();

			model = mapper.Map<CompanyInfoViewModel>(companyInfo);

			_cache.Set("CompanyInfo", model, TimeSpan.FromDays(1));

			return View(model);
		}
	}
}
