using Blazor.News.Data;
using Blazor.News.DATA.Validators;
using Blazor.News.Services;
using Blazor.Shared.Editors;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.News.Configuratiom
{
    public class NewsDependencyInjection : IDependency
    {
        public void AddDependencies(IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<NewsDBContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("NewsDatabase")));

            services.AddValidatorsFromAssemblyContaining<NieuwsArtikelEditDtoValidator>();
            services.AddFluentValidationAutoValidation();
          
            services.AddScoped<INieuwsService, NieuwsService>();

        }


    }
}
