
using Evo.Scm.Modeling;
using Evo.Scm.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evo.Scm.EntityTypeConfigurations;

internal class SupplierPaymentInfoConfiguration : IEntityTypeConfiguration<SupplierPaymentInfo>
{
    public void Configure(EntityTypeBuilder<SupplierPaymentInfo> builder)
    {
        builder.ToTable(EvoScmDbProperties.DbTablePrefix + "SupplierPaymentInfos", EvoScmDbProperties.DbSchema);
        builder.Configure();
        builder.HasComment("供应商支付信息");
        builder.Property(x => x.SupplierId)
            .IsRequired();
        builder.Property(x => x.AccountType)
            .IsRequired();
        builder.Property(x => x.OpeningBank)
            .IsRequired()
            .HasMaxLength(24); 
        builder.Property(x => x.AccountName)
            .IsRequired()
            .HasMaxLength(24); 
        builder.Property(x => x.AccountNo)
            .IsRequired()
            .HasMaxLength(24);
        builder.Property(x => x.IsEnabled)
            .IsRequired();  
        builder.Property(x => x.IsDefault)
            .IsRequired();
    }
}