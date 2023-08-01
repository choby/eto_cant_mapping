using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Guids;

namespace Evo.Scm;

public class Global
{
    public static IServiceCollection ServiceCollection
    {
        get; set;
    }

    public const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public static Guid GenerateId()
    {
       return ServiceCollection.GetRequiredService<IGuidGenerator>().Create();
    }
}
