using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PutawayDataAccess.Models;
using System.Collections.Generic;
using System.IO;

namespace DataAccess
{
    public class PutawayDbContext : DbContext
    {

        public virtual DbSet<wm_BinBalance> wm_BinBalance { get; set; }
        public virtual DbSet<wm_BinCard> wm_BinCard { get; set; }

        public virtual DbSet<View_InsertPutaway> View_InsertPutaway { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false);

                var configuration = builder.Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

                optionsBuilder.UseSqlServer(connectionString);
            }



        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
