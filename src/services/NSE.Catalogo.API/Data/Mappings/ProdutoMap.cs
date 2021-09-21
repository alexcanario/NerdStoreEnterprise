using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using NSE.Catalogo.API.Models;

namespace NSE.Catalogo.API.Data.Mappings {
    public class ProdutoMap : IEntityTypeConfiguration<Produto> {
        public void Configure(EntityTypeBuilder<Produto> builder) {
            builder.ToTable("Produtos");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).HasMaxLength(250).IsRequired();
            builder.Property(p => p.Descricao).HasMaxLength(250).IsRequired();
            builder.Property(p => p.Imagem).HasMaxLength(250).IsRequired();
            builder.Property(p => p.Valor).HasColumnType("money");
        }
    }
}
