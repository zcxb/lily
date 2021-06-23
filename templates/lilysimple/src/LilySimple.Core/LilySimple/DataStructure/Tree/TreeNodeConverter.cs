using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LilySimple.DataStructure.Tree
{
    public class TreeNodeConverter : JsonConverter<TreeNode>
    {
        public override TreeNode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, TreeNode value, JsonSerializerOptions options)
        {
            var jsonString = JsonSerializer.Serialize(value, value.GetType(), options);
            JsonDocument.Parse(jsonString).WriteTo(writer);
        }
    }
}
