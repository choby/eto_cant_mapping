namespace Evo.Scm.Excel;

[AttributeUsage(AttributeTargets.Property)]
public class ExcelImageAttribute:Attribute
{
    public int Width { get; set; }
    public int Height { get; set; }
}