
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Volo.Abp.BlobStoring.Aliyun;
using Evo.Scm.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace Evo.Scm.Oss;

public static class AliyunBlobProviderConfigurationExtensions
{
    

    private static string EncodeBase64(string code_type, string code)
    {
        string encode = "";
        byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
        try
        {
            encode = Convert.ToBase64String(bytes);
        }
        catch
        {
            encode = code;
        }
        return encode;
    }

    private static string ComputeSignature(string key, string data)
    {
        using (var algorithm = new HMACSHA1())
        {
            algorithm.Key = Encoding.UTF8.GetBytes(key.ToCharArray());
            return Convert.ToBase64String(
                algorithm.ComputeHash(Encoding.UTF8.GetBytes(data.ToCharArray())));
        }
    }


    public static PolicyToken GetPolicyToken(this AliyunBlobProviderConfiguration configuration,string bucketname, string uploadDir)
    {
        var expireDateTime = DateTime.Now.AddSeconds(configuration.DurationSeconds);

        var config = new PolicyConfig();
        config.expiration = expireDateTime.ToIso8601();
        config.conditions = new List<List<Object>>();
        config.conditions.Add(new List<Object>());
        config.conditions[0].Add("content-length-range");
        config.conditions[0].Add(0);
        config.conditions[0].Add(1048576000);
        config.conditions.Add(new List<Object>());
        config.conditions[1].Add("starts-with");
        config.conditions[1].Add("$key");
        config.conditions[1].Add(uploadDir);

        var policy = JsonSerializer.Serialize(config);
        var policy_base64 = EncodeBase64("utf-8", policy);
        var signature = ComputeSignature(configuration.AccessKeySecret, policy_base64);

        var policyToken = new PolicyToken();
        
        policyToken.accessId = configuration.AccessKeyId;
        policyToken.host = $"https://{bucketname}.{configuration.Endpoint}";
        policyToken.policy = policy_base64;
        policyToken.signature = signature;
        policyToken.expire = expireDateTime.ToUnixTimeSeconds().ToString();

        policyToken.dir = uploadDir;

        return policyToken;
    }

    public static void AliyunOssOptions(this AbpBlobStoringOptions options,ServiceConfigurationContext context)
    {
        options.Containers.ConfigureDefault(async container =>
        {
            var settingProvider = context.Services.GetRequiredService<ISettingProvider>();
            // 从配置中读取oss配置, 可以从系统选项Config中读取,如果Config选项中不存在, 则降级读取appsettings.json
            var aliyun = await settingProvider.GetAsync<bool>("Aliyun.Oss", false); //是否使用阿里云oss
            if (aliyun)
                container.UseAliyun(async aliyun =>
                {
                    aliyun.AccessKeyId = await settingProvider.GetOrNullAsync("Aliyun.OSS.AccessKey.ID");
                    aliyun.AccessKeySecret = await settingProvider.GetOrNullAsync("Aliyun.Oss.AccessKey.Secret");
                    aliyun.Endpoint = await settingProvider.GetOrNullAsync("Aliyun.Oss.Endpoint");
                    aliyun.RegionId = await settingProvider.GetOrNullAsync("Aliyun.Oss.RegionID");
                    //aliyun.RoleArn = await settingProvider.GetOrNullAsync("Aliyun.Oss.RoleArn");
                    //aliyun.RoleSessionName = await settingProvider.GetOrNullAsync("Aliyun.Oss.RoleSessionName");
                    //aliyun.Policy = await settingProvider.GetOrNullAsync("Aliyun.Oss.Policy");
                    aliyun.DurationSeconds = await settingProvider.GetAsync<int>("Aliyun.Oss.DurationSeconds");
                    //aliyun.ContainerName = await settingProvider.GetOrNullAsync("Aliyun.Oss.ContainerName");
                    //aliyun.CreateContainerIfNotExists = await settingProvider.GetAsync<bool>("Aliyun.Oss.CreateContainerIfNotExists");
                });
        });
    }
}
