namespace Evo.Scm.Excel;

[AttributeUsage(AttributeTargets.Property)]
public class ExcelColumnAttribute:Attribute
{
    public ExcelColumnAttribute()
    {
        //this.Title = title;
        // this.FontSize = new float?(fontSize);
        // this.Format = format;
        // this.IsBold = isBold;
        // this.IsAutoFit = isAutoFit;
        // this.AutoCenterColumn = autoCenterColumn;
        //this.Width = width;
    }
    
    /// <summary>显示名称</summary>
    public string Title { set; get; }

    public int Width { get; set; }

    public string Format { get; set; }
}