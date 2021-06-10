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

    public class Pager
    {
        [ModelBinder(Name = "page")]
        public int Page { get; set; } = 1;

        [ModelBinder(Name = "page_size")]
        public int PageSize { get; set; } = 20;
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
    }

    public class Listed<T> : Flag
    {
        [JsonPropertyName("items")]
        public IEnumerable<T> Items { get; set; }
    }

    public class Paginated<T> : Listed<T>
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
