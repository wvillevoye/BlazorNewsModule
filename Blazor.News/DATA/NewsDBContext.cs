using Blazor.News.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
namespace Blazor.News.Data
{
    public class NewsDBContext(DbContextOptions<NewsDBContext> options) : DbContext(options)
    {
       
        public DbSet<NieuwsArtikel> NieuwsArtikelen { get; set; }

        public DbSet<NieuwsCategorie> NieuwsCategorieen { get; set; }
        public DbSet<NieuwsInternalLink> NieuwsInternalLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


          

            modelBuilder.Entity<NieuwsInternalLink>()
                .HasOne(l => l.NieuwsArtikel)
                .WithMany(a => a.InterneLinks)
                .HasForeignKey(l => l.NieuwsArtikelId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
