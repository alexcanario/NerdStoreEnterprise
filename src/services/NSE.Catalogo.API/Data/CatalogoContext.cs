﻿using Microsoft.EntityFrameworkCore;

using NSE.Catalogo.API.Models;
using NSE.Core.Data;

using System.Linq;
using System.Threading.Tasks;

namespace NSE.Catalogo.API.Data {
    public class CatalogoContext : DbContext, IUnitOfWork {
        public CatalogoContext(DbContextOptions<CatalogoContext> options) 
            : base(options) { }

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany( e 
                => e.GetProperties().Where(p => p.ClrType.Equals(typeof(string))))) {
                property.SetColumnType("varchar(100)");
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit() {
            return await SaveChangesAsync() > 0;
        }
    }
}
