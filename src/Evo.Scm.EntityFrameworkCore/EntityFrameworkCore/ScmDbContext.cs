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
using Volo.Abp.EntityFrameworkCore.DistributedEvents;
using Volo.Abp.Users;

namespace Evo.Scm.EntityFrameworkCore;


[ConnectionStringName("Default")]
public class ScmDbContext : DbContext<ScmDbContext>, IHasEventOutbox
{
    public ScmDbContext(DbContextOptions<ScmDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigureEventOutbox();
    }

    public DbSet<OutgoingEventRecord> OutgoingEvents { get; set; }
}
