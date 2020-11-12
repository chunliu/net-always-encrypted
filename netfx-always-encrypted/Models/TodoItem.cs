using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace netfx_always_encrypted.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class TodoContext : DbContext
    {
        public TodoContext() : base("TodoDBConnection")
        {

        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}