using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.Models
{
    public class Pager
    {
        /// <summary>
        /// 页码，从1开始
        /// </summary>
        [ModelBinder(Name = "page")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// 分页大小，默认20
        /// </summary>
        [ModelBinder(Name = "page_size")]
        public int PageSize { get; set; } = 20;
    }
}
