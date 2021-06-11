using LilySimple.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LilySimple.EntityFramework
{
    public static class EntityExtensions
    {
        public static TEntity GetById<TEntity>(this IQueryable<TEntity> query, int id)
            where TEntity : ModelBase
        {
            return query.FirstOrDefault(i => id.Equals(i.Id));
        }
    }
}
