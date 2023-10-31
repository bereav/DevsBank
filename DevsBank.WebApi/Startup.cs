using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using DevsBank.ApplicationServices;
using DevsBank.ApplicationServices.Validators;
using DevsBank.Domain.DomainValidators;
using DevsBank.Storage;

namespace DevsBank.WebApi;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services
            .AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
            })
            .AddApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

        services.ConfigureOptions<ConfigureSwaggerOptions>();
        services.AddTransient<IBankUserService, BankUserService>();
        services.AddSingleton<StorageContext>();
        services.AddTransient<IBankUserAccountService, BankUserAccountService>();
        services.AddTransient<IBankUserValidator, BankUserValidator>();
    }

    public void Configure(WebApplication webApplication, IWebHostEnvironment env)
    {
        webApplication.UseSwagger();
        var apiVersionDescriptionProvider = webApplication.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        webApplication.UseSwaggerUI(options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });

        webApplication.UseHttpsRedirection();

        webApplication.UseAuthorization();

        webApplication.MapControllers();
    }
}