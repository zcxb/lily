using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Contexts
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<LilySimpleDbContext>
    {
        public LilySimpleDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LilySimpleDbContext>();
            builder.UseMySql("Server=localhost; Port=3306; Database=LilySimple; Uid=root; Pwd=123456;");
            return new LilySimpleDbContext(builder.Options);
        }
    }
}
