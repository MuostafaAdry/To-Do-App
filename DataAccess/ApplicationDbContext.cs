using Microsoft.EntityFrameworkCore;
using To_Do_List.Models;

namespace To_Do_List.DataAccess
{
    public class ApplicationDbContext :DbContext
    {
        public DbSet<ToDo> ToDos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ToDo-List;Integrated Security=True;TrustServerCertificate=True\r\n");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
