using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LilySimple.JsonConverters
{
    public class UnixTimestampConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                long timestamp = long.Parse(reader.GetDecimal() + "0000000"); // 与移动端约定以秒为单位
                return timestamp.ToDateTime();
            }

            return reader.GetDateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            if (!default(DateTime).Equals(value))
            {
                long timestamp = value.ToTimestamp() / 10000000;// 与移动端约定以秒为单位
                writer.WriteNumberValue((decimal)timestamp);
                return;
            }

            writer.WriteNumberValue(0.0d);
        }
    }
}
