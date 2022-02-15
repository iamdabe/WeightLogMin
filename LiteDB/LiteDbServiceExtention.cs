using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using LiteDB;

namespace LiteDB
{
    public static class LiteDbServiceExtention
    {
        public static void AddLiteDb(this IServiceCollection services, IConfiguration config)
        {
            var liteDbConfig = config.GetSection("LiteDbOptions");
            services.AddTransient<LiteDbContext, LiteDbContext>();
            services.Configure<LiteDbConfig>(liteDbConfig);
        }
    }
}