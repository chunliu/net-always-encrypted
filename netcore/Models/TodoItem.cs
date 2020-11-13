using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class TodoContext : DbContext
    {
        public TodoContext() : base()
        {

        }

        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {

        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
