using System.Text.Json;
using DataAccessLayer.Concrate;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace TraversalCoreProje.Infrastructure
{
    /// <summary>
    /// Erstellt/aktualisiert die SQLite-Datenbank über EF-Core-Migrationen
    /// und befüllt sie beim ersten Start mit Demo-Daten aus App_Data/seed/seed-data.json.
    /// </summary>
    public class DatabaseInitializer
    {
        private static readonly SemaphoreSlim _lock = new(1, 1);

        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Roll> _roleManager;
        private readonly IWebHostEnvironment _env;
        private readonly DemoOptions _demo;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(Context context, UserManager<User> userManager, RoleManager<Roll> roleManager,
            IWebHostEnvironment env, IOptions<DemoOptions> demo, ILogger<DatabaseInitializer> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
            _demo = demo.Value;
            _logger = logger;
        }

        public async Task InitializeAsync(bool reset = false)
        {
            await _lock.WaitAsync();
            try
            {
                if (reset)
                {
                    _logger.LogInformation("Demo-Datenbank wird zurückgesetzt …");
                    await _context.Database.EnsureDeletedAsync();
                    SqliteConnection.ClearAllPools();
                }

                await _context.Database.MigrateAsync();

                if (!await _context.Users.AnyAsync())
                {
                    await SeedAsync();
                    _logger.LogInformation("Demo-Daten wurden eingespielt.");
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        private async Task SeedAsync()
        {
            var path = Path.Combine(_env.ContentRootPath, "App_Data", "seed", "seed-data.json");
            var json = await File.ReadAllTextAsync(path);
            var data = JsonSerializer.Deserialize<SeedData>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            var now = DateTime.Now;

            // Rollen
            foreach (var r in data.Roles)
                if (!await _roleManager.RoleExistsAsync(r.Name))
                    await _roleManager.CreateAsync(new Roll(r.Name) { Description = r.Description });

            // Benutzer
            var users = new Dictionary<string, User>();
            int i = 0;
            foreach (var u in data.Users)
            {
                var user = new User
                {
                    UserName = u.Email,
                    Email = u.Email,
                    EmailConfirmed = true,
                    Name = u.Name,
                    Surname = u.Surname,
                    gender = u.Gender,
                    PhoneNumber = u.PhoneNumber,
                    Image = u.Image,
                    CreatedAt = now.AddDays(-120 + (i++ * 9))
                };
                var result = await _userManager.CreateAsync(user, _demo.DemoPassword);
                if (!result.Succeeded)
                    throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
                await _userManager.AddToRolesAsync(user, u.Roles);
                users[u.Key] = user;
            }

            var admin = users["admin"];

            // Inhalte
            var destinations = data.Destinations.Select(d => { d.Turlider = admin.Id; return d; }).ToList();
            _context.destinitons.AddRange(destinations);
            _context.guides.AddRange(data.Guides);
            _context.features.AddRange(data.Features);
            _context.features2.AddRange(data.Features2);
            _context.testsimonials.AddRange(data.Testimonials);
            _context.abouts.Add(data.About);
            _context.abouts2.Add(data.About2);
            _context.subabouts.AddRange(data.SubAbouts);
            _context.contacts.Add(data.Contact);
            _context.newsletters.AddRange(data.Newsletter.Select((m, n) => new Newsletter { Mail = m, SubscribeDate = now.AddDays(-n * 4 - 1) }));
            _context.contactMessages.AddRange(data.Messages.Select(m => new ContactMessage
            {
                Name = m.Name, Mail = m.Mail, Subject = m.Subject, Message = m.Message,
                IsRead = m.IsRead, MessageDate = now.AddDays(-m.DaysAgo).AddHours(-3)
            }));
            await _context.SaveChangesAsync();

            Destiniton City(string name) => destinations.First(d => d.City == name);

            foreach (var c in data.Comments)
            {
                var u = users[c.User];
                _context.comments.Add(new Comment
                {
                    Userid = u.Id,
                    CommentUser = $"{u.Name} {u.Surname}",
                    CommentContent = c.Text,
                    CommentData = now.AddDays(-c.DaysAgo).AddHours(-2),
                    Destinitonid = City(c.City).DestinationID,
                    status = c.Active
                });
            }

            foreach (var r in data.Reservations)
            {
                var u = users[r.User];
                var start = now.Date.AddDays(r.StartInDays);
                _context.reservitions.Add(new Reservition
                {
                    Userid = u.Id,
                    Username = $"{u.Name} {u.Surname}",
                    Destintionid = City(r.City).DestinationID,
                    HowmanyPapel = r.Persons,
                    ReservDate = now.AddDays(-r.BookedDaysAgo),
                    ReservStart = start,
                    ReservEnd = start.AddDays(r.Length),
                    status = r.Status,
                    Guidid = r.GuideIndex.HasValue ? data.Guides[r.GuideIndex.Value].GuideID : null
                });
            }
            await _context.SaveChangesAsync();
        }

        #region Seed-Modelle
        private class SeedData
        {
            public List<Destiniton> Destinations { get; set; }
            public List<Guide> Guides { get; set; }
            public List<Feature> Features { get; set; }
            public List<Feature2> Features2 { get; set; }
            public List<Testimonial> Testimonials { get; set; }
            public About About { get; set; }
            public About2 About2 { get; set; }
            public List<SubAbout> SubAbouts { get; set; }
            public Contact Contact { get; set; }
            public List<SeedRole> Roles { get; set; }
            public List<SeedUser> Users { get; set; }
            public List<SeedComment> Comments { get; set; }
            public List<SeedReservation> Reservations { get; set; }
            public List<SeedMessage> Messages { get; set; }
            public List<string> Newsletter { get; set; }
        }
        private class SeedRole { public string Name { get; set; } public string Description { get; set; } }
        private class SeedUser
        {
            public string Key { get; set; } public string Email { get; set; } public string Name { get; set; }
            public string Surname { get; set; } public string Gender { get; set; } public string PhoneNumber { get; set; }
            public string Image { get; set; } public List<string> Roles { get; set; }
        }
        private class SeedComment { public string User { get; set; } public string City { get; set; } public int DaysAgo { get; set; } public string Text { get; set; } public bool Active { get; set; } }
        private class SeedReservation
        {
            public string User { get; set; } public string City { get; set; } public int BookedDaysAgo { get; set; }
            public int StartInDays { get; set; } public int Length { get; set; } public int Persons { get; set; }
            public string Status { get; set; } public int? GuideIndex { get; set; }
        }
        private class SeedMessage { public string Name { get; set; } public string Mail { get; set; } public string Subject { get; set; } public string Message { get; set; } public int DaysAgo { get; set; } public bool IsRead { get; set; } }
        #endregion
    }
}
