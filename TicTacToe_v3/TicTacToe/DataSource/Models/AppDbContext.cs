using Microsoft.EntityFrameworkCore;

namespace TicTacToe.DataSource.Models;

public class AppDbContext
(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserDTO> Users { get; set; }
    public DbSet<CurrentGameDTO> Games { get; set; }
    public DbSet<UserWinRatioDTO> UserWinRatios { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserWinRatioDTO>().HasNoKey();
    }
}