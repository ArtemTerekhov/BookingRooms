using Microsoft.EntityFrameworkCore;

namespace BookingRooms.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Order> Orders { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            User testUser = new User
            {
                UserId = 1,
                Initials = "Тестовый пользователь",
                Login = "Login",
                Password = "Password"
            };

            Room testRoom = new Room
            {
                RoomId = 1,
                Price = 100.50m,
                Description = "Тест"
               
            };

            modelBuilder.Entity<User>().HasData(new User[] { testUser });
            modelBuilder.Entity<Room>().HasData(new Room[] { testRoom });

            base.OnModelCreating(modelBuilder);
        }
    }
}

