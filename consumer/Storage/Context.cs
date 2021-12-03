using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitConsumer.Storage
{
    public class Context: DbContext
    {

        public DbSet<Element> Element { get; set; }

        public Context(DbContextOptions<Context> options)
            : base(options)
        { }
    }
}
