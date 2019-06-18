using Microsoft.EntityFrameworkCore;
using ConsoleApplication.Entity;

namespace ConsoleApplication.Data
{
    public class WidgetsContext : DbContext
    {
        
        public WidgetsContext(DbContextOptions<WidgetsContext> options) : base(options)
        { }

        public DbSet<Widget> Widgets { get; set; }
    }
}
