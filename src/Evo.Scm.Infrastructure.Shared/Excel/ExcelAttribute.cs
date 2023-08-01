namespace Evo.Scm.Excel;

[AttributeUsage(AttributeTargets.Class)]
public class ExcelAttribute:Attribute
{
    /// <summary>名称(比如当前Sheet 名称)</summary>
    public string Name { get; set; }
}