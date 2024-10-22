using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoeBoardAPI.Models;

namespace ShoeBoardAPI.DataBase
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Shoe> Shoes { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<ShoeFeed> ShoeFeeds { get; set; }
        public DbSet<ShoeCatalog> ShoeCatalogs { get; set; }
        public DbSet<UserShoeCatalog> UserShoeCatalogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.User)
                .WithMany(u => u.Friends)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.FriendUser)
                .WithMany()
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Shoe>()
                .HasOne(s => s.User)
                .WithMany(u => u.Shoes)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Shoe>()
                .HasOne(s => s.ShoeCatalog)
                .WithMany()
                .HasForeignKey(s => s.ShoeCatalogId);

            modelBuilder.Entity<Shoe>()
                .HasOne(s => s.UserShoeCatalog)
                .WithMany()
                .HasForeignKey(s => s.UserShoeCatalogId);

            modelBuilder.Entity<ShoeFeed>()
                .HasOne(sf => sf.Shoe)
                .WithMany()
                .HasForeignKey(sf => sf.ShoeId);

            modelBuilder.Entity<ShoeFeed>()
                .HasOne(sf => sf.Friend)
                .WithMany()
                .HasForeignKey(sf => sf.FriendId);

            modelBuilder.Entity<UserShoeCatalog>()
                .HasOne(c => c.User)
                .WithMany(u => u.UserShoeCatalogs)
                .HasForeignKey(c => c.UserId);

        }
    }
}
