using LilySimple.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LilySimple.EntityFrameworkCore
{
    public static class EntityExtensions
    {
        public static IQueryable<TEntity> GetById<TEntity>(this IQueryable<TEntity> query, int id)
            where TEntity : ModelBase
        {
            return query.Where(i => i.Id == id);
        }
    }
}
