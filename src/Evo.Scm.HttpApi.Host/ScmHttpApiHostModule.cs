using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Consul;
using Evo.Scm.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Evo.Scm.EntityFrameworkCore;
using Evo.Scm.Oss;
using Evo.Scm.Security.Disguise;
using Evo.Scm.Suppliers;
using Evo.Scm.Swagger;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.Auditing;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Aliyun;
using Volo.Abp.MultiTenancy;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.RabbitMq;

namespace Evo.Scm;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpBlobStoringAliyunModule),
    typeof(AbpDistributedLockingModule),
    typeof(AbpEventBusRabbitMqModule),
    typeof(ScmHttpApiModule),
    typeof(ScmApplicationModule),
    typeof(ScmEntityFrameworkCoreModule),
    typeof(ScmInfrastructureModule),
    typeof(ScmInfrastructureSharedModule),
    typeof(ScmEventHandlersModule)
)]
[DependsOn(typeof(AbpDistributedLockingModule))]
public class ScmHttpApiHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        Configure<AbpAuditingOptions>(options =>
        {
            options.IsEnabled = true;
            options.EntityHistorySelectors.AddAllEntities();
        });

        ConfigureConventionalControllers();
        ConfigureAuthentication(context, configuration);
        //ConfigureLocalization();
        ConfigureCache(configuration);
        ConfigureVirtualFileSystem(context);
        ConfigureDataProtection(context, configuration, hostingEnvironment);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration, hostingEnvironment);
        ConfigureBlobStoring(context);
        ConfigureNotProdEnvAccess(context);
        ConfigureMultiTenants(context);
        ConfigureResponseCompression(context);
        ConfigureRedisCache(context);
        ConfigureDistributedLock(context);
        ConfigureConsul(context);
        ConfigureAntiForgery();
        ConfigureDistributedEventBus();
    }

    private void ConfigureDistributedEventBus()
    {
        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            //Enable for all entities
            //options.AutoEventSelectors.AddAll();

            //Enable for a single entity
            options.AutoEventSelectors.Add<Supplier>();
            // options.AutoEventSelectors.Add<Brand>();
            options.EtoMappings.Add<Supplier, SupplierEto>();
            
            //Enable for all entities in a namespace (and child namespaces)
            //options.AutoEventSelectors.AddNamespace("MyProject.Products");

            //Custom predicate expression that should return true to select a type
            // options.AutoEventSelectors.Add(
            //     type => type.Namespace.StartsWith("MyProject.")
            // );
        });
    }

    private void ConfigureAntiForgery()
    {
        Configure<AbpAntiForgeryOptions>(options =>
        {
            options.TokenCookie.Expiration = TimeSpan.FromDays(365);
            
        });
    }

    
    private void ConfigureResponseCompression(ServiceConfigurationContext context)
    {
        context.Services.AddResponseCompression(options => { options.EnableForHttps = true; });
    }

    private void ConfigureMultiTenants(ServiceConfigurationContext context)
    {
        Configure<AbpMultiTenancyOptions>(options => { options.IsEnabled = true; });

        //因为本项目没有租户表,所以启动时,从identityserver获取所有租户信息,否则的话系统会报错找不到租户
        Configure<AbpDefaultTenantStoreOptions>(async options =>
        {
            options.Tenants = new []{ new TenantConfiguration(new Guid(), "租户名称") };
        });
    }

    private void ConfigureNotProdEnvAccess(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        if (!hostingEnvironment.IsProduction())
            context.Services.AddAlwaysAllowAuthorization();
    }

    private void ConfigureCache(IConfiguration configuration)
    {
        Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "Scm:"; });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsEnvironment(RunEnvironments.Local))
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<ScmDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Evo.Scm.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<ScmDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Evo.Scm.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<ScmApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Evo.Scm.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<ScmApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Evo.Scm.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options => { options.ConventionalControllers.Create(typeof(ScmApplicationModule).Assembly, opts => { opts.RootPath = "scm"; }); });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.Audience = "scm_backend";
            });
        
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration, IWebHostEnvironment env)
    {
        if (env.IsEnvironment(RunEnvironments.Local) || env.IsDevelopment())
        {
            context.Services.AddAbpSwaggerGenWithOAuth(
                configuration["AuthServer:Authority"],
                new Dictionary<string, string>
                {
                    { "openid", "openid" },
                    { "profile", "profile" },
                    { "User", "User" },
                    { "role", "role" },
                    { "scm_backend", "scm_backend" } //swagger请求的权限范围
                },
                options =>
                {
                    options.SwaggerDocGroups();
                    // options.DocInclusionPredicate((docName, description) => true); //不去掉的话上面的分组无效, 所有接口都会出现在各个group中
                    options.CustomSchemaIds(type => type.FullName);
                    options.DocumentFilter<EnumDocumentFilter>();
                    options.IncludeXmlComments(Path.Combine(env.ContentRootPath, "Notes/EvoApplication.xml"), true);
                    options.IncludeXmlComments(Path.Combine(env.ContentRootPath, "Notes/EvoApplicationContracts.xml"), true);
                });
        }
    }


    private void ConfigureDataProtection(
        ServiceConfigurationContext context,
        IConfiguration configuration,
        IWebHostEnvironment hostingEnvironment)
    {
        //var dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName("Scm");
        //if (!hostingEnvironment.IsEnvironment(Environments.Local))
        //{
        //    var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
        //    dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "Scm-Protection-Keys");
        //}
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    private void ConfigureBlobStoring(ServiceConfigurationContext context)
    {
        Configure<AbpBlobStoringOptions>(options => { options.AliyunOssOptions(context); });
    }

    private void ConfigureRedisCache(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "evo_scm_cache_";
            options.GlobalCacheEntryOptions.SlidingExpiration = TimeSpan.FromMinutes(20);
        });
        Configure<RedisCacheOptions>(options => { options.InstanceName = "evo_scm_cache_"; });
    }

    private void ConfigureDistributedLock(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<AbpDistributedLockOptions>(options => { options.KeyPrefix = "evo_scm_lock_"; });
        context.Services.AddSingleton<IDistributedLockProvider>(sp =>
        {
            var connection = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
        });
    }
    
    private void ConfigureConsul(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IConsulClient, ConsulClient>();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsEnvironment(RunEnvironments.Local))
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        var supportedCultures = new[] { new CultureInfo("zh-CN") };
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("zh-CN"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });
        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseMultiTenancy();
        app.UseAuthorization();
        if (env.IsEnvironment(RunEnvironments.Local) || env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpointGroups();
                options.SwaggerEndpointCopyable();
                options.DefaultModelsExpandDepth(-1);
                var configuration = context.GetConfiguration();
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
                options.OAuthScopes("openid", "profile", "role", "User", "scm_backend");
            });
        }

        app.UseAuditing();
#if DEBUG
        app.UseAbpSerilogEnrichers();
#else
        app.UseSentryTracing(); // debug模式下不上报异常到sentry
#endif
        app.UseUnitOfWork();
        app.UseDisguiseUserMiddleware();
        app.UseConfiguredEndpoints();
        app.UseResponseCompression();
    }


    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        base.PostConfigureServices(context);
        Global.ServiceCollection = context.Services;
    }
}