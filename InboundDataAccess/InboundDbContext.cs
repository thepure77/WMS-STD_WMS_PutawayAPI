using GRDataAccess.Models;
using InboundDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace InboundDataAccess
{
    public class InboundDataAccessDbContext : DbContext
    {

        public virtual DbSet<im_GoodsReceiveItemLocation> im_GoodsReceiveItemLocation { get; set; }
        public virtual DbSet<wm_TagItem> wm_TagItem { get; set; }
        public virtual DbSet<View_TaskGR> View_TaskGR { get; set; }
        public virtual DbSet<View_TaskGR_Pallet> View_TaskGR_Pallet { get; set; }
        public virtual DbSet<im_TaskGR> im_TaskGR { get; set; }
        public virtual DbSet<im_TaskGRItem> im_TaskGRItem { get; set; }
        public virtual DbSet<View_ReportPutawayGR> View_ReportPutawayGR { get; set; }
        public virtual DbSet<im_GoodsReceive> im_GoodsReceive { get; set; }
        public virtual DbSet<wm_Tag> wm_Tag { get; set; }
        public virtual DbSet<View_TaskGR_Putaway> View_TaskGR_Putaway { get; set; }

        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false);

                var configuration = builder.Build();

                var connectionString = configuration.GetConnectionString("InboundDataAccess").ToString();

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
