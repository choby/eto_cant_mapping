using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace Evo.Scm.Options
{
    public class OptionItemDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 选项Id
        /// </summary>
        public virtual Guid OptionId { get; set; }
        /// <summary>
        /// 选项Item值
        /// </summary>
        public virtual string Value { get; set; }
        /// <summary>
        /// 选项Item编码
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string? Description { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual float Sort { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public virtual string? Image { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public virtual DateTime? StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public virtual DateTime? EndDate { get; set; }
        /// <summary>
        /// 状态，场景值为0时，按此过滤
        /// </summary>
        public virtual bool IsEnabled { get; set; }
        /// <summary>
        /// 场景值为1时，按此过滤
        /// </summary>
        public virtual bool IsFiltration { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        public virtual string? Color { get; set; }
        /// <summary>
        /// 父Id
        /// </summary>
        public virtual Guid? ParentId { get; set; }
        /// <summary>
        /// 树结构深度
        /// </summary>
        public virtual short Level { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public ExtraPropertyDictionary ExtraProperties { get; set; }
        
    }
}
