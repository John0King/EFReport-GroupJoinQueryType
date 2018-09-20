using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GroupJoinQueryType.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Query<Student>(builder =>
            {
                builder.ToView("Students");
            });

        }

        public DbSet<StudentExtendField> StudentExtendFields { get; set; }
    }
}
