using Volo.Abp.Modularity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.ApiExploring;
using System.Net;
using System.Text.Json.Serialization;
using Volo.Abp.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Evo.Scm.Filter;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.Data;
using Evo.Scm.BrandIsolation;
using Evo.Scm.Converter;
using Microsoft.Extensions.Hosting;
using Volo.Abp.Settings;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;
using Evo.Scm.Fakes;
using Volo.Abp.Security.Claims;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.BackgroundJobs;
using Evo.Feishu;
using Evo.Scm.SupplierIsolation;
using Volo.Abp.Json.SystemTextJson;

namespace Evo.Scm;


[DependsOn(typeof(AbpSettingManagementDomainModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(ScmInfrastructureSharedModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpBackgroundJobsModule),
    typeof(EvoFeishuApiModule))]
public class ScmInfrastructureModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {

        context.Services.AddControllers(
           o =>
           {
               //替换掉abp的错误过滤器
               o.Filters.ReplaceOne(x => (x as ServiceFilterAttribute)?.ServiceType.FullName == typeof(AbpExceptionFilter).FullName,
                   new ServiceFilterAttribute(typeof(ExceptionFilter)));
               o.Filters.Add(typeof(ApiResultFilterAttribute));
              
           }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
        });

        context.Services.Configure<AbpSystemTextJsonSerializerOptions>(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
        
        context.Services.Configure<AbpExceptionHandlingOptions>(options =>
        {
            options.SendStackTraceToClients = true; //发送错误堆栈到客户端
            options.SendExceptionsDetailsToClients = true; //发送错误详情到客户端
        });
        
        Configure<AbpRemoteServiceApiDescriptionProviderOptions>(options =>
        {
            options.SupportedResponseTypes.Clear();
            var statusCodes = new List<int>
            {

                (int) HttpStatusCode.Forbidden,
                (int) HttpStatusCode.Unauthorized,
                (int) HttpStatusCode.BadRequest,
                (int) HttpStatusCode.NotFound,
                (int) HttpStatusCode.NotImplemented,
                (int) HttpStatusCode.InternalServerError,
                (int) HttpStatusCode.OK
            };

            options.SupportedResponseTypes.AddIfNotContains(statusCodes.Select(statusCode => new ApiResponseType
            {
                Type = typeof(RemoteServiceErrorInfo),
                StatusCode = statusCode
            }));

        });


        if (context.Services.GetHostingEnvironment().IsEnvironment(RunEnvironments.Local))
        {
            Configure<AbpDataFilterOptions>(options =>
            {
                options.DefaultStates[typeof(IBrandIsolation)] = new DataFilterState(isEnabled: false);
                options.DefaultStates[typeof(ISupplierIsolation<Guid>)] = new DataFilterState(isEnabled: false);
                options.DefaultStates[typeof(ISupplierIsolation<Guid?>)] = new DataFilterState(isEnabled: false);
            });
        }
        else
        {

            Configure<AbpDataFilterOptions>(options =>
            {
                options.DefaultStates[typeof(IBrandIsolation)] = new DataFilterState(isEnabled: true);
                options.DefaultStates[typeof(ISupplierIsolation<Guid>)] = new DataFilterState(isEnabled: false); //供应商隔离全局关闭， 需要隔离的模块单独开启
                options.DefaultStates[typeof(ISupplierIsolation<Guid?>)] = new DataFilterState(isEnabled: false);
			});

        }
       
        Configure<SettingManagementOptions>(options =>
        {
            options.Providers.Clear();
        });
        Configure<AbpSettingOptions>(options =>
        {
           
            //配置在选项表中的业务配置项
            options.ValueProviders.Clear();
        });
      
      
    }
  
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        base.PostConfigureServices(context);

        var hostingEnvironment = context.Services.GetHostingEnvironment();
        if (hostingEnvironment.IsEnvironment(RunEnvironments.Local))
        {
#if DEBUG
            Configure<AbpTenantResolveOptions>(options =>
            {
                options.TenantResolvers.Clear();
                options.TenantResolvers.Add(new DebugTenantResolveContributor());
            });

           
            context.Services.Replace(ServiceDescriptor.Singleton<ICurrentPrincipalAccessor, DebugCurrentPrincipalAccessor>());
            context.Services.Replace(ServiceDescriptor.Singleton<ICurrentUserBrandsAccessor, DebugCurrentUserBrandsAccessor>());
            context.Services.Replace(ServiceDescriptor.Singleton<ITenantStore, DebugTenantStore>());
            context.Services.Replace(ServiceDescriptor.Singleton<ICurrentUserSuppliersAccessor, DebugUserSuppliersAccessor>());
#endif

        }

    }
}
