using EntityLayer.Concrate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrate
{
    /// <summary>
    /// EF-Core-Kontext der Anwendung (SQLite).
    /// Die Verbindung wird per Dependency Injection in Program.cs konfiguriert
    /// (ConnectionStrings:DefaultConnection → App_Data/traversal.db).
    /// </summary>
    public class Context : IdentityDbContext<User, Roll, int>
    {
        public const string DefaultConnection = "Data Source=App_Data/traversal.db";

        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<About> abouts { get; set; }
        public DbSet<About2> abouts2 { get; set; }
        public DbSet<Contact> contacts { get; set; }
        public DbSet<ContactMessage> contactMessages { get; set; }
        public DbSet<Destiniton> destinitons { get; set; }
        public DbSet<Feature> features { get; set; }
        public DbSet<Feature2> features2 { get; set; }
        public DbSet<Guide> guides { get; set; }
        public DbSet<Newsletter> newsletters { get; set; }
        public DbSet<SubAbout> subabouts { get; set; }
        public DbSet<Testimonial> testsimonials { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Reservition> reservitions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Reiseziel 1 - n Kommentare
            builder.Entity<Comment>()
                .HasOne(c => c.Destiniton)
                .WithMany(d => d.Comments)
                .HasForeignKey(c => c.Destinitonid)
                .OnDelete(DeleteBehavior.Cascade);

            // Benutzer 1 - n Kommentare
            builder.Entity<Comment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.Userid)
                .OnDelete(DeleteBehavior.Cascade);

            // Reiseziel 1 - n Reservierungen
            builder.Entity<Reservition>()
                .HasOne(r => r.Destiniton)
                .WithMany(d => d.reservitions)
                .HasForeignKey(r => r.Destintionid)
                .OnDelete(DeleteBehavior.Cascade);

            // Benutzer 1 - n Reservierungen
            builder.Entity<Reservition>()
                .HasOne(r => r.User)
                .WithMany(u => u.reservitions)
                .HasForeignKey(r => r.Userid)
                .OnDelete(DeleteBehavior.Cascade);

            // Reiseführer 1 - n Reservierungen (optional)
            builder.Entity<Reservition>()
                .HasOne(r => r.Guide)
                .WithMany(g => g.Reservations)
                .HasForeignKey(r => r.Guidid)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Newsletter>().HasIndex(n => n.Mail).IsUnique();
        }
    }
}
