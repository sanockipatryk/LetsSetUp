using LetsSetUp.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsSetUp.Data
{
    public class LetsSetUpContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public LetsSetUpContext(DbContextOptions<LetsSetUpContext> options) : base(options)
        { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SignUp> SignUps { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Event>()
            .HasIndex(b => b.Id)
            .IsUnique();

            modelBuilder.Entity<Category>()
            .HasIndex(b => b.Id)
            .IsUnique();

            modelBuilder.Entity<SignUp>()
            .HasIndex(b => b.Id)
            .IsUnique();

            modelBuilder.Entity<Comment>()
            .HasIndex(b => b.Id)
            .IsUnique();

            modelBuilder.Entity<Event>()
            .HasOne(e => e.Category)
            .WithMany(c => c.Events)
            .HasForeignKey(e => e.CategoryId);

            modelBuilder.Entity<Event>()
            .HasOne(e => e.AppUser)
            .WithMany(a => a.Events)
            .HasForeignKey(e => e.CreatedBy);

            modelBuilder.Entity<SignUp>()
            .HasOne(s => s.Event)
            .WithMany(e => e.SignUps)
            .HasForeignKey(e => e.EventId);

            modelBuilder.Entity<SignUp>()
           .HasOne(s => s.AppUser)
           .WithMany(e => e.SignUps)
           .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Comment>()
            .HasOne(c => c.AppUser)
            .WithMany(a => a.Comments)
            .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Comment>()
            .HasOne(c => c.Event)
            .WithMany(a => a.Comments)
            .HasForeignKey(e => e.EventId);

            modelBuilder.Entity<Notification>()
            .HasOne(c => c.AppUser)
            .WithMany(a => a.Notifications)
            .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Notification>()
            .HasOne(c => c.Event)
            .WithMany(a => a.Notifications)
            .HasForeignKey(e => e.EventId);
        }

    }
}
