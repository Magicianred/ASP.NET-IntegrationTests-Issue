using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IntegrationTestsIssue
{
    public class BloggingContext : DbContext
    {
        private IConfiguration _configuration;

        public BloggingContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
    }
}
