
using Evo.Scm.Modeling;
using Evo.Scm.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evo.Scm.EntityTypeConfigurations
{
    internal class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable(EvoScmDbProperties.DbTablePrefix + "Suppliers", EvoScmDbProperties.DbSchema);
            builder.Configure();
            builder.HasComment("供应商");
            builder.Property(x => x.Sn)
                .IsRequired()
                .HasMaxLength(64);
            builder.Property(x => x.Contact)
                .IsRequired()
                .HasMaxLength(24);
            builder.Property(x => x.ShortName)
                .IsRequired()
                .HasMaxLength(12);
            builder.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(32);
            builder.Property(x => x.Mobile)
                .IsRequired()
                .HasMaxLength(12);
            builder.Property(x => x.Province)
                .IsRequired()
                .HasMaxLength(24);
            builder.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(24);
            builder.Property(x => x.District)
                .IsRequired()
                .HasMaxLength(24);
            builder.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(x => x.IsEnabled)
               .IsRequired();
            builder.Property(x => x.ServiceClass)
               .IsRequired()
               .HasMaxLength(12);
            builder.Property(x => x.Level)
               .IsRequired()
               .HasMaxLength(12);
            builder.Property(x => x.ProductionClass)
               .IsRequired()
               .HasMaxLength(12);
            builder.Property(x => x.SpecializeIn)
               .HasMaxLength(256);
            builder.Property(x => x.SettlementMode)
               .HasMaxLength(12);
            builder.Property(x => x.TaxMode)
               .HasMaxLength(12);
            builder.Property(x => x.HaveInvoice);
            builder.HasMany<SupplierPaymentInfo>(x => x.SupplierPaymentInfos).WithOne().HasForeignKey(x => x.SupplierId);
        }
    }
}
