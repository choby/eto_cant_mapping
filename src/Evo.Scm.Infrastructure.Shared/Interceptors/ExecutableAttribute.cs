namespace Evo.Scm.Interceptors;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class ExecutableAttribute : Attribute
{
    public string ActionName { get; set; }
}