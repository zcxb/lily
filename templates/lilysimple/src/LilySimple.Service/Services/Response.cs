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
        public bool Success => _success;

        protected bool _success = false;

        [JsonPropertyName("msg")]
        public string Msg => _msg;

        protected string _msg;

        public Flag Succeed(string msg = "ok")
        {
            _success = true;
            _msg = msg;
            return this;
        }

        public Flag Fail(string msg)
        {
            _success = false;
            _msg = msg;
            return this;
        }
    }

    public class Wrapped<T> : Flag
    {
        [JsonPropertyName("data")]
        public T Data => _data;

        protected T _data;

        public Wrapped<T> Succeed(T data, string msg = "ok")
        {
            base.Succeed(msg);
            _data = data;
            return this;
        }
    }

    public class Listed<T> : Flag
    {
        [JsonPropertyName("items")]
        public IEnumerable<T> Items => _items;

        protected IEnumerable<T> _items;

        public Listed<T> Succeed(IEnumerable<T> items, string msg = "ok")
        {
            base.Succeed(msg);
            _items = items;
            return this;
        }
    }

    public class Paginated<T> : Listed<T>
    {
        [JsonPropertyName("count")]
        public long Count => _count;

        protected long _count;

        public Paginated<T> Succeed(IEnumerable<T> items, long count, string msg = "ok")
        {
            base.Succeed(msg);
            _items = items;
            _count = count;
            return this;
        }
    }
}
