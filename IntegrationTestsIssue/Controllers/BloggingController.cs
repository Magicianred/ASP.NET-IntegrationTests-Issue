using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTestsIssue.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BloggingController : ControllerBase
    {
        private readonly ILogger<BloggingController> _logger;
        private readonly BloggingContext _context;

        public BloggingController(ILogger<BloggingController> logger, BloggingContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Blog>> Get()
        {
            return await _context.Blogs.ToListAsync();
        }
    }
}
