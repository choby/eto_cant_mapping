using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Evo.Scm.Samples;

public interface ISampleService : IApplicationService
{
    Task<SampleConfirmMaterialInfoDto> GetConfirmMaterialInfoAsync(Guid id);
    Task<SampleSecondaryProcessDto> GetSecondaryProcessAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> FillInSecondaryProcessInfo(Guid id, [FromBody] FillInSecondaryProcessInfoDto input);
    Task<PrepareDrawingDraftDto> GetPrepareDrawingDraftAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> PatchDrawingDraftAsync(Guid id, UpdateSampleDrawingDraftDto input, CancellationToken cancellationToken);
    Task<bool> FinishCutAsync(FinishCutDto input);
    Task<bool> FinishSecondaryProcessAsync(FinishSecondaryProcessDto input);
    Task<bool> FinishTailorAsync(FinishTailorDto input);
    Task<bool> PatchUploadSampleImagesAsync(Guid id, IEnumerable<FillInSampleProduceImageDto> input);
}

