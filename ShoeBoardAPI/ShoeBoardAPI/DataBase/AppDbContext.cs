using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoeBoardAPI.Models;

namespace ShoeBoardAPI.DataBase
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Shoe> Shoes { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<ShoeCatalog> ShoeCatalogs { get; set; }
        public DbSet<UserShoeCatalog> UserShoeCatalogs { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Friend>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<FriendRequest>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.User)
                .WithMany(u => u.Friends)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.FriendUser)
                .WithMany()
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Shoe)
                .WithMany()
                .HasForeignKey(p => p.ShoeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<Comment>()
               .HasOne(c => c.User)
               .WithMany(u => u.Comments)
               .HasForeignKey(c => c.UserId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserShoeCatalog>()
                .HasOne(c => c.User)
                .WithMany(u => u.UserShoeCatalogs)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Shoe>()
                .HasOne(s => s.ShoeCatalog)
                .WithMany()
                .HasForeignKey(s => s.ShoeCatalogId);

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Requester)
                .WithMany()
                .HasForeignKey(fr => fr.RequesterId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Receiver)
                .WithMany()
                .HasForeignKey(fr => fr.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
