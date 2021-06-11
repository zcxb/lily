using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LilySimple.ViewModels
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

        public void SetSuccess(string msg = "ok")
        {
            Success = true;
            Msg = msg;
        }

        public void SetError(string msg)
        {
            Success = false;
            Msg = msg;
        }
    }

    public class Wrapped<T> : Flag
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }

        public void SetSuccess(T data, string msg = "ok")
        {
            base.SetSuccess(msg);
            Data = data;
        }
    }

    public class Listed<T> : Flag
    {
        [JsonPropertyName("items")]
        public IEnumerable<T> Items { get; set; }

        public void SetSuccess(IEnumerable<T> items, string msg = "ok")
        {
            base.SetSuccess(msg);
            Items = items;
        }
    }

    public class Paginated<T> : Listed<T>
    {
        [JsonPropertyName("count")]
        public long Count { get; set; }

        public void SetSuccess(IEnumerable<T> items, long count, string msg = "ok")
        {
            base.SetSuccess(msg);
            Items = items;
            Count = count;
        }
    }
}
