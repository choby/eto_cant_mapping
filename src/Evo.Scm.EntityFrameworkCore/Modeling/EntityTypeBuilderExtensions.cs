using Evo.Scm.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.EntityFrameworkCore.ValueComparers;
using Volo.Abp.EntityFrameworkCore.ValueConverters;

namespace Evo.Scm.Modeling
{
    internal static class EntityTypeBuilderExtensions
    {
        public static void Configure(this EntityTypeBuilder b)
        {
            b.TryConfigureConcurrencyStamp();
            b.TryConfigureExtraPropertiesToJsonb();
            b.TryConfigureObjectExtensions();
            b.TryConfigureMayHaveCreator();
            b.TryConfigureMustHaveCreator();
            b.TryConfigureSoftDelete();
            b.TryConfigureDeletionTime();
            b.TryConfigureDeletionAudited();
            b.TryConfigureCreationTime();
            b.TryConfigureLastModificationTime();
            b.TryConfigureModificationAudited();
            b.TryConfigureMultiTenant();

            //冗余创建人,修改人,删除人名字
            b.TryConfigureMayHaveCreatorName();
            b.TryConfigureModificationAuditedWithName();
            b.TryConfigureDeletionAuditedWithName();
        }
        /// <summary>
        /// 扩展字段类型对应到pgsql的json类型，虽然把pgsql的扩展字段设置成text也能正常工作， 但是就失去了pgsql利用sql对json执行curd的特性
        /// </summary>
        /// <param name="b"></param>
        private static void TryConfigureExtraPropertiesToJsonb(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo(typeof(IHasExtraProperties)))
            {
                b.Property<ExtraPropertyDictionary>(nameof(IHasExtraProperties.ExtraProperties))
                    .HasColumnName(nameof(IHasExtraProperties.ExtraProperties))
                    .HasColumnType("jsonb")
                    .HasConversion(new ExtraPropertiesValueConverter(b.Metadata.ClrType))
                    .Metadata.SetValueComparer(new ExtraPropertyDictionaryValueComparer());
            }
        }

        private static void TryConfigureMayHaveCreatorName(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IMayHaveCreatorName>())
            {
                b.Property(nameof(IMayHaveCreatorName.CreatorName))
                    .IsRequired(false)
                    .HasMaxLength(24)
                    .HasColumnName(nameof(IMayHaveCreatorName.CreatorName));
            }
        }

        private static void TryConfigureModificationAuditedWithName(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IModificationAuditedObjectWithUserName>())
            {
                b.Property(nameof(IModificationAuditedObjectWithUserName.LastModifierName))
                    .IsRequired(false)
                    .HasMaxLength(24)
                    .HasColumnName(nameof(IModificationAuditedObjectWithUserName.LastModifierName));
            }
        }

        private static void TryConfigureDeletionAuditedWithName(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IDeletionAuditedObjectWithUserName>())
            {
                b.Property(nameof(IDeletionAuditedObjectWithUserName.DeleterName))
                    .IsRequired(false)
                    .HasMaxLength(24)
                    .HasColumnName(nameof(IDeletionAuditedObjectWithUserName.DeleterName));
            }
        }
    }
}
