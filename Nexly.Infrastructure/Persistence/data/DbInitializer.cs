using Microsoft.AspNetCore.Identity;
using Nexly.Domain;

namespace Nexly.Infrastructure.Persistence.data
{
    public class DbInitializer
    {
        public static async Task SeedData(NexlyDbContext context, UserManager<User> userManager)
        {
            var users = new List<User>
                {
                    new() {Id = "bob-id", DisplayName = "Bob", UserName = "bob@test.com", Email = "bob@test.com"},
                    new() {Id = "tom-id", DisplayName = "Tom", UserName = "tom@test.com", Email = "tom@test.com"},
                    new() {Id = "jane-id", DisplayName = "Jane", UserName = "jane@test.com", Email = "jane@test.com"}
                };

            if (!userManager.Users.Any())
            {
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }

            if (context.Articles.Any()) return;

            var articles = new List<Article>
            {
                new()
                {
                    Title = "Test article 1",
                    SourceName = "NewsAsia",
                    PublishedAt = DateTime.Now.AddDays(-1),
                    SourceUrl = "http://some-url",
                    Summary = "Summary of the article 1",
                },
                new()
                {
                    Title = "Test article 2",
                    SourceName = "NewsAsia",
                    PublishedAt = DateTime.Now.AddDays(-1),
                    SourceUrl = "http://some-url",
                    Summary = "Summary of the article 2",
                },
                new()
                {
                    Title = "Test article 3",
                    SourceName = "NewsAsia",
                    PublishedAt = DateTime.Now.AddDays(-1),
                    SourceUrl = "http://some-url",
                    Summary = "Summary of the article 3",
                }
            };

            context.Articles.AddRange(articles);

            await context.SaveChangesAsync();
        }
    }
}
