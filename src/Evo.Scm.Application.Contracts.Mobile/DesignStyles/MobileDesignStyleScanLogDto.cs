using System;
using Volo.Abp.Application.Dtos;

namespace Evo.Scm.DesignStyles;

public class MobileDesignStyleScanLogDto : EntityDto<Guid>
{
    /// <summary>
    /// 设计款Id
    /// </summary>
    public Guid DesignStyleId { get; set; }
    /// <summary>
    /// 样衣Id
    /// </summary>
    public Guid SampleId { get; set; }
    /// <summary>
    /// 扫描人Id
    /// </summary>
    public Guid ScannerId { get; set; }
    /// <summary>
    /// 扫描人名字
    /// </summary>
    public string ScannerName { get; set; }
    /// <summary>
    /// 扫描时间
    /// </summary>
    public DateTime ScanTime { get; set; }
}
