using Evo.Scm.Samples;
using System;
using System.Collections.Generic;

namespace Evo.Scm.DesignStyles;

public class MobileDesignStyleInfoDto : MobileDesignStyleScanLogDto
{
    /// <summary>
    /// 设计款图片
    /// </summary>
    public string DesignStyleImage { get; set; }
    /// <summary>
    /// 设计款SN
    /// </summary>
    public string DesignStyleSn { get; set; }
    /// <summary>
    /// 样板类型
    /// </summary>
    public string SampleType { get; set; }
    /// <summary>
    /// 样衣状态
    /// </summary>
    public string SampleStatus { get; set; }
    /// <summary>
    /// 类目名称
    /// </summary>
    public string CategoryName { get; set; }
    /// <summary>
    /// 设计师名字
    /// </summary>
    public string DesignerName { get; set; }
    /// <summary>
    /// 要求交版日期
    /// </summary>
    public DateTime? RequireTime { get; set; }
    
}

public class MobileDesignStyleDetailDto : MobileDesignStyleInfoDto
{
    /// <summary>
    /// 加急
    /// </summary>
    public bool? IsUrgent { get; set; }
    /// <summary>
    /// 异常
    /// </summary>
    public bool? IsError { get; set; }

    /// <summary>
    /// 裁版师姓名
    /// </summary>
    public string CutterName { get; set; }
    /// <summary>
    /// 裁版完成时间
    /// </summary>
    public DateTime? CutFinishTime { get; set; }
    /// <summary>
    /// 版布幅宽
    /// </summary>
    public decimal? CutFabricWidth { get; set; }

    /// <summary>
    /// 二次加工跟进人
    /// </summary>
    public string? SecondaryProcessFollowerName { get; set; }
    /// <summary>
    /// 二次加工完成时间
    /// </summary>
    public DateTime? SecondaryProcessFinishTime { get; set; }

    /// <summary>
    /// 车版师姓名
    /// </summary>
    public string? TailorName { get; set; }
    /// <summary>
    /// 车版完成时间
    /// </summary>
    public DateTime? TailorFinishTime { get; set; }
    /// <summary>
    /// 车版件数
    /// </summary>
    public int? TailorQuantity { get; set; }

    /// <summary>
    /// 品牌Id
    /// </summary>
    public Guid BrandId { get; set; }

    /// <summary>
    /// 要求数量
    /// </summary>
    public int RequireQuantity { get; set; }

    /// <summary>
    /// 样衣图片
    /// </summary>
    public IEnumerable<UploadSampleImageDto> SampleImages { get; set; }
    /// <summary>
    /// 是否已上传正面图片（方便前端判断）
    /// </summary>
    public bool UploadImageFront { get; set; }
    /// <summary>
    /// 图案（花稿）（有/无）
    /// </summary>
    public string DrawingDraft { get; set; }
    /// <summary>
    /// 二次加工, (有/无, 来源于选项)
    /// </summary>
    public string SecondaryProcess { get; set; }
    /// <summary>
    /// 工厂Id
    /// </summary>
    public Guid? SupplierId { get; set; }
    
    /// <summary>
    /// 物料准备时间
    /// </summary>
    public DateTime? MaterialConfirmTime { get; set; }
    /// <summary>
    /// 二次加工准备时间
    /// </summary>
    public DateTime? SecondaryProcessPreparationTime { get; set; }
    /// <summary>
    /// 图案（花稿）确认时间
    /// </summary>
    public DateTime? DrawingDraftConfirmTime { get; set; }
    /// <summary>
    /// 确认发出时间
    /// </summary>
    public DateTime? GiveOutConfirmTime { get; set; }
    /// <summary>
    /// 允许先裁版
    /// </summary>
    public bool? AllowFirstCut { get; set; }
}
