using RESTcats.Models;

namespace RESTcats.Services
{
    public static class DbSeeder
    {
        public static void SeedUsers(CatsDbContext context)
        {
            // Only seed if database is empty
            if (context.Users.Any())
                return;

            var users = new List<User>
            {
                new User
                {
                    Username = "admin",
                    PasswordHash = AuthService.HashPassword("1234"),
                    Role = "Admin"
                },
                new User
                {
                    Username = "anders",
                    PasswordHash = AuthService.HashPassword("Secret12"),
                    Role = "User"
                },
                new User
                {
                    Username = "henrik",
                    PasswordHash = AuthService.HashPassword("password"),
                    Role = "User"
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
