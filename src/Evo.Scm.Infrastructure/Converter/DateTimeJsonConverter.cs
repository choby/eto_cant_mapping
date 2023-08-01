using System.Text.Json;
using System.Text.Json.Serialization;

namespace Evo.Scm.Converter;

/// <summary>
/// 设置Json默认DateTime格式化
/// </summary>
public class DateTimeJsonConverter : JsonConverter<DateTime>
{
	private readonly string format;
	public DateTimeJsonConverter()
	{
		this.format = "yyyy-MM-dd HH:mm:ss";
	}
	public override void Write(Utf8JsonWriter writer, DateTime date, JsonSerializerOptions options)
	{
		writer.WriteStringValue(date.ToString(format));
	}
	public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return DateTime.ParseExact(reader.GetString() ?? throw new InvalidOperationException(), format, null);
	}

}
