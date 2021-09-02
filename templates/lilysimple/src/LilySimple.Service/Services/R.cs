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
        public bool Success { get; set; }

        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("msg")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public object Data { get; set; }

        [JsonPropertyName("count")]
        public long? Count { get; set; }

        public static R Error(int code, string message)
        {
            return new R
            {
                Code = code,
                Message = message,
            };
        }

        public static R Ok(object data = null, long? count = null, string message = "Ok")
        {
            return new R
            {
                Success = true,
                Code = ErrorCode.Ok,
                Message = message,
                Data = data,
                Count = count,
            };
        }

        public static R Object<T>(T data) => Ok(data: data);

        public static R List<T>(IEnumerable<T> items) => Ok(data: items);

        public static R Page<T>(IEnumerable<T> items, long count) => Ok(data: items, count: count);
    }
}
