using System;
// using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Evo.Scm.Excel;

public  class ExcelColumn
{
    public string Format { get; set; }
    /// <summary>
    /// 列标题
    /// </summary>
    public string Title { get; set; }   
    /// <summary>
    /// 数据实体属性类型
    /// </summary>
    public Type MemberType { get; set; }

        
    /// <summary>
    /// 数据实体属性
    /// </summary>
    public string Member { get; set; }
    /// <summary>
    /// 列宽度，NPOI设置列宽度规则为： width * 1 / 256， 因为列最大显示字符为256，所以1 / 256 代表一个字符宽度
    /// </summary>
    public int Width { get; set; }
    /// <summary>
    /// 图片列
    /// </summary>
    public ExcelImage ExcelImage { get; set; }
}