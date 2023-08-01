using Evo.Scm.DesignStyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Evo.Scm.DesignStyles;

public interface IMobileDesignStyleScanLogService
{
    Task<IEnumerable<MobileDesignStyleInfoDto>> GetAsync(int pageIndex = 1, int pageSize = 10);
    Task<MobileDesignStyleDetailDto> GetDetailAsync(Guid? id, string designStyleSn, CancellationToken cancellationToken);
    Task<IEnumerable<DesignStyleSelectOptionDto>> GetDesignStyleSelectOptionDataAsync(string sn);
}
