using Microsoft.EntityFrameworkCore;

using MusicHub.Data.Models;

namespace MusicHub.Data
{

    public class MusicHubDbContext : DbContext
    {
        public MusicHubDbContext()
        {

        }

        public MusicHubDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Performer> Performers { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<SongPerformer> SongsPerformers { get; set; }
        public DbSet<Writer> Writers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SongPerformer>(x =>
            {
                x.HasKey(x => new { x.PerformerId, x.SongId });
            });

    //    builder.Entity<SongPerformer>(x =>
    //    {
    //        x.HasOne(e => e.Song)
    //        .WithMany(s => s.SongPerformers)
    //        .HasForeignKey(e => e.SongId);
    //    });
    //
    //    builder.Entity<SongPerformer>(x =>
    //    {
    //        x.HasOne(e => e.Performer)
    //        .WithMany(s => s.PerformerSongs)
    //        .HasForeignKey(e => e.PerformerId);
    //    });
        }
    }
}
