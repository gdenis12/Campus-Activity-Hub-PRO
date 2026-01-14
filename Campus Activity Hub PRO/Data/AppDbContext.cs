using Campus_Activity_Hub_PRO.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Campus_Activity_Hub_PRO.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<EventTag> EventTags => Set<EventTag>();
        public DbSet<Registration> Registrations => Set<Registration>();
        public DbSet<ErrorLog> ErrorLogs => Set<ErrorLog>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Category>(e =>
            {
                e.Property(x => x.Name).HasMaxLength(80).IsRequired();
                e.Property(x => x.Description).HasMaxLength(500);
                e.HasIndex(x => x.Name).IsUnique();
            });

            b.Entity<Event>(e =>
            {
                e.Property(x => x.Title).HasMaxLength(120).IsRequired();
                e.Property(x => x.Description).HasMaxLength(2000);
                e.Property(x => x.PosterPath).HasMaxLength(400);

                e.HasOne(x => x.Category)
                 .WithMany(c => c.Events)
                 .HasForeignKey(x => x.CategoryId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Organizer)
                 .WithMany()
                 .HasForeignKey(x => x.OrganizerId)
                 .OnDelete(DeleteBehavior.Restrict);


                e.HasQueryFilter(x => !x.IsDeleted);

                e.HasIndex(x => new { x.CategoryId, x.StartsAt });
                e.HasIndex(x => x.OrganizerId);
            });

            b.Entity<Tag>(e =>
            {
                e.Property(x => x.Name).HasMaxLength(40).IsRequired();
                e.HasIndex(x => x.Name).IsUnique();
            });

            b.Entity<EventTag>(e =>
            {
                e.HasKey(x => new { x.EventId, x.TagId });

                e.HasOne(x => x.Event)
                 .WithMany(ev => ev.EventTags)
                 .HasForeignKey(x => x.EventId);

                e.HasOne(x => x.Tag)
                 .WithMany(t => t.EventTags)
                 .HasForeignKey(x => x.TagId);
            });

            b.Entity<Registration>(e =>
            {
                e.Property(x => x.Comment).HasMaxLength(300);
                e.Property(x => x.Feedback).HasMaxLength(400);

                e.HasOne(x => x.Event)
                 .WithMany(ev => ev.Registrations)
                 .HasForeignKey(x => x.EventId);

                e.HasOne(x => x.User)
                 .WithMany()
                 .HasForeignKey(x => x.UserId);


                e.HasIndex(x => new { x.EventId, x.UserId }).IsUnique();

                e.HasIndex(x => x.RegistrationDate);
            });

            b.Entity<ErrorLog>(e =>
            {
                e.Property(x => x.Path).HasMaxLength(400).IsRequired();
                e.Property(x => x.UserEmail).HasMaxLength(120);
                e.Property(x => x.Message).HasMaxLength(2000).IsRequired();
            });
        }
    }
}
