using WorkTrack.Services.Interfaces;
using WorkTrack.Services;
using Taskmaster.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IAppCrudService, AppCrudService>();
builder.Services.AddScoped<IEmployeeFilterService, EmployeeFilterService>();
builder.Services.AddScoped<ISalaryReportService, SalaryReportService>();

builder.Services.AddMemoryCache();
builder.Services.AddMvc();

var app = builder.Build();

app.UseStaticFiles();

app.MapDefaultControllerRoute();
app.Run();
