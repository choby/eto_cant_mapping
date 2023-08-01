using Casbin.Adapter.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;

namespace Evo.Scm.Casbin
{
    internal static class CasbinDbContextExtensions
    {
        public static void AddCasbinDbContext(this IServiceCollection services)
        {
            services.AddDbContext<CasbinDbContext<long>>((serviceProvider,builder)=>
            {
               var connectionString = serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("Default");
                builder.UseNpgsql(connectionString);
            });
        }
    }
}
