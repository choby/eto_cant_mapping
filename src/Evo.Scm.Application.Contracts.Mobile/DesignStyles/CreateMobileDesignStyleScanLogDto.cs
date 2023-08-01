using System;
using System.ComponentModel.DataAnnotations;

namespace Evo.Scm.DesignStyles;

public class SaveMobileDesignStyleScanLogDto
{
    /// <summary>
    /// 设计款Id
    /// </summary>
    [Required]
    public Guid DesignStyleId { get; set; }
}
