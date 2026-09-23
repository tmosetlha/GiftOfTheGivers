using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Donation> Donations { get; set; } = null!;
        public DbSet<Volunteer> Volunteers { get; set; } = null!;
        public DbSet<ReliefProject> ReliefProjects { get; set; } = null!;
        public DbSet<ReliefUpdate> ReliefUpdates { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // keeps Identity's own table configuration intact

            // ---- Donation ----
            builder.Entity<Donation>(entity =>
            {
                // Anonymous donation → DonorId stays NULL if the ApplicationUser is deleted.
                // We never want to lose the donation record itself.
                entity.HasOne(d => d.Donor)
                      .WithMany(u => u.Donations)
                      .HasForeignKey(d => d.DonorId)
                      .OnDelete(DeleteBehavior.SetNull);

                // If a project is removed, its donations remain (general fund) rather than vanish.
                entity.HasOne(d => d.ReliefProject)
                      .WithMany(p => p.Donations)
                      .HasForeignKey(d => d.ReliefProjectId)
                      .OnDelete(DeleteBehavior.SetNull);

                // Section 2.3: indexes on the fields most queried for reporting/filtering.
                entity.HasIndex(d => d.DonationDate);
                entity.HasIndex(d => d.Status);
            });

            // ---- Volunteer ----
            builder.Entity<Volunteer>(entity =>
            {
                // A user account being deleted shouldn't erase volunteer history.
                entity.HasOne(v => v.User)
                      .WithMany(u => u.VolunteerRegistrations)
                      .HasForeignKey(v => v.UserId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(v => v.Availability);
            });

            // ---- ReliefUpdate ----
            builder.Entity<ReliefUpdate>(entity =>
            {
                // Required FK — but still Restrict, not Cascade: an employee account
                // should not be deletable while they have authored updates on record.
                entity.HasOne(ru => ru.Employee)
                      .WithMany(u => u.PostedUpdates)
                      .HasForeignKey(ru => ru.EmployeeId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Updates are owned by their project — deleting the project
                // deletes its updates with it.
                entity.HasOne(ru => ru.ReliefProject)
                      .WithMany(p => p.ReliefUpdates)
                      .HasForeignKey(ru => ru.ReliefProjectId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}