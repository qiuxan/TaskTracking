using Microsoft.EntityFrameworkCore;
using TaskTracking.Models;

namespace TaskTracking.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {}
    
    public DbSet<TaskItem> Tasks { get; set; }
}