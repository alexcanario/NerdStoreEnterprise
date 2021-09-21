using Microsoft.Extensions.DependencyInjection;

using NSE.Catalogo.API.Data;
using NSE.Catalogo.API.Data.Repository;
using NSE.Catalogo.API.Models;

namespace NSE.Catalogo.API.Config {
    public static class DipConfig {
        public static void AddDipConfig(this IServiceCollection services) {
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<CatalogoContext>();
        }
    }
}
