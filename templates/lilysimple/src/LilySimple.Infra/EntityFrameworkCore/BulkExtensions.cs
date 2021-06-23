using LilySimple.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LilySimple.EntityFrameworkCore
{
    public static class BulkExtensions
    {
        public static void RemoveRange<TEntity>(this DbSet<TEntity> set, int[] keyValues)
            where TEntity : ModelBase
        {
            var entities = set.Where(i => keyValues.Contains(i.Id));
            set.RemoveRange(entities);
        }
    }
}
