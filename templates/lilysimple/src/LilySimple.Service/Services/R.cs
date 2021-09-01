using LilySimple.DataStructure.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LilySimple.Services
{
    public partial class ErrorCode
    {
        public const int Ok = 0;
        public const int Error = -1;
    }

    public class R
    {
        [JsonPropertyName("success")]
        public bool success { get; set; }

        [JsonPropertyName("code")]
        public int code { get; set; }

        [JsonPropertyName("msg")]
        public string message { get; set; }

        [JsonPropertyName("data")]
        public object data { get; set; }

        [JsonPropertyName("count")]
        public long? count { get; set; }

        public static R Error(int code, string message)
        {
            return new R
            {
                code = code,
                message = message,
            };
        }

        private static R Ok(object data = null, long? count = null, string message = "Ok")
        {
            return new R
            {
                success = true,
                code = ErrorCode.Ok,
                message = message,
                data = data,
                count = count,
            };
        }

        public static R Ok() => Ok();

        public static R Data<T>(T data) => Ok(data: data);

        public static R List<T>(IEnumerable<T> items) => Ok(data: items);

        public static R Page<T>(IEnumerable<T> items, long count) => Ok(data: items, count: count);
    }
}
