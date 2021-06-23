using LilySimple.DataStructure.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LilySimple.Services
{
    public class Id
    {
        [JsonPropertyName("id")]
        public int Value { get; set; }

        public Id(int value)
        {
            Value = value;
        }
    }

    public class Flag
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; } = false;

        [JsonPropertyName("msg")]
        public string Msg { get; set; }

        public Flag Succeed(string msg = "ok")
        {
            Success = true;
            Msg = msg;
            return this;
        }

        public Flag Fail(string msg)
        {
            Success = false;
            Msg = msg;
            return this;
        }
    }

    public class Wrapped<T> : Flag
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }

        public Wrapped<T> Succeed(T data, string msg = "ok")
        {
            base.Succeed(msg);
            Data = data;
            return this;
        }
    }

    public class Listed<T> : Flag
    {
        [JsonPropertyName("items")]
        public IEnumerable<T> Items { get; set; }

        public Listed<T> Succeed(IEnumerable<T> items, string msg = "ok")
        {
            base.Succeed(msg);
            Items = items;
            return this;
        }
    }

    public class Paginated<T> : Listed<T>
    {
        [JsonPropertyName("count")]
        public long Count { get; set; }

        public Paginated<T> Succeed(IEnumerable<T> items, long count, string msg = "ok")
        {
            base.Succeed(msg);
            Items = items;
            Count = count;
            return this;
        }
    }
}
