using CommunityCares_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityCares_Web.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ApplicationDbContext()
        {
        }
        public virtual DbSet<Admin> Admins { get; set; }

        public virtual DbSet<Assylum> Assylums { get; set; }

        public virtual DbSet<Campaign> Campaigns { get; set; }

        public virtual DbSet<Collect> Collects { get; set; }

        public virtual DbSet<Donation> Donations { get; set; }

        public virtual DbSet<Donor> Donors { get; set; }

        public virtual DbSet<Image> Images { get; set; }

        public virtual DbSet<Person> People { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("workstation id=Hackaton.mssql.somee.com;packet size=4096;user id=Nekotina365_SQLLogin_1;pwd=Spartan387;data source=Hackaton.mssql.somee.com;persist security info=False;initial catalog=Hackaton");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdNavigation).WithOne(p => p.Admin)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Admin_Person");

                entity.HasOne(d => d.IdAssylumNavigation).WithMany(p => p.Admins)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Admin_Assylum");
            });

            modelBuilder.Entity<Assylum>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.HasOne(d => d.IdAssylumNavigation).WithMany(p => p.Campaigns)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Campaign_Assylum");
            });

            modelBuilder.Entity<Collect>(entity =>
            {
                entity.HasOne(d => d.IdCampaignNavigation).WithMany(p => p.Collects)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Collect_Campaign");
            });

            modelBuilder.Entity<Donation>(entity =>
            {
                entity.Property(e => e.IsAnonymus).IsFixedLength();
                entity.Property(e => e.IsReceived).IsFixedLength();

                entity.HasOne(d => d.IdCampaignNavigation).WithMany(p => p.Donations)
                .HasForeignKey(d => d.IdCampaign)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Donation_Campaign");
                entity.HasOne(d => d.IdCollectsNavigation).WithMany(p => p.Donations)
                .HasForeignKey(d => d.IdCollect)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Donation_Collect");

                entity.HasOne(d => d.IdDonnorsNavigation).WithMany(p => p.Donations)
                .HasForeignKey(d => d.IdDonnors)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Donation_Donor");
            });

            modelBuilder.Entity<Donor>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Donors");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdNavigation).WithOne(p => p.Donor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Donors_Person");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Images");

                entity.HasOne(d => d.IdCampaignNavigation).WithMany(p => p.Images).HasConstraintName("FK_Images_Campaign");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdNavigation).WithOne(p => p.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Person");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
