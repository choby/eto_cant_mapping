using Evo.Scm.BrandIsolation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Evo.Scm.SupplierIsolation;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Users;

namespace Evo.Scm.EntityFrameworkCore;


public class DbContext<TDbContext> : AbpDbContext<TDbContext>
    where TDbContext : DbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    #endregion


    public DbContext(DbContextOptions<TDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //加载当前程序集的所有实体配置
        //这一行代码不能放到OnModelCreating后面，否则ShouldFilterEntity等相关方法不会被执行
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);


        /* Include modules to your migration db context */


        //var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
        //      .Where(x => !string.IsNullOrEmpty(x.Namespace))
        //      .Where(x => x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));
        //foreach (var type in typesToRegister)
        //{
        //    dynamic configurationInstance = Activator.CreateInstance(type);
        //    builder.ApplyConfiguration(configurationInstance
        //      );
        //}



        //builder.Entity<Author>(builder =>
        //{
        //    builder.ToTable("EvoAuthors");
        //    builder.ConfigureByConvention();
        //    builder.Property(x => x.Name)
        //       .IsRequired()
        //       .HasMaxLength(12);

        //    builder.HasIndex(x => x.Name);
        //});

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(ScmConsts.DbTablePrefix + "YourEntities", ScmConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }


    #region 品牌、供应商、当前用户隔离逻辑
    public ICurrentUser CurrentUser => LazyServiceProvider.LazyGetRequiredService<ICurrentUser>();
    protected virtual bool BrandIsolationFilterEnabled => DataFilter?.IsEnabled<IBrandIsolation>() ?? false;

    protected virtual bool SupplierIsolationFilterEnabled => (DataFilter?.IsEnabled<ISupplierIsolation<Guid>>() ?? false) || (DataFilter?.IsEnabled<ISupplierIsolation<Guid?>>() ?? false);
    

	public ICurrentUserBrandsAccessor CurrentUserBrandsAccessor => LazyServiceProvider.LazyGetRequiredService<ICurrentUserBrandsAccessor>();
    public ICurrentUserSuppliersAccessor CurrentUserSuppliersAccessor => LazyServiceProvider.LazyGetRequiredService<ICurrentUserSuppliersAccessor>();
    protected virtual Guid[] BrandIds => CurrentUserBrandsAccessor.BrandIds;
    protected virtual Guid[] SupplierIds => CurrentUserSuppliersAccessor.SupplierIds;

    private bool Isolated<TEntity>()
    {
        return this.IsolatedWithBrand<TEntity>() || this.IsolatedWithSupplier<TEntity>();
    }

    private bool IsolatedWithBrand<TEntity>()
    {
        return typeof(IBrandIsolation).IsAssignableFrom(typeof(TEntity));
    }

    private bool IsolatedWithSupplier<TEntity>()
    {
        return typeof(TEntity).GetInterfaces().Where(i => i.IsGenericType).Select(i => i.GetGenericTypeDefinition()).Any(i => i == typeof(ISupplierIsolation<>));
    }


	protected override bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType)
    {
        if (!CurrentUser.IsInRole(Authorization.Roles.Admin) && this.Isolated<TEntity>())
        {
            return true;
        }
        return base.ShouldFilterEntity<TEntity>(entityType);
    }

    protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
    {
        var expression = base.CreateFilterExpression<TEntity>();

        if (this.IsolatedWithBrand<TEntity>())
        {
            Expression<Func<TEntity, bool>> brandIsolationFilter =
                e => !BrandIsolationFilterEnabled || BrandIds.Contains(EF.Property<Guid>(e, "BrandId"));
            expression = expression == null ? brandIsolationFilter : CombineExpressions(expression, brandIsolationFilter);
        }

        if (this.IsolatedWithSupplier<TEntity>())
        {
            Expression<Func<TEntity, bool>> supplierIsolationFilter =
                e => !SupplierIsolationFilterEnabled || (!SupplierIds.Any() || SupplierIds.Contains(EF.Property<Guid>(e, "SupplierId")));
            expression = expression == null ? supplierIsolationFilter : CombineExpressions(expression, supplierIsolationFilter);
        }

        return expression;
    }
    #endregion

    
    
#if DEBUG
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
     //输出到控制台
     //.LogTo(System.Console.WriteLine, new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting })
     //输出到vs输出窗口
     .EnableSensitiveDataLogging()
     .LogTo((msg) => System.Diagnostics.Trace.WriteLine(msg), new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting })
     ;
        
    }
#endif
    
}
