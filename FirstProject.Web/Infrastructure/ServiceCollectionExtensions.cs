namespace FirstProject.Web.Infrastructure
{
    internal static class ServiceCollectionExtensions
    {
        public static void ConfigureAPIBase(this IServiceCollection services, 
            IConfiguration configuration)
        {
            RequestConfig.ArticlesAPIBase = configuration["ServiceUrls:ArticlesAPI"];
        }
    }
}
